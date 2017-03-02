using System;
using System.Collections.Generic;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public abstract class BaseFigureFilter
    {
        public Func<vdFigure, vdFigure, bool> PreFilterFunc { get; set; }
        /// <summary>
        /// 在<see cref="srcFigures"/>中过滤出和<see cref="sampleFigure"/>匹配的所有图元
        /// </summary>
        /// <param name="srcFigures"></param>
        /// <param name="sampleFigure"></param>
        /// <returns></returns>
        public List<vdFigure> Filter(List<vdFigure> srcFigures, vdFigure sampleFigure)
        {
            if (srcFigures == null || srcFigures.Count == 0)
                throw new ArgumentNullException(nameof(srcFigures));
            if (sampleFigure == null)
                throw new ArgumentNullException(nameof(sampleFigure));

            var passedSet = new List<vdFigure>();
            var sampleType = sampleFigure.GetType();
            foreach (vdFigure srcFigure in srcFigures)
            {
                if (srcFigure.GetType() != sampleType)
                    continue;
                if (srcFigure == sampleFigure)
                    continue;
                if (PreFilterFunc != null && !PreFilterFunc.Invoke(srcFigure, sampleFigure))
                    continue;
                if (FilterItem(srcFigure, sampleFigure))
                    passedSet.Add(srcFigure);
            }
            return passedSet;
        }

        /// <summary>
        /// 判断两个图元是否匹配
        /// </summary>
        /// <param name="figure1"></param>
        /// <param name="figure2"></param>
        /// <returns></returns>
        public bool IsMatchable(vdFigure figure1, vdFigure figure2)
        {
            if (figure1.GetType() != figure2.GetType())
                return false;
            if (PreFilterFunc != null && !PreFilterFunc.Invoke(figure1, figure2))
                return false;
            return FilterItem(figure1, figure2);
        }

        protected abstract bool FilterItem(vdFigure item, vdFigure sampleMajor);
    }
}