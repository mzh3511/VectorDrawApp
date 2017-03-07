using System.Linq;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.MatchingLib;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.Commands
{
    public class LocateGridSelectionCommand : VectorDrawCommand
    {
        public override string CommandName => nameof(LocateGridSelectionCommand);
        public override object Execute(vdControls.vdFramedControl vdFramedControl)
        {
            var document = vdFramedControl.BaseControl.ActiveDocument;
            var vdGrid = vdFramedControl.vdGrid;
            var selectedObjArr = vdGrid.SelectedObject as object[];
            if (selectedObjArr == null)
                return null;
            VdActionUtil.LocateFigures(document, selectedObjArr.OfType<vdFigure>().ToList());
            VdActionUtil.RefreshVectorDraw(document);
            return null;
        }
    }
}