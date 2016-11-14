using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using System.Drawing;

namespace ComputerVision
{
    public static class Helper
    {
        public static System.Drawing.Point ToGDIPoint(this AForge.Point point)
        {
            IntPoint intPoint = point.Round();

            return intPoint.ToGDIPoint();
        }

        public static System.Drawing.Point ToGDIPoint(this AForge.IntPoint point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }

        public static AForge.Point ToAForgePoint(this Size size)
        {
            return new AForge.Point(size.Width, size.Height);
        }

        public static AForge.IntPoint ToAForgePoint(this System.Drawing.Point point)
        {
            return new AForge.IntPoint(point.X, point.Y);
        }
    }
}
