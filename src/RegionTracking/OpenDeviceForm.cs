using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;

namespace RegionTracking
{
    public partial class OpenDeviceForm : Form
    {
        class DeviceInfo
        {
            public string Name { get; set; }
            public VideoCaptureDevice Device { get; set; }
        }
        
        public VideoCaptureDevice SelectedDevice { get; private set; }
        private Size _size;

        public OpenDeviceForm(Size size)
        {
            InitializeComponent();

            _size = size;

            ModeLabel.Text = string.Format("{0}x{1}", size.Width, size.Height);
            
            var devicesInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice)
                .OfType<FilterInfo>()
                .Select(f => new DeviceInfo (){ Name = f.Name, Device = new VideoCaptureDevice(f.MonikerString) })
                .Where(devInfo => devInfo.Device.VideoCapabilities != null && devInfo.Device.VideoCapabilities.Where(vc => vc.FrameSize == size).Count() > 0);

            DevicesComboBox.DisplayMember = "Name";

            foreach (var deviceInfo in devicesInfo)
            {
                DevicesComboBox.Items.Add(deviceInfo);
            }

            if (devicesInfo.Count() > 0)
                DevicesComboBox.SelectedIndex = 0;
        }

        private void DevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDevice = ((DeviceInfo)DevicesComboBox.SelectedItem).Device;

            SelectedDevice.DesiredFrameSize = _size;
        }
    }
}
