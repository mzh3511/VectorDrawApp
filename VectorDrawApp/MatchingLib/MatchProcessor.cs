using System;
using System.Collections.Generic;
using System.Linq;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Render;
using VectorDrawApp.MatchingLib.Comparers;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.MatchingLib
{
    /// <summary>
    /// 匹配处理器
    /// </summary>
    public class MatchProcessor
    {
        private readonly vdDocument _document;

        public MatchProcessor(vdDocument document)
        {
            _document = document;
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="srcFigures"></param>
        /// <param name="itemsOfSample">样本组，里面包含多个图元</param>
        /// <returns></returns>
        public MatchItem Match(List<vdFigure> srcFigures, List<vdFigure> itemsOfSample)
        {
            if (itemsOfSample == null || itemsOfSample.Count < 2)
                throw new ArgumentException(nameof(itemsOfSample));

            //选中的样品，后续就是找跟这个相似的
            var sample = new FigureSet(itemsOfSample);

            var majorFilter = FilterFactory.Get(sample.Major.GetType());
            if (majorFilter == null)
                return null;
            //从源图元集合中找出符合样本特征图元的元素
            var majorList = majorFilter.Filter(srcFigures, sample.Major);

            //样本组外包矩形
            var sampleBoundingBox = sample.GetBoundingBox();
            //样本组特征图元到中心点的位移
            var sampleOffsetOfMajor2Center = new gPoint(
                sampleBoundingBox.MidPoint.x - sample.Major.BoundingBox.MidPoint.x,
                sampleBoundingBox.MidPoint.y - sample.Major.BoundingBox.MidPoint.y);
            //样本组特征图元到中心点的位移长度
            var sampleOffsetLenOfMajor2Center = gPoint.Distance2D(sampleBoundingBox.MidPoint,
                sample.Major.BoundingBox.MidPoint);
            //sampleBoundingBox.MidPoint - sample.Major.BoundingBox.MidPoint;
            //样本外包矩形的对角线长度
            var sampleDiagonalLen = Math.Pow(Math.Pow(sampleBoundingBox.Width, 2.0) + Math.Pow(sampleBoundingBox.Height, 2.0), 0.5);

            //找到的和样本组类似的图元组集合
            var result = new MatchItem { Sample = sample,CadFileName = _document.FileName};

            //选择集合
            var selectingList = new List<vdFigure>();
            var debugLayer = VdSqlUtil.AppendLayer(_document, "DebugLayer");
            VdSqlUtil.DeleteFiguresByLayer(_document.ActionLayout, debugLayer);
            foreach (var major in majorList)
            {
                //根据特征图元的中心点、样本组外包矩形的对角线长度，样本组特征图元到中心点的位移
                //计算该次框选的范围
                var boundingBox = new Box();
                boundingBox.AddPoint(major.BoundingBox.MidPoint);
                boundingBox.AddWidth((sampleDiagonalLen + sampleOffsetLenOfMajor2Center) / 2d + 1);
                //boundingBox.AddWidth(sampleMajorMaxLenOfBox);
                //boundingBox.AddWidth(1);
                //boundingBox.Offset(sampleOffsetOfMajor2Center.x, sampleOffsetOfMajor2Center.y, 0);

                //框选
                var selection = _document.Selections.Add("BoundingByMajor");
                selection.RemoveAll();
                selection.Select(RenderSelect.SelectingMode.WindowRectangle, new gPoints(new[] { boundingBox.UpperLeft, boundingBox.LowerRight }));

                //selectingList.Add(major);
                //selectingList.Add(AppendRect(_document, boundingBox, debugLayer));
                //continue;

                var fromFigures = selection.OfType<vdFigure>().ToList();
                fromFigures.Sort(new FigureMidPointComparer());
                //从框选结果中筛选图元
                var item = MatchCore(fromFigures, major, sample);
                if (item != null && GetXorItem(result, item) == null)
                {
                    result.Results.Add(item);
                    //selectingList.AddRange(item.Entities);
                }
                else
                {
                    //VdUtil.SelectFigures(_document, fromFigures);
                    //VdUtil.RefreshVectorDraw(_document);
                    //break;
                }
            }
            if (selectingList.Count > 0)
            {
                VdActionUtil.SelectFigures(_document, selectingList);
                VdActionUtil.RefreshVectorDraw(_document);
            }

            return result;
        }

        private FigureSet GetXorItem(MatchItem matchItem, FigureSet test)
        {
            var testBox = test.GetBoundingBox();
            var testDiagonalLen2 = Math.Pow(Math.Pow(testBox.Width, 2.0) + Math.Pow(testBox.Height, 2.0), 0.5) * 2;
            if (matchItem.Sample.GetBoundingBox().MidPoint.Distance2D(testBox.MidPoint) < testDiagonalLen2)
            {
                if (matchItem.Sample.Entities.Any(cond => test.Entities.Contains(cond)))
                    return matchItem.Sample;
            }

            foreach (var item in matchItem.Results)
            {
                var itemBox = item.GetBoundingBox();
                var distance = itemBox.MidPoint.Distance2D(testBox.MidPoint);
                if (distance < testDiagonalLen2)
                {
                    if (item.Entities.Any(cond => test.Entities.Contains(cond)))
                        return item;
                }
            }
            return null;
        }

        public FigureSet MatchCore(List<vdFigure> srcFigures, vdFigure srcMajor, FigureSet sample)
        {
            if (srcFigures.Count < sample.Entities.Count)
                return null;
            //后面对该集合进行改动
            srcFigures = new List<vdFigure>(srcFigures);

            var resultItemList = new List<vdFigure>();
            foreach (var sampleItem in sample.Entities)
            {
                if (sampleItem == sample.Major)
                {
                    resultItemList.Add(srcMajor);
                    continue;
                }
                var srcFigure = FilterFigure(srcFigures, srcMajor, sampleItem, sample.Major);
                if (srcFigure != null)
                {
                    resultItemList.Add(srcFigure);
                    srcFigures.Remove(srcFigure);

                }
                else
                {
                    return null;
                }
            }

            if (resultItemList.Count == sample.Entities.Count)
                return new FigureSet(resultItemList, srcMajor);
            return null;
        }

        private vdFigure FilterFigure(List<vdFigure> srcFigures, vdFigure srcMajor, vdFigure sampleFigure, vdFigure sampleMajor)
        {
            var sampleFigureType = sampleFigure.GetType();
            var sampleFigureOffsetLenSquared = sampleFigure.BoundingBox.MidPoint.DistanceSquared(sampleMajor.BoundingBox.MidPoint);
            for (var i = 0; i < srcFigures.Count; i++)
            {
                var srcfigure = srcFigures[i];
                if (srcfigure == srcMajor)
                    continue;
                if (srcfigure.GetType() != sampleFigureType)
                    continue;

                var offset = srcfigure.BoundingBox.MidPoint - srcMajor.BoundingBox.MidPoint;
                if (Math.Abs(offset.x * offset.x + offset.y * offset.y - sampleFigureOffsetLenSquared) >= 2)
                    continue;

                var filter = FilterFactory.Get(sampleFigureType);
                if (filter != null)
                {
                    if (filter.IsMatchable(srcfigure, sampleFigure))
                        return srcfigure;
                }
            }
            return null;
        }

        private vdRect AppendRect(vdDocument document, Box boundingBox, vdLayer layer = null)
        {
            var rect = new vdRect
            {
                InsertionPoint = new gPoint(boundingBox.Left, boundingBox.Bottom),
                Width = boundingBox.Width,
                Height = boundingBox.Height,
                Layer = layer ?? document.ActiveLayer,
                LineType = document.LineTypes.DPIDash,
                PenColor = new vdColor(System.Drawing.Color.Green)
            };
            document.ActiveLayOut.Entities.AddItem(rect);
            return rect;
        }
    }
}