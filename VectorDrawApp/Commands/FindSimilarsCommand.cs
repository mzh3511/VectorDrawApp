using System.Linq;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.MatchingLib;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.Commands
{
    public class FindSimilarsCommand : VectorDrawCommand
    {
        public override string CommandName => nameof(FindSimilarsCommand);
        public override object Execute(vdControls.vdFramedControl vdFramedControl)
        {
            var document = vdFramedControl.BaseControl.ActiveDocument;
            var layout = vdFramedControl.BaseControl.ActiveDocument.ActiveLayOut;

            var gripSelection = VdProUtil.GetGripSelection(layout);
            var srcList = layout.Entities.Cast<vdFigure>().ToList();
            var entitiesOfSample = gripSelection.Cast<vdFigure>().ToList();

            var processor = new MatchProcessor(document);
            return processor.Match(srcList, entitiesOfSample);
        }
    }
}