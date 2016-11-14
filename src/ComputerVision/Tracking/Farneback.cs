using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using AForge.Imaging;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace ComputerVision.Tracking
{
    public class Farneback : IOpticalFlowEstimator
    {
        struct PolyCoefficients
        {
            // p(x,y) = r1 + r2*x + r3 * y + r4 * x^2 + r5 * y^2 + r6 * x * y
            // or
            // p(x,y) = Xt*A*X + bt*X + c
            // where 
            // X = | x |
            //     | y |
            //
            // A = | r4    r6/2 |
            //     | r6/2  r5   |
            //
            // b = | r2 |
            //     | r3 |
            //
            // c = r1

            public PolyCoefficients(Vector<float> R)
            {
                //r1 = R[0];
                //r2 = R[1];
                //r3 = R[2];
                //r4 = R[3];
                //r5 = R[4];
                //r6 = R[5];

                A = new DenseMatrix(new[,]
                {
                    { R[3], R[5] / 2.0f },
                    { R[5] / 2.0f, R[4]},
                });

                B = new DenseVector(new[]
                {
                     R[1],
                     R[2]
                });
            }

            //public float r1;
            //public float r2;
            //public float r3;
            //public float r4;
            //public float r5;
            //public float r6;

            public Matrix<float> A;
            public Vector<float> B;
        }

        private Matrix<float> CalculateBr(int n)
        {
            int neighborhoodAreaSideSize = 2 * n + 1;
            int neighborhoodPixelAmount = neighborhoodAreaSideSize * neighborhoodAreaSideSize;

            Matrix<float> BMat = new DenseMatrix(neighborhoodPixelAmount, 6);

            for (int y = -n; y <= n; y++)
            {
                for (int x = -n; x <= n; x++)
                {
                    // numerating in B matrix (from left to right, from up to down)
                    int line = (y + n) * neighborhoodAreaSideSize + (x + n);
                    //          ^^^^^                                ^^^^^
                    // absolute line number                            |
                    //                                         absolute column

                    BMat[line, 0] = 1;
                    BMat[line, 1] = x;
                    BMat[line, 2] = y;
                    BMat[line, 3] = x * x;
                    BMat[line, 4] = y * y;
                    BMat[line, 5] = x * y;
                }
            }

            Matrix<float> BtMat = BMat.Transpose();
            Matrix<float> BrMat = BtMat.Multiply(BMat).Inverse().Multiply(BtMat);

            return BrMat;
        }

        /// <summary>
        /// Calculating polynom's coefficients for each pixel
        /// </summary>
        /// <param name="img">Source image</param>
        /// <param name="n">Neighborhood area radius</param>
        /// <returns>Polynom's coefficients field</returns>
        private PolyCoefficients PolynomialExpansion(UnmanagedImage img, int n, IntPoint point, Matrix<float> BrMat)
        {
            // problem: argmin (B * R - f)^2  (minimizing by finding R at which (B * R - f)^2 = min)
            // solution: R = (Bt * B)^-1 * Bt * f = Br * f 
            // where: R - unknown coefficeints of the basis functions vector
            // basis: (1, x, y, x^2, y^2, xy)
            //         0  1  2  3    4    5
            // f - signal value vector
            // B - basis func values matrix in neighborhood area

            // point numeration schemee (from left to right, from up to down)
            //   1  2  3
            //   4  5  6
            //   7  8  9

            int neighborhoodAreaSideSize = 2 * n + 1;
            int neighborhoodPixelAmount = neighborhoodAreaSideSize * neighborhoodAreaSideSize;

            //float[,] B = new float[neighborhoodPixelAmount, 6];

            //for (int y = -n; y <= n; y++)
            //{
            //    for (int x = -n; x <= n; x++)
            //    {
            //        // numerating in B matrix (from left to right, from up to down)
            //        int line = (y + n) * neighborhoodAreaSideSize + (x + n);
            //        //          ^^^^^                                ^^^^^
            //        // absolute line number                            |
            //        //                                         absolute column

            //        B[line, 0] = 1;
            //        B[line, 1] = x;
            //        B[line, 2] = y;
            //        B[line, 3] = x * x;
            //        B[line, 4] = y * y;
            //        B[line, 5] = x * y;
            //    }
            //}

            //Matrix<float> BMat = new DenseMatrix(B);
            //Matrix<float> BtMat = BMat.Transpose();
            //Matrix<float> BrMat = BtMat.Multiply(BMat).Inverse().Multiply(BtMat);

            // Br * f = R

            // creating f for pixel (x,y)
            Vector<float> fVec = new DenseVector(neighborhoodPixelAmount);

            for (int yy = point.Y - n; yy <= point.Y + n; yy++)
            {
                for (int xx = point.X - n; xx <= point.X + n; xx++)
                {
                    int line = (yy + n - point.Y) * neighborhoodAreaSideSize + (xx + n - point.X);

                    fVec[line] = GetPixel(img, xx, yy);
                }
            }

            Vector<float> R = BrMat.Multiply(fVec);

            return new PolyCoefficients(R);
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

        public Point CalculateVelocity(UnmanagedImage current, UnmanagedImage previous, IntPoint point)
        {
            float maxNorm = 7.0f;

            //Vector<float> velocity = CalculateVelocityAreaLS(current, previous, point, 4, 3, 1.1f);
            Vector<float> velocity = CalculateVelocityPointwise(current, previous, point, 8);

            if (float.IsNaN(velocity[0]) || float.IsNaN(velocity[1]))
                velocity[0] = velocity[1] = 0f;

            float norm = (float)Math.Sqrt(velocity[0] * velocity[0] + velocity[1] * velocity[1]);
            if (norm > maxNorm)
            {
                velocity[0] /= norm * (1 / maxNorm);
                velocity[1] /= norm * (1 / maxNorm);
            }

            return new Point(velocity[0], velocity[1]);
        }

        public Vector<float> CalculateVelocityPointwise(UnmanagedImage current, UnmanagedImage previous, IntPoint point, int n)
        {
            Matrix<float> Br = CalculateBr(n);

            PolyCoefficients pc1Val = PolynomialExpansion(current, n, point, Br);
            PolyCoefficients pc2Val = PolynomialExpansion(previous, n, point, Br);

            Matrix<float> A = pc1Val.A + pc2Val.A;
            Vector<float> deltab = pc2Val.B - pc1Val.B;

            return A.Inverse() * deltab;
        }
        
        public Vector<float> CalculateVelocityAreaLS(UnmanagedImage current, UnmanagedImage previous, IntPoint point, int n, int winSize, float sigma)
        {
            Matrix<float> Br = CalculateBr(n);
            // I : [x - winSize; x + winSize] x [y - wS; y + wS]

            Func<float, float> _weight = (d) => (float)(Math.Exp(-d * d / (2 * sigma * sigma)));

            Matrix<float> wAtASum = new DenseMatrix(2, 2);
            Vector<float> wAtdeltabSum = new DenseVector(2);

            for (int dx = - winSize; dx <= winSize; dx++)
            {
                for (int dy = -winSize; dy <= winSize; dy++)
                {
                    IntPoint p = new IntPoint(point.X + dx, point.Y + dy);
                    float w = _weight(dx) * _weight(dy);

                    PolyCoefficients pc1Val = PolynomialExpansion(current, n, p, Br);
                    PolyCoefficients pc2Val = PolynomialExpansion(previous, n, p, Br);

                    Matrix<float> A = pc1Val.A + pc2Val.A;//).Multiply(0.5f);
                    Matrix<float> At = A.Transpose();
                    Vector<float> deltab = pc2Val.B - pc1Val.B;//).Multiply(0.5f);

                    wAtASum.Add(A * At * w, wAtASum);
                    wAtdeltabSum.Add(At * deltab * w, wAtdeltabSum);
                }
            }

            return wAtASum.Inverse() * wAtdeltabSum;
        }
    }
}
