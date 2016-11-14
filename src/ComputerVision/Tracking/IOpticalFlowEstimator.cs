using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using AForge.Imaging;

namespace ComputerVision.Tracking
{
    public interface IOpticalFlowEstimator
    {
        Point CalculateVelocity(UnmanagedImage current, UnmanagedImage previous, IntPoint point);
        //Point[,] CalculateVelocityField(UnmanagedImage current, UnmanagedImage previous);
    }
}
