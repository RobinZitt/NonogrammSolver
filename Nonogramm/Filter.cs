using System.Collections.Generic;

namespace Nonogramm.Nonogramm;

public class Filter
{
    public static void Filt(List<List<bool>> rowList, int rowNumber, bool isRow)
    {
        for (var i = 0; i < rowList.Count; i++)
        {
            var t = rowList[i];
            for (var j = 0; j < t.Count; j++)
            {
                var isCross = Nonogramm.GetRowValue(rowNumber, j, isRow) == NonoField.Cross &&
                              t[j];
                var isBox = Nonogramm.GetRowValue(rowNumber, j, isRow) == NonoField.Box && !t[j];
                if (isCross || isBox)
                {
                    rowList.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}