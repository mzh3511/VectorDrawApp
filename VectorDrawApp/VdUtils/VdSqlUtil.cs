using System.Linq;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.VdUtils
{
    /// <summary>
    /// VD对象的增删改查
    /// </summary>
    public static class VdSqlUtil
    {
        public static vdLayer AppendLayer(vdDocument document, string layerName)
        {
            vdLayer layer;
            if ((layer = document.Layers.FindName(layerName)) == null)
            {
                layer = document.Layers.Add(layerName);
            }
            return layer;
        }

        public static int DeleteFiguresByLayer(vdLayout layout, vdLayer layer)
        {
            var result = 0;
            for (var i = layout.Entities.Count - 1; i >= 0; i--)
            {
                if (layout.Entities[i].Layer == layer)
                {
                    layout.Entities.RemoveAt(i);
                    ++result;
                }
            }
            return result;
        }

        public static vdFigure GetFigureByHandle(vdDocument document, ulong handleId)
        {
            return document.FindFromHandle(new vdHandle(handleId), typeof(vdFigure)) as vdFigure;
        }
    }
}