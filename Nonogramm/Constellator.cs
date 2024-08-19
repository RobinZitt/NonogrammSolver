using System.Collections.Generic;
using System.Linq;

namespace Nonogramm.Nonogramm;

public class Constellator
{
    public List<List<bool>> VariationIterator(List<int> numberList, int length)
    {
        var completeRowList = new List<List<bool>>();
        var gapList = new List<int>();
        var gapCounter = numberList.Count + 1;
        var freeFields = numberList.Aggregate(length, (current, t) => current - t);
        freeFields -= numberList.Count - 1;
        if (freeFields == 0 || freeFields == length)
        {
            ConstellationOne(numberList, completeRowList, length);
        }
        else
        {
            for (var i = 0; i < gapCounter; i++) gapList.Add(i == 0 ? freeFields : 0);
            ConstellationLoop(gapList, numberList, 0, completeRowList, length);
        }

        return completeRowList;
    }

    private static void ConstellationLoop(List<int> inputGapList, List<int> numberList, int gapPos,
        List<List<bool>> completeRowList, int length)
    {
        var gapList = new List<int>();
        gapList.AddRange(inputGapList);
        if (gapList[0] > 0)
        {
            if (gapPos > 0) gapList[gapPos]++;
            ConstellationChecker(gapList, numberList, completeRowList, length);
            for (var i = 0; i < gapList.Count; i++)
                if (i == 0) gapList[i]--;
                else if (i >= gapPos) ConstellationLoop(gapList, numberList, i, completeRowList, length);
        }
        else
        {
            if (gapPos > 0) gapList[gapPos]++;
            ConstellationChecker(gapList, numberList, completeRowList, length);
        }
    }

    private static void ConstellationChecker(List<int> gapList, List<int> numberList, List<List<bool>> completeRowList,
        int length)
    {
        var fieldList = new List<bool>();
        for (var i = 0; i < length; i++) fieldList.Add(true);

        var variationCounter = 0;
        for (var i = 0; i < gapList.Count; i++)
        {
            for (var j = 0; j < gapList[i]; j++)
            {
                fieldList[variationCounter] = false;
                variationCounter++;
            }

            if (i >= gapList.Count - 1) continue;
            for (var j = 0; j < numberList[i]; j++)
            {
                fieldList[variationCounter] = true;
                variationCounter++;
            }

            if (i >= gapList.Count - 2) continue;
            fieldList[variationCounter] = false;
            variationCounter++;
        }

        completeRowList.Add(fieldList);
    }

    private static void ConstellationOne(List<int> numberList, List<List<bool>> completeRowList, int length)
    {
        var fieldList = new List<bool>();
        if (numberList[0] == 0)
        {
            for (var i = 0; i < length; i++) fieldList.Add(false);
            completeRowList.Add(fieldList);
            return;
        }

        for (var i = 0; i < length; i++) fieldList.Add(true);

        var variationCounter = 0;
        for (var i = 0; i < numberList.Count; i++)
        {
            for (var j = 0; j < numberList[i]; j++)
            {
                fieldList[variationCounter] = true;
                variationCounter++;
            }

            if (i < numberList.Count - 1)
            {
                fieldList[variationCounter] = false;
                variationCounter++;
            }
        }

        completeRowList.Add(fieldList);
    }
}