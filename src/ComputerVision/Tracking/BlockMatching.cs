using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Point = AForge.Point;
using AForge.Imaging;
using AForge;

namespace ComputerVision.Tracking
{
    public class BlockMatching : IOpticalFlowEstimator
    {
        private int _blockWidth;
        private int _blockHeight;

        private int _maxMoveXRadius;
        private int _maxMoveYRadius;

        private int _shiftX;
        private int _shiftY;

        public BlockMatching()
            : this(
                new Size(5, 5), // block size
                new Size(1, 1), // step
                new Size(6, 6)) // max move radius
        {
        }
        
        public BlockMatching(Size blockSize, Size shiftSize, Size maxMoveSize)
        {
            _blockWidth = blockSize.Width;
            _blockHeight = blockSize.Height;

            _shiftX = shiftSize.Width;
            _shiftY = shiftSize.Height;

            _maxMoveXRadius = maxMoveSize.Width;
            _maxMoveYRadius = maxMoveSize.Height;
        }

        public Point CalculateVelocity(UnmanagedImage current, UnmanagedImage previous, IntPoint point)
        {
            //return CalculateVelocity(current, previous, point, Metric.SumOfSquaresDifferences);
            float nc;
            return CalculateVelocityUsingNCMetic(current, previous, point, out nc);
        }

        public Point CalculateVelocityUsingNCMetic(UnmanagedImage current, UnmanagedImage previous, IntPoint point, out float normalizedCorrelation)
        {
            ComparisonMetric NCMetric = ComparisonMetric.NormalizedCorrelation;
            Size areaSize = new Size(_blockWidth, _blockHeight);
            // centering block
            IntPoint areaLocationPrev = point - new IntPoint(_blockWidth >> 1, _blockHeight >> 1);

            IntPoint optimalDelta = new IntPoint(0, 0);
            float maxCorrelation = float.MinValue;

            for (int dx = -_maxMoveXRadius; dx <= _maxMoveXRadius; dx += _shiftX)
            {
                for (int dy = -_maxMoveYRadius; dy <= _maxMoveYRadius; dy += _shiftY)
                {
                    IntPoint delta = new IntPoint(dx, dy);
                    IntPoint areaLocationCur = areaLocationPrev + delta;

                    // checking position
                    if (areaLocationCur.X < 0 || areaLocationCur.Y < 0
                        || areaLocationCur.X + areaSize.Width > current.Width
                        || areaLocationCur.Y + areaSize.Height > current.Height)
                        continue;

                    float nolmalizedCorrelation = NCMetric.Calculate(current, previous, areaSize, areaLocationCur, areaLocationPrev);

                    if (nolmalizedCorrelation > maxCorrelation)
                    {
                        maxCorrelation = nolmalizedCorrelation;
                        optimalDelta = delta;
                    }

                }
            }

            normalizedCorrelation = maxCorrelation;

            return optimalDelta;
        }

        public Point CalculateVelocityUsingSSDMetic(UnmanagedImage current, UnmanagedImage previous, IntPoint point)
        {
            ComparisonMetric SSDMetric = ComparisonMetric.SumOfSquaresDifferences;
            Size areaSize = new Size(_blockWidth, _blockHeight);
            // centering block
            IntPoint areaLocationPrev = point - new IntPoint(_blockWidth >> 1, _blockHeight >> 1);
            
            IntPoint optimalDelta = new IntPoint(0, 0);

            float minSSD = float.MaxValue;
            float maxSSD = float.MinValue;

            for (int dx = -_maxMoveXRadius; dx <= _maxMoveXRadius; dx += _shiftX)
            {
                for (int dy = -_maxMoveYRadius; dy <= _maxMoveYRadius; dy += _shiftY)
                {
                    IntPoint delta = new IntPoint(dx, dy);
                    IntPoint areaLocationCur = areaLocationPrev + delta;

                    // checking position
                    if (areaLocationCur.X < 0 || areaLocationCur.Y < 0
                        || areaLocationCur.X + areaSize.Width > current.Width 
                        || areaLocationCur.Y + areaSize.Height > current.Height)
                        continue;

                    float SSD = SSDMetric.Calculate(current, previous, areaSize, areaLocationCur, areaLocationPrev);

                    if (SSD > maxSSD)
                        maxSSD = SSD;
                    
                    if (SSD < minSSD)
                        //|| metricValue == optimalMetric && delta.EuclideanNorm() < optimalDelta.EuclideanNorm())
                    {
                        minSSD = SSD;
                        optimalDelta = delta;
                    }
                }
            }

            // improves the stability
            float reliability = maxSSD / (areaSize.Width * areaSize.Height * 255 * 255);

            if (reliability < 0.001f)
                return new IntPoint();

            return optimalDelta;
        }
    }
}
