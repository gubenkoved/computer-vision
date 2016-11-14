using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge;
using AForge.Video;
using Point = AForge.Point;

using ComputerVision;
using ComputerVision.Tracking;

namespace OpticalFlow
{
    public partial class MainForm : Form
    {
        public List<IOpticalFlowEstimator> TrackerAlgorithms { get; set; }

        VideoCaptureDevice _device;
        FiltersSequence _preprocessingFilters = new FiltersSequence(Grayscale.CommonAlgorithms.RMY);
        List<Point> _trackingPoints;
        IOpticalFlowEstimator _tracker;

        UnmanagedImage _currentSourceImage;
        UnmanagedImage _currentPreprocessedImage;
        UnmanagedImage _previousPreprocessedImage;

        Size _frameSize = new Size(320, 240);
        ResizeBilinear _resizer;

        Thread _workerThread;
        AutoResetEvent _frameRecievedEvent = new AutoResetEvent(false);

        public MainForm()
        {
            InitializeComponent();

            _resizer = new ResizeBilinear(_frameSize.Width, _frameSize.Height);

            TrackerAlgorithms = new List<IOpticalFlowEstimator>() 
            { 
                new LucasKanade(), 
                new BlockMatching(),
                new Farneback()
            };

            _trackingPoints = new List<Point>();

            _workerThread = new Thread(DoWorkerJob);
            _workerThread.Start();
        }

        private void DoWorkerJob()
        {
            // infinite cycle
            while (true)
            {
                _frameRecievedEvent.WaitOne();

                ProcessFrame();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainFormBindingSource.DataSource = this;
            _tracker = (IOpticalFlowEstimator)TrackerSelector.SelectedItem;
        }

        private void Invoke(Action action)
        {
            Invoke((Delegate)action);
        }

        private void OpenDeviceButton_Click(object sender, EventArgs e)
        {
            StopDevice();

            using (VideoCaptureDeviceForm deviceSelectorForm = new VideoCaptureDeviceForm())
            {
                if (deviceSelectorForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    VideoCaptureDevice device = deviceSelectorForm.VideoDevice;

                    StartDevice(device);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _workerThread.Abort();

            StopDevice();            
        }

        private void StartDevice(VideoCaptureDevice device)
        {
            _device = device;
            _device.NewFrame += Device_NewFrame;
            _device.Start();
        }

        private void StopDevice()
        {
            if (_device != null)
            {
                _device.NewFrame -= Device_NewFrame;
                
                _device.SignalToStop();
                _device.WaitForStop();
            }
        }
        
        private void UpdatePointPosition(int i, Point velosity)
        {
            float newX = _trackingPoints[i].X + velosity.X;
            float newY = _trackingPoints[i].Y + velosity.Y;

            float indention = 10f;

            newX = Math.Max(indention, Math.Min(_frameSize.Width - 1 - indention, newX));
            newY = Math.Max(indention, Math.Min(_frameSize.Height - 1 - indention, newY));

            _trackingPoints[i] = new Point(newX, newY);
        }

        private void Device_NewFrame(object sender, NewFrameEventArgs args)
        {
            //var time = DateTime.Now;
            _currentSourceImage = _resizer.Apply(UnmanagedImage.FromManagedImage(args.Frame));
            //MessageBox.Show((DateTime.Now - time).TotalMilliseconds.ToString("F4"));

            _frameRecievedEvent.Set();
        }

        private void ProcessFrame()
        {
            _currentPreprocessedImage = _preprocessingFilters.Apply(_currentSourceImage);

            UnmanagedImage sourceCopy = _currentSourceImage.Clone();
            UnmanagedImage output = UnmanagedImage.Create(sourceCopy.Width, sourceCopy.Height, sourceCopy.PixelFormat);

            if (_previousPreprocessedImage != null)
            {
                //output = _lucasKanade.ShowDerivative(preprocessed, _previous, LucasKanade.DerivativeComponent.X);

                #region Point tracking
                Point centroid = new Point();
                for (int i = 0; i < _trackingPoints.Count; ++i)
                {
                    Point point = _trackingPoints[i];
                    Point velocity = _tracker.CalculateVelocity(_currentPreprocessedImage, _previousPreprocessedImage, point.Round());

                    UpdatePointPosition(i, velocity);

                    centroid += _trackingPoints[i] / _trackingPoints.Count;

                    Drawing.Rectangle(sourceCopy, new Rectangle((int)point.X - 2, (int)point.Y - 2, 5, 5), Color.Yellow);
                }

                Drawing.Rectangle(sourceCopy, new Rectangle((int)centroid.X - 3, (int)centroid.Y - 3, 7, 7), Color.Red);
                #endregion

                DrawVelocityMap(output);
            }

            _previousPreprocessedImage = _currentPreprocessedImage;

            SourcePictureBox.Image = sourceCopy.ToManagedImage();
            PreprocessedPictureBox.Image = _currentPreprocessedImage.ToManagedImage();
            if (output != null) OutputPictureBox.Image = output.ToManagedImage();
        }

        private void DrawVelocityMap(UnmanagedImage output)
        {
            Point sumVelocity = new Point();
            
            int indention = 7;
            int step = 10;

            Invoke(() => step = VelocitiesDistanceTrackBar.Value);

            //Point[,] velocityField = ((dynamic)_tracker).CalculateVelocityField(_currentPreprocessedImage, _previousPreprocessedImage);

            for (int x = indention; x < _frameSize.Width - indention; x += step)
            {
                for (int y = indention; y < _frameSize.Height - indention; y += step)
                {
                    IntPoint p = new IntPoint(x, y);
                    Point velocity = _tracker.CalculateVelocity(_currentPreprocessedImage, _previousPreprocessedImage, p);
                    //Point velocity = velocityField[y, x];

                    sumVelocity += velocity;

                    if (velocity.EuclideanNorm() != 0.0f)
                    {
                        IntPoint newp = (p + velocity).Round();
                        Drawing.Line(output, p, newp, Color.White);
                        output.SetPixel(newp, Color.Red);
                    }
                }
            }

            Invoke(() => InfoLabel.Text = string.Format("Summary motion: ({0:+00.00;-00.00}; {1:+00.00;-00.00})", sumVelocity.X / 10, sumVelocity.Y / 10));
        }

        private void SourcePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _trackingPoints.Add(new IntPoint(e.X, e.Y));
        }

        private void TrackerSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _tracker = (IOpticalFlowEstimator)TrackerSelector.SelectedItem;
        }

    }
}
