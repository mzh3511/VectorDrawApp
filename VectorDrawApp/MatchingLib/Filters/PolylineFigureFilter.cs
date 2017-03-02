using System;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public class PolylineFigureFilter : BaseFigureFilter
    {
        public bool ConsiderVertex { get; set; } = true;

        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemFigure = item as vdPolyline;
            var sampleFigure = sampleMajor as vdPolyline;
            if (itemFigure == null || sampleFigure == null)
                return false;
            if (itemFigure.Layer != sampleFigure.Layer)
                return false;

            if (itemFigure.SPlineFlag != sampleFigure.SPlineFlag)
                return false;

            if (itemFigure.PenColor != sampleFigure.PenColor)
                return false;
            if (Math.Abs(itemFigure.PenWidth - sampleFigure.PenWidth) > 0.1d)
                return false;

            if (ConsiderVertex)
            {
                if (itemFigure.VertexList.Count != sampleFigure.VertexList.Count)
                    return false;

                if (sampleFigure.VertexList.Count >= 3)
                {
                    //如果多余两个顶点，则判断面积
                    if (Math.Abs(Math.Abs(itemFigure.Area()) - Math.Abs(sampleFigure.Area())) >= 0.001d)
                        return false;
                }
                else
                {
                    //如果只有俩顶点则判断长度
                    if (Math.Abs(itemFigure.Length() - sampleFigure.Length()) >= 0.001d)
                        return false;
                }
            }
            return true;
        }
    }
}