using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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

        class CapabilityInfo
        {
            public string Name { get; set; }

            public VideoCapabilities Capability { get; set; }
        }

        public VideoCaptureDevice SelectedDevice { get; private set; }

        public OpenDeviceForm(Size size)
        {
            InitializeComponent();

            IEnumerable<DeviceInfo> devicesInfos = new FilterInfoCollection(FilterCategory.VideoInputDevice)
                .OfType<FilterInfo>()
                .Select(f => new DeviceInfo() { Name = f.Name, Device = new VideoCaptureDevice(f.MonikerString) })
                .Where(devInfo => devInfo.Device.VideoCapabilities != null && devInfo.Device.VideoCapabilities.Any())
                .ToArray();

            cbDevice.DisplayMember = "Name";

            foreach (DeviceInfo deviceInfo in devicesInfos)
            {
                cbDevice.Items.Add(deviceInfo);
            }

            if (devicesInfos.Count() > 0)
                cbDevice.SelectedIndex = 0;
        }

        private void DevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDevice = ((DeviceInfo)cbDevice.SelectedItem).Device;

            RebindCapabilities(SelectedDevice);
        }

        private void RebindCapabilities(VideoCaptureDevice device)
        {
            cbCapability.Items.Clear();

            foreach (var capability in device.VideoCapabilities)
            {
                cbCapability.Items.Add(new CapabilityInfo()
                {
                    Name = $"{capability.FrameSize.Width}x{capability.FrameSize.Height} ({capability.BitCount} bit) {capability.FrameRate.ToString("F0")} fps",
                    Capability = capability,
                });
            }

            cbCapability.SelectedIndex = 0;
        }

        private void cbCapability_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoCapabilities selectedCapability = ((CapabilityInfo)cbCapability.SelectedItem).Capability;

            cbCapability.DisplayMember = "Name";

            SelectedDevice.VideoResolution = selectedCapability;
        }
    }
}
