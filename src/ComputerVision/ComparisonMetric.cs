using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging;
using System.Drawing;
using AForge;

namespace ComputerVision
{
    public abstract class ComparisonMetric
    {
        public float Calculate(UnmanagedImage imageA, UnmanagedImage imageB, Size areaSize, IntPoint startA, IntPoint startB)
        {
            if (imageA.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed
                || imageA.PixelFormat != imageB.PixelFormat)
                throw new ArgumentException("Both images must have 8 bit per pixel (grayscaled) format");

            if (areaSize.Width <= 0 || areaSize.Height <= 0)
                throw new ArgumentException("Area size is invalid");

            if (startA.X < 0 || startA.X + areaSize.Width > imageA.Width
                || startA.Y < 0 || startA.Y + areaSize.Height > imageA.Height
                || startB.X < 0 || startB.X + areaSize.Width > imageB.Width
                || startB.Y < 0 || startB.Y + areaSize.Height > imageB.Height)
                throw new ArgumentException("One or both start points is invalid");

            return CalculateImplementation(imageA, imageB, areaSize, startA, startB);
        }
        protected abstract float CalculateImplementation(UnmanagedImage imageA, UnmanagedImage imageB, Size areaSize, IntPoint startA, IntPoint startB);

        public static readonly ComparisonMetric SumOfSquaresDifferences = new SumOfSquaresDifferencesMetric();
        public static readonly ComparisonMetric NormalizedCorrelation = new NormalizedCorrelationMetric();

        #region Metrics
        class SumOfSquaresDifferencesMetric : ComparisonMetric
        {
            protected override unsafe float CalculateImplementation(UnmanagedImage imageA, UnmanagedImage imageB, Size areaSize, IntPoint startA, IntPoint startB)
            {
                float SSD = 0f;

                byte* ptrA = (byte*)imageA.ImageData.ToPointer();
                byte* ptrB = (byte*)imageB.ImageData.ToPointer();

                // allign pointer to the first pixel to process
                ptrA += startA.Y * imageA.Stride + startA.X;
                ptrB += startB.Y * imageB.Stride + startB.X;

                // offset translates pointer to next line and left side of area
                int offsetA = imageA.Stride - areaSize.Width;
                int offsetB = imageB.Stride - areaSize.Width;

                // for each line	
                for (int y = 0; y < areaSize.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < areaSize.Width; x++, ptrA++, ptrB++)
                    {
                        byte imageAByte = *ptrA;
                        byte imageBByte = *ptrB;

                        SSD += (imageAByte - imageBByte) * (imageAByte - imageBByte);
                    }

                    ptrA += offsetA;
                    ptrB += offsetB;
                }

                return SSD;
            }
        }
        class NormalizedCorrelationMetric : ComparisonMetric
        {
            protected override unsafe float CalculateImplementation(UnmanagedImage imageA, UnmanagedImage imageB, Size areaSize, IntPoint startA, IntPoint startB)
            {
                byte* ptrA = (byte*)imageA.ImageData.ToPointer();
                byte* ptrB = (byte*)imageB.ImageData.ToPointer();

                // allign pointer to the first pixel to process
                ptrA += startA.Y * imageA.Stride + startA.X;
                ptrB += startB.Y * imageB.Stride + startB.X;

                // offset translates pointer to next line and left side of area
                int offsetA = imageA.Stride - areaSize.Width;
                int offsetB = imageB.Stride - areaSize.Width;

                #region Calculating average
                float sumA = 0;
                float sumB = 0;

                // for each line	
                for (int y = 0; y < areaSize.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < areaSize.Width; x++, ptrA++, ptrB++)
                    {
                        sumA += *ptrA;
                        sumB += *ptrB;
                    }

                    ptrA += offsetA;
                    ptrB += offsetB;
                }

                byte avgA = (byte)Math.Round(sumA / (areaSize.Width * areaSize.Height));
                byte avgB = (byte)Math.Round(sumB / (areaSize.Width * areaSize.Height));
                #endregion

                #region Reset pointers
                // set pointers to start
                ptrA = (byte*)imageA.ImageData.ToPointer();
                ptrB = (byte*)imageB.ImageData.ToPointer();

                // allign pointer to the first pixel to process
                ptrA += startA.Y * imageA.Stride + startA.X;
                ptrB += startB.Y * imageB.Stride + startB.X; 
                #endregion

                #region Variance and covariation 
                // COV(X,Y) = SUM (X - avg(X))*(Y - avg(Y))
                float covariation = 0;
                // D[X] = SUM (X - avg(X))^2
                float varianceA = 0;
                float varianceB = 0;

                // for each line	
                for (int y = 0; y < areaSize.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < areaSize.Width; x++, ptrA++, ptrB++)
                    {
                        byte imageAByte = *ptrA;
                        byte imageBByte = *ptrB;
                        
                        covariation += (imageAByte - avgA) * (imageBByte - avgB);
                        varianceA += (imageAByte - avgA) * (imageAByte - avgA);
                        varianceB += (imageBByte - avgB) * (imageBByte - avgB);
                    }

                    ptrA += offsetA;
                    ptrB += offsetB;
                } 
                #endregion

                // COR(X,Y) = COV(X,Y) / SQRT(D[X]*D[Y])

                return (float)(covariation / Math.Sqrt(varianceA * varianceB));
            }
        }
        #endregion
    }
}
