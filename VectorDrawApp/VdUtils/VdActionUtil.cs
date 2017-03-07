using System.Collections.Generic;
using System.Linq;
using VectorDraw.Actions;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.VdUtils
{
    public class VdActionUtil
    {
        public static vdSelection GetGripSelection(vdLayout layout)
        {
            var gripsetname = "VDGRIPSET_" + layout.Handle.ToStringValue();
            if (layout.ActiveViewPort != null)
                gripsetname = gripsetname + layout.ActiveViewPort.Handle.ToStringValue();
            var gripset = layout.Document.Selections.FindName(gripsetname);
            return gripset;
        }

        public static void SelectFigures(vdDocument document, IList<vdFigure> entities)
        {
            var entities1 = entities.Distinct().ToList();
            if (entities1.Count < entities.Count)
            {
                var aa = from r in entities
                         group r by r.HandleId into g
                         where g.Count() > 1
                         select g;
                var bb = aa.Select(cond => cond.Key).ToList();
                var cc = aa.Select(cond => cond).ToList();

                entities = entities1;
            }
            var gripSelectioin = GetGripSelection(document.ActionLayout);
            gripSelectioin.RemoveAll();
            if (entities.Count >= 10)
            {
                //前面图元列表不触发事件，只在添加最后一个图元时触发事件
                var freezeEvents = gripSelectioin.FreezeEvents;
                gripSelectioin.FreezeEvents = true;

                var list1 = new List<vdFigure>(entities);
                list1.RemoveAt(list1.Count - 1);
                gripSelectioin.AddRange(new vdEntities(list1.ToArray()), vdSelection.AddItemCheck.Nochecking);
                gripSelectioin.FreezeEvents = freezeEvents;

                var list2 = new List<vdFigure>(entities);
                list2.RemoveRange(0, list2.Count - 1);
                gripSelectioin.AddRange(new vdEntities(list2.ToArray()), vdSelection.AddItemCheck.Nochecking);
            }
            else
            {
                gripSelectioin.AddRange(new vdEntities(entities.ToArray()), vdSelection.AddItemCheck.Nochecking);
            }

            gripSelectioin.ShowGrips(true);
        }

        public static void LocateFigures(vdDocument document, IList<vdFigure> entities)
        {
            var entities1 = entities.Distinct().ToList();
            if (entities1.Count < entities.Count)
            {
                entities = entities1;
            }
            var boundingBox = GetBoundingBox(entities);
            document.ZoomWindow(boundingBox.UpperLeft, boundingBox.LowerRight);
            document.ZoomScale(50);
        }

        public static Box GetBoundingBox(IList<vdFigure> entities)
        {
            var vdEntities = new vdEntities(entities.ToArray());
            return vdEntities.GetBoundingBox(true, true);
        }

        public static bool TryGetUserRect(vdDocument document,out Box box)
        {
            box = null;
            var pt = document.ActionUtility.getUserPoint() as gPoint;
            if (pt == null)
                return false;
            return document.ActionUtility.getUserRectViewCS(pt, out box) == StatusCode.Success;
        }

        public static void RefreshVectorDraw(vdDocument document)
        {
            document.Update();
            document.Redraw(true);
        }
    }
}