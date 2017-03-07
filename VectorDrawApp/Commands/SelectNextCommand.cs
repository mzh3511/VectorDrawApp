using System.Collections.Generic;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.MatchingLib;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp.Commands
{
    public class SelectNextCommand : VectorDrawCommand
    {
        private int _moveNothingCount = 0;
        private int _setIndex = -1;
        private List<List<vdFigure>> _setList;
        public override string CommandName => nameof(SelectNextCommand);

        public List<List<vdFigure>> SetList
        {
            get { return _setList; }
            set
            {
                _setList = value;
                _setIndex = -1;
            }
        }

        public override object Execute(vdControls.vdFramedControl vdFramedControl)
        {
            if (_setList == null)
                return null;

            if (_moveNothingCount >= 1)
            {
                _setIndex = -1;
            }

            ++_setIndex;
            if (_setIndex <= _setList.Count - 1)
            {
                _moveNothingCount = 0;
                var entities = _setList[_setIndex];
                VdActionUtil.SelectFigures(vdFramedControl.BaseControl.ActiveDocument, entities);
                VdActionUtil.LocateFigures(vdFramedControl.BaseControl.ActiveDocument, entities);
                VdActionUtil.RefreshVectorDraw(vdFramedControl.BaseControl.ActiveDocument);
            }
            else
            {
                ++_moveNothingCount;
            }
            return null;
        }
    }
}