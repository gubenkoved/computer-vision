using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AForge.Imaging;
using AForge;

using Point = AForge.Point;
using AForge.Imaging.Filters;

namespace ComputerVision.Refined
{
    public class FeatureRegion
    {
        /// <summary>
        /// In range [0; 1]
        /// If conservatism is equal to 1 then feature image will not changed
        /// Otherwise if it equal to 0 feature image will overwritten on each detection
        /// </summary>
        public float Conservatism;

        /// <summary>
        /// In range [-1; 1]
        /// If normalized correlation coefficient will be less then threshold, the feature will be lost
        /// </summary>
        public float LostThreshold;

        public int MaxVelocity;

        public UnmanagedImage FeatureImage { get; private set; }

        public Size Size { get; private set; }

        public IntPoint Location { get; private set; }

        public bool IsLost { get; private set; }
        /// <summary>
        /// In range [-1; 1]
        /// Equals to normalized correlation on last feature detection
        /// </summary>
        public float Accuracy { get; private set; }

        public FeatureRegion(UnmanagedImage image, Size size, IntPoint location)
            :this(image, size, location, 0.92f, 0.8f, 10)
        {
        }

        public FeatureRegion(UnmanagedImage image, Size size, IntPoint location, float conservatism, float lostThreshold, byte maxMove)
        {
            Size = size;
            Location = location;
            Conservatism = conservatism;
            LostThreshold = lostThreshold;
            MaxVelocity = maxMove;

            Crop crop = new Crop(new Rectangle(location.ToGDIPoint(), size));
            FeatureImage = crop.Apply(image);
        }

        public unsafe void TrackFeature(UnmanagedImage image)
        {
            ComparisonMetric ncCalculator = ComparisonMetric.NormalizedCorrelation;
            float maxCorrelation = float.MinValue;
            IntPoint optimalLocation = new IntPoint();

            for (int dx = -MaxVelocity; dx <= MaxVelocity; dx += 1)
            {
                for (int dy = -MaxVelocity; dy <= MaxVelocity; dy += 1)
                {
                    IntPoint delta = new IntPoint(dx, dy);
                    IntPoint newLocalation = Location + delta;

                    // checking position
                    if (newLocalation.X < 0 || newLocalation.Y < 0 || newLocalation.X + Size.Width > image.Width || newLocalation.Y + Size.Height > image.Height)
                        continue;

                    float nolmalizedCorrelation = ncCalculator.Calculate(image, FeatureImage, Size, newLocalation, new IntPoint(0,0));

                    if (nolmalizedCorrelation > maxCorrelation)
                    {
                        maxCorrelation = nolmalizedCorrelation;
                        optimalLocation = newLocalation;
                    }
                }
            }

            // if success detection
            if (maxCorrelation > LostThreshold)
            {
                Accuracy = maxCorrelation;
                IsLost = false;
                Location = optimalLocation;

                // update feature image
                UpdateFeatureImage(image, Location);
            }
            else
            {
                IsLost = true;
            }
        }

        private unsafe void UpdateFeatureImage(UnmanagedImage image, IntPoint location)
        {
            byte* ptrImg = (byte*)image.ImageData.ToPointer();
            byte* ptrFeatureImg = (byte*)FeatureImage.ImageData.ToPointer();

            ptrImg += location.Y * image.Stride + location.X;

            int offsetImg = image.Stride - Size.Width;
            int offsetFeatureImg = FeatureImage.Stride - Size.Width;

            for (int y = 0; y < FeatureImage.Height; y++)
            {
                for (int x = 0; x < FeatureImage.Width; x++, ptrImg++, ptrFeatureImg++)
                {
                    *ptrFeatureImg = (byte)(Conservatism * (*ptrFeatureImg) + (1.0f - Conservatism) * (*ptrImg));
                }

                ptrImg += offsetImg;
                ptrFeatureImg += offsetFeatureImg;
            }
        }
    }
}
