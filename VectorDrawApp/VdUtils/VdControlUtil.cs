
namespace VectorDrawApp.VdUtils
{
    public class VdControlUtil
    {
        public static void SmartVisibleLayout(vdControls.vdFramedControl vd, vdControls.vdFramedControl.LayoutStyle layoutStyle)
        {
            var visible = vd.GetLayoutStyle(layoutStyle);
            vd.SetLayoutStyle(layoutStyle, !visible);
            if (visible)
                return;
            if (layoutStyle == vdControls.vdFramedControl.LayoutStyle.PropertyGrid)
            {
                var selection = VdProUtil.GetGripSelection(vd.BaseControl.ActiveDocument.ActionLayout);
                if (selection.Count > 0)
                    vd.vdGrid.SelectedObject = selection;
                else
                    vd.vdGrid.SelectedObject = vd.BaseControl.ActiveDocument;
            }
        }
    }
}