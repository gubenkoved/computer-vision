using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using AForge.Imaging;

using Point = AForge.Point;
using System.Drawing;

namespace ComputerVision.Tracking
{
    public struct Derivative3
    {
        public float X;
        public float Y;
        public float t;

        public Derivative3(float x, float y, float t)
            :this()
        {
            this.X = x;
            this.Y = y;
            this.t = t;
        }
    }

    public class LucasKanade : IOpticalFlowEstimator
    {
        private readonly int _winXRadius;
        private readonly int _winYRadius;

        /// <summary>
        /// Influens on pixels' weight when using weighted window method
        /// (uses exponential decreasing weight from 1 in central pixel to exp(beta) in peripheral pixels)
        /// Suitable range is [-10; -1]
        /// Sigher values give the more robust algorithm, but it may hurt a little sensitivity
        /// (recommended value: -3.0f)
        /// </summary>
        private const float _beta = -3.0f;
        private readonly Func<float, float, float> _weightFunction;
            
        /// <summary>
        /// Precomputed weight values
        /// w(dx, dy) = _weight[ |dx|, |dy| ]
        /// (performance optimisation)
        /// </summary>
        private float[,] _weight;
        private void FillWeightArray()
        {
            _weight = new float[_winXRadius + 1, _winYRadius + 1];

            for (int dx = 0; dx <= _winXRadius; dx++)
            {
                for (int dy = 0; dy <= _winYRadius; dy++)
                {
                    _weight[dx, dy] = _weightFunction(dx, dy);
                }
            }
        }

        public LucasKanade()
            :this(new Size(11, 11))
        {
        }
        public LucasKanade(Size windowSize)
        {
            _winXRadius = (windowSize.Width - 1) >> 1;
            _winYRadius = (windowSize.Height - 1) >> 1;

            _weightFunction = (dx, dy) => (float)Math.Exp(_beta * (dx * dx + dy * dy) / (_winXRadius * _winXRadius + _winYRadius * _winYRadius));

            FillWeightArray();
        }

        public Point CalculateVelocity(UnmanagedImage current, UnmanagedImage previous, IntPoint point)
        {
            //return CalculateVelocity(current, previous, point, true);
            return CalculateVelocity(current, previous, point, false);
        }
        //public Point IterativeCalculateVelocity(UnmanagedImage imageA, UnmanagedImage imageB, IntPoint point, bool weightedWindow, int maxIterations)
        //{
        //    float[,] G = new float[2, 2];
        //    float[] b;

        //    #region Calculating G
        //    for (int i = point.Y - WindowHalfHeight; i < point.Y + WindowHalfHeight; i++)
        //    {
        //        for (int j = point.X - WindowHalfWidth; j < point.X + WindowHalfWidth; j++)
        //        {
        //            Derivative3 dIij = CalculateDerivative(imageA, imageB, j, i);

        //            if (!weightedWindow)
        //            {
        //                G[0, 0] += dIij.X * dIij.X;
        //                G[1, 1] += dIij.Y * dIij.Y;
        //                G[0, 1] += dIij.X * dIij.Y;
        //                G[1, 0] = G[0, 1];
        //            }
        //            else // weightedWindow
        //            {
        //                int dx = j - point.X;
        //                int dy = i - point.Y;

        //                float w = alpha * (float)Math.Exp(beta * (dx * dx + dy * dy) / (float)(WindowHalfWidth * WindowHalfWidth + WindowHalfHeight * WindowHalfHeight));

        //                G[0, 0] += w * dIij.X * dIij.X;
        //                G[1, 1] += w * dIij.Y * dIij.Y;
        //                G[0, 1] += w * dIij.X * dIij.Y;
        //                G[1, 0] = G[0, 1];
        //            }
        //        }
        //    }
        //    #endregion

        //    Point shift = new Point();

        //    for (int iter = 0; iter < maxIterations; iter++)
        //    {
        //        #region Iteration: Calculating vector b
        //        b = new float[2];

        //        for (int i = point.Y - WindowHalfHeight; i < point.Y + WindowHalfHeight; i++)
        //        {
        //            for (int j = point.X - WindowHalfWidth; j < point.X + WindowHalfWidth; j++)
        //            {
        //                Derivative3 dIij = CalculateDerivative(imageA, imageB, j, i, (int)Math.Round(shift.X), (int)Math.Round(shift.Y));

        //                if (!weightedWindow)
        //                {
        //                    b[0] += dIij.X * dIij.t;
        //                    b[1] += dIij.Y * dIij.t;
        //                }
        //                else // weightedWindow
        //                {
        //                    int dx = j - point.X;
        //                    int dy = i - point.Y;

        //                    float w = alpha * (float)Math.Exp(beta * (dx * dx + dy * dy) / (float)(WindowHalfWidth * WindowHalfWidth + WindowHalfHeight * WindowHalfHeight));

        //                    b[0] += w * dIij.X * dIij.t;
        //                    b[1] += w * dIij.Y * dIij.t;
        //                }
        //            }
        //        }
        //        #endregion

        //        Point dP = SolveMatrixEquation(G, b);

        //        if (dP.EuclideanNorm() < 1f)
        //            return shift + dP;

        //        shift += dP;
        //    }

        //    return shift;
        //}
        public Point CalculateVelocity(UnmanagedImage imageA, UnmanagedImage imageB, IntPoint point, bool weightedWindow)
        {
            float[,] G = new float[2, 2];
            float[] b = new float[2];

            int px = point.X;
            int py = point.Y;

            for (int i = py - _winYRadius; i < py + _winYRadius; i++)
            {
                for (int j = px - _winXRadius; j < px + _winXRadius; j++)
                {
                    Derivative3 d = CalculateDerivative(imageA, imageB, j, i);

                    if (!weightedWindow)
                    {
                        G[0, 0] += d.X * d.X;
                        G[1, 1] += d.Y * d.Y;
                        G[0, 1] += d.X * d.Y;
                        G[1, 0] = G[0, 1];

                        b[0] += d.X * d.t;
                        b[1] += d.Y * d.t;
                    }
                    else // weightedWindow
                    {
                        int dxAbs = j - px; dxAbs = dxAbs < 0 ? -dxAbs : dxAbs;
                        int dyAbs = i - py; dyAbs = dyAbs < 0 ? -dyAbs : dyAbs;

                        float w = _weight[dxAbs, dyAbs];

                        G[0, 0] += w * d.X * d.X;
                        G[1, 1] += w * d.Y * d.Y;
                        G[0, 1] += w * d.X * d.Y;
                        G[1, 0] = G[0, 1];

                        b[0] += w * d.X * d.t;
                        b[1] += w * d.Y * d.t;
                    }
                }
            }

            return Solve2x2LinearMatrixEquation(G, b);
        }

        private float CalcualateIntegralElement(float[,] integralRepresentedArray, int i, int j, float ijValue)
        {
            float value = ijValue;

            if (i - 1 >= 0)
                value = value + integralRepresentedArray[i - 1, j];

            if (j - 1 >= 0)
                value = value + integralRepresentedArray[i, j - 1];

            if (i - 1 >= 0 && j - 1 >= 0)
                value = value - integralRepresentedArray[i - 1, j - 1];

            return value;
        }
        private float GetRegionSumFromIntegralRepresentation(float[,] integralRepresentation, int x, int y, int width, int height)
        {
            float result = integralRepresentation[y + height - 1, x + width - 1];

            if (x > 0)
                result -= integralRepresentation[y + height - 1, x - 1];

            if (y > 0)
                result -= integralRepresentation[y - 1, x + width - 1];

            if (x > 0 && y > 0)
                result += integralRepresentation[y - 1, x - 1];

            return result;
        }
        /// <summary>
        /// Calculates full velocity feild uses integral repsenetations to speed up
        /// </summary>
        public unsafe Point[,] CalculateVelocityField(UnmanagedImage imageA, UnmanagedImage imageB)
        {
            int width = imageA.Width;
            int height = imageA.Height;

            float[,] IxIx = new float[height, width];
            float[,] IxIy = new float[height, width];
            float[,] IyIy = new float[height, width];
            float[,] ItIx = new float[height, width];
            float[,] ItIy = new float[height, width];

            // calculate integral representation

            //Parallel.For(0, height, (i) =>
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Derivative3 curdI = CalculateDerivative(imageA, imageB, j, i);

                    IxIx[i, j] = CalcualateIntegralElement(IxIx, i, j, curdI.X * curdI.X);
                    IxIy[i, j] = CalcualateIntegralElement(IxIy, i, j, curdI.X * curdI.Y);
                    IyIy[i, j] = CalcualateIntegralElement(IyIy, i, j, curdI.Y * curdI.Y);
                    ItIx[i, j] = CalcualateIntegralElement(ItIx, i, j, curdI.t * curdI.X);
                    ItIy[i, j] = CalcualateIntegralElement(ItIy, i, j, curdI.t * curdI.Y);
                }
            }
            
            Point[,] velocities = new Point[height, width];

            int winW = 2 * _winXRadius + 1;
            int winH = 2 * _winYRadius + 1;

            Parallel.For(0, width - winW + 1, (winX) =>
            //for (int winX = 0; winX <= width - winW; winX++)
            {

                for (int winY = 0; winY <= height - winH; winY++)
                {
                    // calculate G and b into window
                    float IxIxWinValue = GetRegionSumFromIntegralRepresentation(IxIx, winX, winY, winW, winH);
                    float IxIyWinValue = GetRegionSumFromIntegralRepresentation(IxIy, winX, winY, winW, winH);
                    float IyIyWinValue = GetRegionSumFromIntegralRepresentation(IyIy, winX, winY, winW, winH);
                    float ItIxWinValue = GetRegionSumFromIntegralRepresentation(ItIx, winX, winY, winW, winH);
                    float ItIyWinValue = GetRegionSumFromIntegralRepresentation(ItIy, winX, winY, winW, winH);

                    float[,] G = new float[2, 2] { { IxIxWinValue, IxIyWinValue }, { IxIyWinValue, IyIyWinValue } };
                    float[] b = new float[2] { ItIxWinValue, ItIyWinValue };

                    velocities[winY + _winYRadius, winX + _winXRadius] = Solve2x2LinearMatrixEquation(G, b);
                }
            });

            return velocities;
        }

        private float[,] Inverse2x2(float[,] G)
        {
            float detInv = 1.0f / (G[0, 0] * G[1, 1] - G[0, 1] * G[1, 0]);

            return new [,]
                {
                    {detInv * G[1, 1], -detInv * G[0, 1]},
                    {-detInv * G[1, 0], detInv * G[0, 0]}
                };
        }
        private Point Solve2x2LinearMatrixEquation(float[,] G, float[] b)
        {
            Point V = new Point();
            float[,] GInv = Inverse2x2(G);

            V.X = GInv[0, 0] * b[0] + GInv[0, 1] * b[1];
            V.Y = GInv[1, 0] * b[0] + GInv[1, 1] * b[1];

            if (float.IsNaN(V.X))
                V.X = 0;

            if (float.IsNaN(V.Y))
                V.Y = 0;

            return V;
        }

        public enum DerivativeComponent
        {
            X,
            Y,
            t
        }
        public unsafe UnmanagedImage ShowDerivative(UnmanagedImage imageA, UnmanagedImage imageB, DerivativeComponent component)
        {
            UnmanagedImage result = UnmanagedImage.Create(imageA.Width, imageA.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            byte* ptrBase = (byte*)result.ImageData.ToPointer();
            byte* ptr = ptrBase;

            for (int i = 0; i < imageA.Height - 1; i++)
            {
                for (int j = 0; j < imageA.Width - 1; j++)
                {
                    Derivative3 dI = CalculateDerivative(imageA, imageB, j, i);
                    
                    ptr = ptrBase + (i * result.Stride + j);

                    float value = 0f;

                    switch (component)
                    {
                        case DerivativeComponent.X: value = dI.X; break;
                        case DerivativeComponent.Y: value = dI.Y; break;
                        case DerivativeComponent.t: value = dI.t; break;
                        default: break;
                    }

                    *ptr = (byte)Math.Min(255, Math.Max(0, value));
                }
            }

            return result;
        }

        private unsafe Derivative3 CalculateDerivative(UnmanagedImage imageA, UnmanagedImage imageB, int x, int y, int imgBShiftX, int imgBShiftY)
        {
            byte I_ijk = GetPixel(imageA, x, y);
            byte I_ij1k = GetPixel(imageA, x + 1, y);
            byte I_i1jk = GetPixel(imageA, x, y + 1);
            byte I_i1j1k = GetPixel(imageA, x + 1, y + 1);

            byte I_ijk1 = GetPixel(imageB, x - imgBShiftX, y - imgBShiftY);
            byte I_ij1k1 = GetPixel(imageB, x + 1 - imgBShiftX, y - imgBShiftY);
            byte I_i1jk1 = GetPixel(imageB, x - imgBShiftX, y + 1 - imgBShiftY);
            byte I_i1j1k1 = GetPixel(imageB, x + 1 - imgBShiftX, y + 1 - imgBShiftY);

            float dIdX = 0.25f * (I_ij1k + I_i1j1k + I_i1j1k1 + I_ij1k1) - 0.25f * (I_ijk + I_i1jk + I_i1jk1 + I_ijk1);
            float dIdY = 0.25f * (I_i1jk + I_i1j1k + I_i1jk1 + I_i1j1k1) - 0.25f * (I_ijk + I_ij1k + I_ijk1 + I_ij1k1);
            float dIdt = 0.25f * (I_ijk1 + I_i1jk1 + I_ij1k1 + I_i1j1k1) - 0.25f * (I_ijk + I_i1jk + I_ij1k + I_i1j1k);

            return new Derivative3(dIdX, dIdY, dIdt);
        }
        private unsafe Derivative3 CalculateDerivative(UnmanagedImage imageA, UnmanagedImage imageB, int x, int y)
        {
            return CalculateDerivative(imageA, imageB, x, y, 0, 0);
        }

        private unsafe byte GetPixel(UnmanagedImage image, int x, int y)
        {
            if (x >= image.Width || x < 0)
                return 0;
            
            if (y >= image.Height || y < 0)
                return 0;

            byte* ptr = (byte*)image.ImageData.ToPointer();

            // allign pointer to the pixel
            ptr += y * image.Stride + x;

            return *ptr;
        }
    }
}