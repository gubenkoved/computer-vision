using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging;
using System.Drawing;

namespace ComputerVision
{
    public abstract class RegionMetric
    {
        public float Calculate(UnmanagedImage image, Rectangle rect)
        {
            if (rect.X < 0 || rect.X + rect.Width > image.Width
                || rect.Y < 0 || rect.Y + rect.Height > image.Height)
                throw new ArgumentException("Invalid rectangle");

            return CalculateImplementation(image, rect);
        }
        protected abstract float CalculateImplementation(UnmanagedImage image, Rectangle rect);

        public static readonly RegionMetric Variance = new VarianceMetric();

        #region Metrics
        class VarianceMetric : RegionMetric
        {
            protected unsafe override float CalculateImplementation(UnmanagedImage image, Rectangle rect)
            {
                byte* ptr = (byte*)image.ImageData.ToPointer();

                // allign pointer to the first pixel to process
                ptr += rect.Y * image.Stride + rect.X;

                // offset translates pointer to next line and left side of area
                int offset = image.Stride - rect.Width;

                float sum = 0f;
                // for each line	
                for (int y = 0; y < rect.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, ptr++)
                    {
                        sum += *ptr;
                    }

                    ptr += offset;
                }

                // reset pointer
                ptr = (byte*)image.ImageData.ToPointer();
                ptr += rect.Y * image.Stride + rect.X;

                float avg = sum / (rect.Width * rect.Height);
                float variance = 0f;

                for (int y = 0; y < rect.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < rect.Width; x++, ptr++)
                    {
                        // D[X] = SUM (X - avg(X))^2
                        variance += (*ptr - avg) * (*ptr - avg);
                    }

                    ptr += offset;
                }

                return variance;
            }
        }
        #endregion
    }
}
