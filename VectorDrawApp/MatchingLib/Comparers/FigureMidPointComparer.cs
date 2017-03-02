using System;
using System.Collections.Generic;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib.Comparers
{
    class FigureMidPointComparer : IComparer<vdFigure>
    {
        public int Compare(vdFigure x, vdFigure y)
        {
            var midPtX = x.BoundingBox.MidPoint;
            var midPtY = y.BoundingBox.MidPoint;
            if (Math.Abs(midPtX.x - midPtY.x) > 0.001)
                return midPtX.x.CompareTo(midPtY.x);
            return midPtX.y.CompareTo(midPtY.y);
        }
    }
}