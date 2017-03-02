using System;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public class LineFigureFilter : BaseFigureFilter
    {
        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemFigure = item as vdLine;
            var sampleFigure = sampleMajor as vdLine;
            if (itemFigure == null || sampleFigure == null)
                return false;

            if (itemFigure.Layer != sampleFigure.Layer)
                return false;
            if (itemFigure.PenColor != sampleFigure.PenColor)
                return false;
            if (Math.Abs(itemFigure.PenWidth - sampleFigure.PenWidth) > 0.1d)
                return false;
            var sampleLen = sampleFigure.Length();
            if (Math.Abs(itemFigure.Length() - sampleLen) > sampleLen * 0.1d)
                return false;
            return true;
        }
    }
}