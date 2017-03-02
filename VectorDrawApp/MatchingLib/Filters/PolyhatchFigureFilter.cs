using System;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public class PolyhatchFigureFilter : BaseFigureFilter
    {
        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemFigure = item as vdPolyhatch;
            var sampleFigure = sampleMajor as vdPolyhatch;
            if (itemFigure == null || sampleFigure == null)
                return false;

            if (itemFigure.HatchProperties.FillMode != sampleFigure.HatchProperties.FillMode)
                return false;
            if (itemFigure.PolyCurves.Count != sampleFigure.PolyCurves.Count)
                return false;

            var sampleMidPt = sampleFigure.BoundingBox.MidPoint;
            var itemMidPt = itemFigure.BoundingBox.MidPoint;

            for (var i = 0; i < sampleFigure.PolyCurves.Count; i++)
            {
                var itemCurves = itemFigure.PolyCurves[i];
                var sampleCurves = sampleFigure.PolyCurves[i];
                if (itemCurves.Count != sampleCurves.Count)
                    return false;

                for (var j = 0; j < sampleCurves.Count; j++)
                {
                    var itemCurve = itemCurves[j];
                    var sampleCurve = sampleCurves[j];
                    if (itemCurve.GetType() != sampleCurve.GetType())
                        return false;
                    var itemDistance = gPoint.Distance2D(itemCurve.BoundingBox.MidPoint, itemMidPt);
                    var sampleDistance = gPoint.Distance2D(sampleCurve.BoundingBox.MidPoint, sampleMidPt);
                    if (Math.Abs(itemDistance - sampleDistance) > 1d)
                        return false;
                }
            }
            return true;
        }
    }
}