using System;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public class ArcFigureFilter : BaseFigureFilter
    {
        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemFigure = item as vdArc;
            var sampleFigure = sampleMajor as vdArc;
            if (itemFigure == null || sampleFigure == null)
                return false;

            var radiusRange = sampleFigure.Radius * 0.1;
            if (radiusRange > 1)
                radiusRange = 1;
            if (Math.Abs(itemFigure.Radius - sampleFigure.Radius) > radiusRange)
                return false;
            //使用圆弧面积进行比较
            if (Math.Abs(Math.Abs(itemFigure.Area()) - Math.Abs(sampleFigure.Area())) > radiusRange* radiusRange)
                return false;
            return true;
        }
    }
}