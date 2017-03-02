using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VectorDrawApp.MatchingLib
{
    public class TextFigureFilter : BaseFigureFilter
    {
        protected override bool FilterItem(vdFigure item, vdFigure sampleMajor)
        {
            var itemString = string.Empty;
            var sampleString = string.Empty;
            var sampleVdText = sampleMajor as vdText;
            var sampleVdMText = sampleMajor as vdMText;
            if (sampleVdText == null && sampleVdMText == null)
                return false;
            if (sampleVdText != null)
            {
                var itemFigure = item as vdText;
                if (itemFigure != null)
                    itemString = itemFigure.TextString;
                else
                    return false;
                sampleString = sampleVdText.TextString;
            }
            if (sampleVdMText != null)
            {
                var itemFigure = item as vdMText;
                if (itemFigure != null)
                    itemString = itemFigure.TextString;
                else
                    return false;
                sampleString = sampleVdMText.TextString;
            }
            if (string.IsNullOrWhiteSpace(itemString) || string.IsNullOrWhiteSpace(sampleString))
                return false;
            var count = 0;
            var itemEnumerator = itemString.GetEnumerator();
            var sampleEnumerator = sampleString.GetEnumerator();
            while (itemEnumerator.MoveNext() && sampleEnumerator.MoveNext())
            {
                if (itemEnumerator.Current == sampleEnumerator.Current)
                    ++count;
                else
                    break;
            }
            return count >= 4 || count >= 2 && sampleString.Length == count;
        }
    }
}