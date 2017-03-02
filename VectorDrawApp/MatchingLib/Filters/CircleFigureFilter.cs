using System;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    internal class CircleFigureFilter : BaseFigureFilter
    {
        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemFigure = item as vdCircle;
            var sampleFigure = sampleMajor as vdCircle;
            if (itemFigure == null || sampleFigure == null)
                return false;

            if (itemFigure.Layer != sampleFigure.Layer)
                return false;
            if (itemFigure.PenColor != sampleFigure.PenColor)
                return false;
            if (Math.Abs(itemFigure.PenWidth - sampleFigure.PenWidth) > 0.1d)
                return false;
            if (Math.Abs(itemFigure.Radius - sampleFigure.Radius) > 0.1d)
                return false;
            return true;
        }
    }
}