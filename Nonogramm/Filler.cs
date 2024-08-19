using System.Collections.Generic;

namespace Nonogramm.Nonogramm;

public class Filler
{
    public static void Fill(int rows, int columns, List<List<bool>> rowList, int rowNumber, bool isRow)
    {
        var completeBoxList = new List<bool>();
        var completeCrossList = new List<bool>();
        var size = isRow ? columns : rows;
        for (var i = 0; i < size; i++)
        {
            completeBoxList.Add(true);
            completeCrossList.Add(true);
        }

        foreach (var t in rowList)
            for (var i = 0; i < t.Count; i++)
            {
                completeBoxList[i] = completeBoxList[i] && t[i];
                completeCrossList[i] = completeCrossList[i] && !t[i];
            }

        for (var i = 0; i < size; i++)
        {
            if (Nonogramm.GetRowValue(rowNumber, i, isRow) != NonoField.Empty) continue;
            if (!completeBoxList[i] && !completeCrossList[i]) continue;
            if (completeBoxList[i]) Nonogramm.SetRowValue(rowNumber, i, NonoField.Box, isRow);
            if (completeCrossList[i]) Nonogramm.SetRowValue(rowNumber, i, NonoField.Cross, isRow);
            Nonogramm.FieldCounterInc();
            if (isRow)
            {
                Nonogramm.rowFilled[rowNumber] = true;
                Nonogramm.columnFiltered[i] = false;
            }
            else
            {
                Nonogramm.columnFilled[rowNumber] = true;
                Nonogramm.rowFiltered[i] = false;
            }
        }
    }
}