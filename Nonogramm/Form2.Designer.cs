using System.ComponentModel;

namespace Nonogramm.Nonogramm
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.columnNumber = new System.Windows.Forms.NumericUpDown();
            this.rowNumber = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.columnNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 86);
            this.button1.TabIndex = 1;
            this.button1.Text = "Continue\r\n";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(175, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Number of rows\r\n";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(175, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 40);
            this.label2.TabIndex = 5;
            this.label2.Text = "Number of columns";
            // 
            // columnNumber
            // 
            this.columnNumber.Location = new System.Drawing.Point(378, 240);
            this.columnNumber.Name = "columnNumber";
            this.columnNumber.Size = new System.Drawing.Size(97, 31);
            this.columnNumber.TabIndex = 7;
            this.columnNumber.ValueChanged += new System.EventHandler(this.columnNumber_ValueChanged);
            // 
            // rowNumber
            // 
            this.rowNumber.Location = new System.Drawing.Point(378, 185);
            this.rowNumber.Name = "rowNumber";
            this.rowNumber.Size = new System.Drawing.Size(97, 31);
            this.rowNumber.TabIndex = 8;
            this.rowNumber.ValueChanged += new System.EventHandler(this.rowNumber_ValueChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rowNumber);
            this.Controls.Add(this.columnNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.columnNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowNumber)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.NumericUpDown rowNumber;
        private System.Windows.Forms.NumericUpDown columnNumber;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button button1;

        #endregion
    }
}