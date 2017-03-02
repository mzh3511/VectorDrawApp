using System;
using System.Collections.Generic;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib.Comparers
{
    class MajorFigureComparer : IComparer<vdFigure>
    {
        private readonly List<Type> _majorTypes = new List<Type> { typeof(vdPolyhatch), typeof(vdCircle), typeof(vdArc), typeof(vdPolyline), typeof(vdText), typeof(vdMText), typeof(vdLine) };

        public int Compare(vdFigure x, vdFigure y)
        {
            var typeX = x.GetType();
            var typeY = y.GetType();
            //if (typeX == typeY)
            //    return 0;
            var indexX = _majorTypes.IndexOf(typeX);
            var indexY = _majorTypes.IndexOf(typeY);
            if (indexX >= 0 && indexY >= 0)
            {
                if (indexX == indexY)
                {
                    if (typeX == typeof(vdPolyline))
                        return Comparer<int>.Default.Compare((y as vdPolyline).VertexList.Count, (x as vdPolyline).VertexList.Count);
                    if (typeX == typeof(vdLine))
                        return Comparer<double>.Default.Compare((y as vdLine).Length(), (x as vdLine).Length());
                    if(typeX == typeof(vdArc))
                        return Comparer<double>.Default.Compare((y as vdArc).Length(), (x as vdArc).Length());
                }
                return Comparer<int>.Default.Compare(indexX, indexY);
            }
            if (indexX >= 0 && indexY < 0)
                return -1;
            if (indexX < 0 && indexY >= 0)
                return 1;
            return 0;
        }
    }
}