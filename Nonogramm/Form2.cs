using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nonogramm.Nonogramm;

public partial class Form2 : Form
{
    private Button _solveButton;
    private TextBox[,] columnInputFields;
    private int columns;
    private TextBox[,] rowInputFields;
    private int rows;
    private TableLayoutPanel tableLayoutPanel1;
    private Label[,] nonoFields;


    public Form2()
    {
        InitializeComponent();
        Name = "Nonogramm";
        Text = "Nonogramm";
    }
    


    private void Form2_Load(object sender, EventArgs e)
    {
        //throw new System.NotImplementedException();
    }


    private void button1_Click(object sender, EventArgs e)
    {
        if (rows <= 0 || columns <= 0)
        {
            return;
        }
        SuspendLayout();
        var inputRows = InputFieldCalculator(columns);
        var inputColumns = InputFieldCalculator(rows);
        var totalRows = rows + inputColumns;
        var totalColumns = columns + inputRows;
        rowInputFields = new TextBox[rows, inputRows];
        columnInputFields = new TextBox[columns, inputColumns];
        nonoFields = new Label[rows, columns];
        const int boxSize = 30;
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.TabIndex = 0;
        tableLayoutPanel1.BackColor = Color.White;
        tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
        tableLayoutPanel1.RowCount = totalRows;
        tableLayoutPanel1.ColumnCount = totalColumns;
        for (var i = 0; i < totalRows; i++) tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, boxSize));
        for (var i = 0; i < totalColumns; i++) tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, boxSize));
        for (var i = 0; i < totalRows; i++)
        {
            for (var j = 0; j < totalColumns; j++)
            {
                if (i < inputColumns && j < inputRows) continue;

                if (i < inputColumns && j >= inputRows)
                {
                    columnInputFields[j - inputRows, i] = new TextBox();
                    tableLayoutPanel1.Controls.Add(columnInputFields[j - inputRows, i], j, i);
                }

                if (i >= inputColumns && j < inputRows)
                {
                    rowInputFields[i - inputColumns, j] = new TextBox();
                    tableLayoutPanel1.Controls.Add(rowInputFields[i - inputColumns, j], j, i);
                }

                if (i >= inputColumns && j >= inputRows)
                {
                    nonoFields[i - inputColumns, j - inputRows] = new Label();
                    nonoFields[i - inputColumns, j - inputRows].BorderStyle = BorderStyle.None;
                    tableLayoutPanel1.Controls.Add(nonoFields[i - inputColumns, j - inputRows], j, i);
                }
            }  
        }
        

        var tableHeight = (boxSize + 3) * totalRows + 3;
        var tableWidth = (boxSize + 3) * totalColumns + 3;
        const int solveButtonHeight = 86;
        const int solveButtonWidth = 184;
        _solveButton = new Button();
        _solveButton.Size = new Size(184, 86);
        var rectangle = new Control();
        rectangle.Size = new Size( (boxSize + 3)* inputRows - 1, (boxSize + 3)* inputColumns - 1);
        rectangle.BackColor = Color.White;
        rectangle.Location = new Point(1, 1);
        Controls.Add(rectangle);
        _solveButton.Text = "Solve!";
        _solveButton.Location = new Point(tableWidth / 2 - solveButtonWidth / 2, tableHeight);
        _solveButton.Click += SolveClick;
        Controls.Remove(button1);
        Controls.Remove(rowNumber);
        Controls.Remove(columnNumber);
        Controls.Remove(label1);
        Controls.Remove(label2);
        Controls.Add(_solveButton);
        tableLayoutPanel1.Size = new Size(tableWidth, tableHeight);
        Size = new Size(tableWidth + boxSize, tableHeight + 2 * boxSize + solveButtonHeight);
        Controls.Add(tableLayoutPanel1);
        ResumeLayout();
    }

    private static int InputFieldCalculator(int rowLength)
    {
        return (rowLength + 1) / 2;
    }

    private static List<int> EmptyListGen()
    {
        var emptyList = new List<int>();
        return emptyList;
    }

    private bool ValueAdder(IList<List<int>> rowValues, bool isRow)
    {
        var inputFields = isRow ? rowInputFields : columnInputFields;
        for (var i = 0; i < inputFields.GetLength(0); i++)
        {
            rowValues.Add(EmptyListGen());
            for (var j = 0; j < inputFields.GetLength(1); j++)
                try
                {
                    if (inputFields[i, j].Text == "") continue;
                    var rowValue = int.Parse(inputFields[i, j].Text);
                    if (rowValue > 0) rowValues[i].Add(rowValue);
                }
                catch (FormatException exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }

            if (!rowValues[i].Any()) rowValues[i].Add(0);
        }
        //checks if any values are too large
        var length = isRow ? columns : rows;
        for (var i = 0; i < rowValues.Count; i++)
        {
            var requiredFields = 0;
            for (var j = 0; j < rowValues[i].Count; j++)
            {
                requiredFields += rowValues[i][j];
                if (j > 0) requiredFields++;
            }

            if (requiredFields <= length) continue;
            var rowColumn = isRow ? "row " : "column ";
            Console.WriteLine("Sum of numbers of " + rowColumn + (i + 1) + " is too large!");
            return true;
        }

        return false;
    }
    
    

    private void SolveClick(object sender, EventArgs e)
    {
        Controls.Remove(tableLayoutPanel1);
        var rowValues = new List<List<int>>();
        var columnValues = new List<List<int>>();
        //Copies the row/ column values in a list that is usable by the nonogramm class
        var rowResult = ValueAdder(rowValues, true);
        var columnResult = ValueAdder(columnValues, false);
        if (rowResult || columnResult) 
        {
            return; //Returns if the input values are too large
        }
        var nonogramm = new Nonogramm(rows, columns, rowValues, columnValues);
        nonogramm.Solver();
        for (int i = 0; i < nonoFields.GetLength(0); i++)
        {
            for (int j = 0; j < nonoFields.GetLength(1); j++)
            {
                if (nonogramm.GetResult(i,j) == NonoField.Box)
                {
                    nonoFields[i, j].BackColor = Color.Black;
                }

                if (nonogramm.GetResult(i,j) == NonoField.Cross)
                {
                    nonoFields[i, j].BackColor = Color.White;
                }
            }
        }
        tableLayoutPanel1.CellPaint += PaintCell;
        Controls.Add(tableLayoutPanel1);
    }

    private void PaintCell(object sender, TableLayoutCellPaintEventArgs e)
    {
        var inputRows = InputFieldCalculator(columns);
        var inputColumns = InputFieldCalculator(rows);
        if (e.Row >= inputColumns && e.Column >= inputRows)
        {
            var currentRow = e.Row - inputColumns;
            var currentColumn = e.Column - inputRows;
            e.Graphics.FillRectangle(
                nonoFields[currentRow, currentColumn].BackColor == Color.Black ? Brushes.Black : Brushes.White,
                e.CellBounds);
        }
        
    }

    private void rowNumber_ValueChanged(object sender, EventArgs e)
    {
        rows = (int)rowNumber.Value;
    }

    private void columnNumber_ValueChanged(object sender, EventArgs e)
    {
        columns = (int)columnNumber.Value;
    }

}