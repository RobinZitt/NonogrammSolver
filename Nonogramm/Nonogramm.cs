using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Nonogramm.Nonogramm;

internal class Nonogramm
{
    /** List for row constellations*/
    private static List<List<List<bool>>> rowConstellations;

    /** List for column constellations*/
    private static List<List<List<bool>>> columnConstellations;

    /** List with input values for all rows*/
    private static List<List<int>> rowValues;

    /** List with input values for all columns*/
    private static List<List<int>> columnValues;

    /** The nonogramm*/
    private static NonoField[,]? _nono;

    private static ReaderWriterLockSlim _lock = new();

    private static readonly ReaderWriterLockSlim _fieldLock = new();

    /* Input for debugging*/
    //Scanner input = new Scanner(System.in);
    /* Lock for accessing the nonogramm fields*/
    //static ReentrantReadWriteLock lock = new ReentrantReadWriteLock();
    /* Write lock for accessing the nonogramm fields*/
    //static Lock writeLock = lock.writeLock();
    /* Read lock for accessing the nonogramm fields*/
    //static Lock readLock = lock.readLock(); 
    /** False if a row should be filled*/
    public static bool[] rowFilled;

    /** False if a column should be filled*/
    public static bool[] columnFilled;

    /** False if a row should be filtered*/
    public static bool[] rowFiltered;

    /** False if a column should be filtered*/
    public static bool[] columnFiltered;

    /** Number of filled out fields*/
    public static int fieldCounter;

    /** Loop counter for debugging*/
    private int loopCounter;


    public Nonogramm(int rows, int columns, List<List<int>> rowInputValues, List<List<int>> columnInputValues)
    {
        Rows = rows;
        Columns = columns;
        rowConstellations = new List<List<List<bool>>>();
        columnConstellations = new List<List<List<bool>>>();
        rowValues = rowInputValues;
        columnValues = columnInputValues;
        _nono = new NonoField[rows, columns];
        rowFilled = new bool[rows];
        columnFilled = new bool[columns];
        rowFiltered = new bool[rows];
        columnFiltered = new bool[columns];
        _lock = new ReaderWriterLockSlim();
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
            _nono[i, j] = NonoField.Empty;

        for (var i = 0; i < rows; i++)
        {
            rowFilled[i] = false;
            rowFiltered[i] = true;
        }

        for (var i = 0; i < columns; i++)
        {
            columnFilled[i] = false;
            columnFiltered[i] = true;
        }

        fieldCounter = 0;
        loopCounter = 0;
    }

    /** Number of rows*/
    public static int Rows { get; private set; }

    /** Number of columns*/
    public static int Columns { get; private set; }

    public void Solver()
    {
        var fieldNumber = Columns * Rows;
        var startTime = DateTime.Now;
        var rowFillers = new Filler[Rows];
        var columnFillers = new Filler[Columns];
        var rowFilters = new Filter[Rows];
        var columnFilters = new Filter[Columns];
        var rowConstellators = new Constellator[Rows];
        var columnConstellators = new Constellator[Columns];
        for (var i = 0; i < Rows; i++)
        {
            rowConstellators[i] = new Constellator();
            rowConstellations.Add(rowConstellators[i].VariationIterator(rowValues[i], Columns));
        }
        for (var i = 0; i < Columns; i++)
        {
            columnConstellators[i] = new Constellator();
            columnConstellations.Add(columnConstellators[i].VariationIterator(columnValues[i], Rows));
        }
        var oldCount = -1;
        while (fieldCounter < fieldNumber)
        {
            if (fieldCounter <= oldCount)
            {
                break;
            }

            oldCount = fieldCounter;

            for (var i = 0; i < Rows; i++)
            {
                rowFillers[i] = new Filler();
                if (rowFilled[i]) continue;
                Filler.Fill(Rows, Columns, rowConstellations[i], i, true);
                loopCounter++;
            }
            for (var i = 0; i < Columns; i++)
            {
                columnFilters[i] = new Filter();
                if (columnFiltered[i]) continue;
                Filter.Filt(columnConstellations[i], i, false);
                columnFiltered[i] = true;
                columnFilled[i] = false;
                loopCounter++;
            }

            for (var i = 0; i < Columns; i++)
            {
                columnFillers[i] = new Filler();
                if (columnFilled[i]) continue;
                Filler.Fill(Rows, Columns, columnConstellations[i], i, false);
                loopCounter++;
            }

            for (var i = 0; i < Rows; i++)
            {
                rowFilters[i] = new Filter();
                if (rowFiltered[i]) continue;
                Filter.Filt(rowConstellations[i], i, true);
                rowFiltered[i] = true;
                rowFilled[i] = false;
                loopCounter++;
            }
        }
        var endTime = DateTime.Now;
        if (!Checker())
        {
            Console.WriteLine("Unsolvable!");
        }
        Console.WriteLine("Time needed: " + (endTime - startTime) + " ms");
    }

    private static NonoField GetField(int row, int column)
    {
        try
        {
            //_lock.EnterReadLock();
            return _nono[row, column];
        }
        finally
        {
            //_lock.ExitReadLock();
        }
    }

    private static void SetField(int row, int column, NonoField input)
    {
        //_lock.EnterWriteLock();
        try
        {
            _nono[row, column] = input;
        }
        finally
        {
            //_lock.ExitReadLock();
        }
    }

    private bool Checker()
    {
        var result = true;
        for (var i = 0; i < Rows; i++) result = result && RowChecker(rowValues[i], i, true);

        for (var i = 0; i < Columns; i++) result = result && RowChecker(columnValues[i], i, false);

        return result;
    }

    private static bool RowChecker(List<int> inputValues, int rowNumber, bool isRow)
    {
        var inputValueCounter = 0;
        var currentValue = 0;
        var rowLength = isRow ? _nono.GetLength(1) : _nono.GetLength(0);
        for (var i = 0; i < rowLength; i++)
        {
            if (GetRowValue(rowNumber, i, isRow) == NonoField.Cross)
            {
                if (currentValue > 0) return false;
                continue;
            }

            if (GetRowValue(rowNumber, i, isRow) == NonoField.Box)
            {
                currentValue++;
                if (inputValueCounter >= inputValues.Count) return false;

                if (currentValue == inputValues[inputValueCounter])
                {
                    currentValue = 0;
                    inputValueCounter++;
                }
            }
            else
            {
                return false;
            }
        }

        if (inputValues[0] == 0 && currentValue == 0) return true;

        return inputValueCounter == inputValues.Count;
    }

    public static NonoField GetRowValue(int staticInput, int iterator, bool isRow)
    {
        return isRow ? GetField(staticInput, iterator) : GetField(iterator, staticInput);
    }

    public static void SetRowValue(int staticInput, int iterator, NonoField field, bool isRow)
    {
        if (isRow)
            SetField(staticInput, iterator, field);
        else
            SetField(iterator, staticInput, field);
    }

    public static void FieldCounterInc()
    {
        try
        {
            _fieldLock.EnterWriteLock();
            fieldCounter++;
        }
        finally
        {
            _fieldLock.ExitWriteLock();
        }
    }

    public NonoField GetResult(int row, int column)
    {
        return _nono[row, column];
    }
}