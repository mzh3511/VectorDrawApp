using System;
using System.Collections.Generic;
using VectorDraw.Professional.vdFigures;

namespace VectorDrawApp.MatchingLib
{
    public class FilterFactory
    {
        private static readonly Dictionary<Type, BaseFigureFilter> _dictionary;

        static FilterFactory()
        {
            _dictionary = new Dictionary<Type, BaseFigureFilter>()
            {
                {typeof (vdText), new TextFigureFilter()},
                {typeof (vdMText), new TextFigureFilter()},
                {typeof (vdPolyline), new PolylineFigureFilter()},
                {typeof (vdArc), new ArcFigureFilter()},
                {typeof (vdPolyhatch), new PolyhatchFigureFilter()},
                {typeof (vdLine), new LineFigureFilter()},
                {typeof (vdCircle), new CircleFigureFilter()}
            };
        }

        public static BaseFigureFilter Get(Type sampleMajorType)
        {
            return _dictionary.ContainsKey(sampleMajorType) ? _dictionary[sampleMajorType] : null;
        }
    }
}