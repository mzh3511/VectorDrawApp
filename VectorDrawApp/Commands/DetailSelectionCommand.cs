using System.Text;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.MatchingLib;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.Commands
{
    public class DetailSelectionCommand : VectorDrawCommand
    {
        public override string CommandName => nameof(LocateGridSelectionCommand);
        public override object Execute(vdControls.vdFramedControl vdFramedControl)
        {
            var document = vdFramedControl.BaseControl.ActiveDocument;
            var selection = VdProUtil.GetGripSelection(document.ActionLayout);
            if (selection.Count == 0)
                return string.Empty;
            var sb = new StringBuilder();
            foreach (vdFigure figure in selection)
            {
                sb.AppendLine($"{figure.GetType().Name}, Handle={figure.HandleId}");
            }
            return sb;
        }
    }
}