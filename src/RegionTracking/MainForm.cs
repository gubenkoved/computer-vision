using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using ComputerVision;
using ComputerVision.Refined;
using Point = AForge.Point;

namespace RegionTracking
{
    public partial class MainForm : Form
    {
        private VideoCaptureDevice _device;
        private Size _frameSize;

        private FiltersSequence _normalizatorFilters;
        private FiltersSequence _preprocessingFilters;

        private UnmanagedImage _currentUnprocessed;
        private UnmanagedImage _currentProcessed;
        private UnmanagedImage _previousProcessed;

        private FeatureRegion _featureRegion;

        private bool _regionSelectionMode;
        private Rectangle _selectedRegion;

        private int _frameCount;
        private DateTime _lastFPSUpdate;

        private Thread _workerThread;
        private AutoResetEvent _frameRecievedEvent = new AutoResetEvent(false);

        public MainForm()
        {
            InitializeComponent();

            Init();

            _workerThread = new Thread(DoWorkerJob);
            _workerThread.Start();
        }

        private void Init()
        {
            _frameSize = new Size(ImageBox.Width, ImageBox.Height);

            _normalizatorFilters = new FiltersSequence(
                new ResizeBilinear(_frameSize.Width, _frameSize.Height));

            _preprocessingFilters = new FiltersSequence(
                Grayscale.CommonAlgorithms.RMY);
        }

        private void Invoke(Action action)
        {
            Invoke((Delegate)action);
        }

        private void DoWorkerJob()
        {
            // infinite cycle
            while (true)
            {
                _frameRecievedEvent.WaitOne();

                ProcessFrameByRefindedMethod();
            }
        }

        private void OpenDeviceButton_Click(object sender, EventArgs e)
        {
            if (_device != null)
            {
                _device.Stop();
                _device.WaitForStop();
            }

            //OpenDeviceButton.Enabled = false;

            using (OpenDeviceForm openDeviceForm = new OpenDeviceForm(_frameSize))
            {
                if (openDeviceForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openDeviceForm.SelectedDevice != null)
                    {
                        _device = openDeviceForm.SelectedDevice;

                        _device.NewFrame -= Device_NewFrame;
                        _device.NewFrame += Device_NewFrame;

                        _device.Start();
                    }
                }
            }
        }

        private void Device_NewFrame(object sender, NewFrameEventArgs args)
        {
            if (_regionSelectionMode)
                return;

            UnmanagedImage rawFrame = UnmanagedImage.FromManagedImage(args.Frame);

            UnmanagedImage normalized = _normalizatorFilters.Apply(rawFrame);

            _currentUnprocessed = normalized;

            // allows worker thread to do the job
            _frameRecievedEvent.Set();

            if ((DateTime.Now - _lastFPSUpdate).TotalMilliseconds > 1000)
            {
                Invoke(() => FPSLabel.Text = string.Format("{0:F2} fps", _frameCount / (DateTime.Now - _lastFPSUpdate).TotalSeconds));

                _lastFPSUpdate = DateTime.Now;
                _frameCount = 0;
            }
        }
        
        private void ProcessFrameByRefindedMethod()
        {
            ++_frameCount;

            UnmanagedImage sourceCopy = _currentUnprocessed.Clone();

            _currentProcessed = _preprocessingFilters.Apply(sourceCopy);

            if (_previousProcessed != null && _featureRegion != null)
            {
                // track the feature
                _featureRegion.TrackFeature(_currentProcessed);

                // update cursor position
                UpdateCursor();

                // draw feature region
                Rectangle rect = new Rectangle(_featureRegion.Location.ToGDIPoint(), _featureRegion.Size);
                if (_featureRegion.IsLost)
                    Drawing.Rectangle(sourceCopy, rect, Color.Red);
                else
                    Drawing.Rectangle(sourceCopy, rect, Color.Green);

                RegionPictureBox.Image = _featureRegion.FeatureImage.ToManagedImage();
                Invoke(() => AccuracyLabel.Text = string.Format("Last detection accuracy: {0:F4}", _featureRegion.Accuracy));
            }
            
            _previousProcessed = _currentProcessed;

            ImageBox.Image = sourceCopy.ToManagedImage();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _workerThread.Abort();

            if (_device != null)
            {
                _device.SignalToStop();
                _device.WaitForStop();
            }
        }
        
        private void ImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_device != null)
            {
                _regionSelectionMode = true;
                _selectedRegion = new Rectangle(e.Location, new Size());
            }
        }

        private void ImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_regionSelectionMode)
            {
                _selectedRegion = new Rectangle(_selectedRegion.Location, new Size(e.X - _selectedRegion.X, e.Y - _selectedRegion.Y));

                UnmanagedImage img = _currentProcessed.Clone();
                Drawing.Rectangle(img, _selectedRegion, Color.White);

                ImageBox.Image = img.ToManagedImage();
            }
        }

        private void ImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_device != null)
            {
                _featureRegion = new FeatureRegion(
                    _currentProcessed, 
                    _selectedRegion.Size, 
                    _selectedRegion.Location.ToAForgePoint());

                // updating parameters
                Parameters_Changed(this, e);

                _regionSelectionMode = false;
            }
        }

        private void Parameters_Changed(object sender, EventArgs e)
        {
            if (_featureRegion != null)
            {
                float conservatism = (ConservativityTrackBar.Value / (1.0f * ConservativityTrackBar.Maximum));
                float accuracyThresold = (AccuracyTrackBar.Value / (1.0f * AccuracyTrackBar.Maximum));
                byte maxVelocity = (byte)MaxVelocityTrackBar.Value;

                _featureRegion.Conservatism = conservatism;
                _featureRegion.LostThreshold = accuracyThresold;
                _featureRegion.MaxVelocity = maxVelocity;
            }
        }

        private IntPoint _featureCenterPostion;
        private Size _screenResolution = Screen.PrimaryScreen.Bounds.Size;
        private void ControlCursorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _lastDeltas.Clear();

            if (ControlCursorCheckBox.Checked)
            {
                if (_featureRegion != null)
                {
                    _featureCenterPostion = _featureRegion.Location;
                    _idleStart = DateTime.Now;
                }
            }
        }

        private IntPoint _previousFeaturePointLocation;
        private List<Point> _lastDeltas = new List<Point>();
        private int _valuablePointsNumber = 15;
        private float _idleThreshold = 0.2f;
        private DateTime _idleStart;
        private float _idleSecondToClick = 1.5f;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private void UpdateCursor()
        {
            if (ControlCursorCheckBox.Checked && _featureRegion != null && !_featureRegion.IsLost)
            {
                if (AbsolutePositioningCheckBox.Checked)
                {
                    IntPoint delta = new IntPoint(
                    -_featureRegion.Location.X + _featureCenterPostion.X,
                    +_featureRegion.Location.Y - _featureCenterPostion.Y);

                    float speedFactor = 2.6f;

                    IntPoint screenCursorDelta = new IntPoint(
                        (int)(speedFactor * delta.X * _screenResolution.Width / _frameSize.Width),
                        (int)(speedFactor * delta.Y * _screenResolution.Height / _frameSize.Height));

                    IntPoint centerScreen = new IntPoint(_screenResolution.Width / 2, _screenResolution.Height / 2);

                    IntPoint newPosition = new IntPoint(
                        (int)Math.Max(0, Math.Min(_screenResolution.Width - 1, centerScreen.X + screenCursorDelta.X)),
                        (int)Math.Max(0, Math.Min(_screenResolution.Height - 1, centerScreen.Y + screenCursorDelta.Y)));

                    Cursor.Position = newPosition.ToGDIPoint();
                }
                else
                {
                    // inversing X axis
                    Point delta = new Point(
                        -_featureRegion.Location.X + _previousFeaturePointLocation.X,
                        _featureRegion.Location.Y - _previousFeaturePointLocation.Y);

                    _lastDeltas.Add(delta);

                    if (_lastDeltas.Count > _valuablePointsNumber)
                        _lastDeltas.RemoveAt(0);

                    Point sumDelta = _lastDeltas.Aggregate<Point, Point>(new Point(), (sum, p) => sum + p);
                    Point avgDelta = sumDelta / _lastDeltas.Count;

                    IntPoint screenCursorDelta = (avgDelta * 10.0f * (float)Math.Pow(avgDelta.EuclideanNorm(), 0.8)).Round();

                    IntPoint newPosition = Cursor.Position.ToAForgePoint() + screenCursorDelta;

                    Cursor.Position = newPosition.ToGDIPoint();

                    if (_lastDeltas.Count == _valuablePointsNumber)
                    {
                        if (avgDelta.EuclideanNorm() > _idleThreshold)
                            _idleStart = DateTime.Now;

                        if ((DateTime.Now - _idleStart).TotalSeconds > _idleSecondToClick)
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                            _idleStart = DateTime.Now;
                        }
                    }
                }
            }

            _previousFeaturePointLocation = _featureRegion.Location;
        }

        
    }
}
