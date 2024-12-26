using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace lab9_2
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;

   
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            buttonSearch = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 30;
            listBox1.Location = new Point(21, 24);
            listBox1.Margin = new Padding(5, 6, 5, 6);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(704, 814);
            listBox1.TabIndex = 1;
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(758, 418);
            buttonSearch.Margin = new Padding(5, 6, 5, 6);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(200, 50);
            buttonSearch.TabIndex = 2;
            buttonSearch.Text = "Поиск погоды";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 900);
            Controls.Add(buttonSearch);
            Controls.Add(listBox1);
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            Text = "Прогноз погоды";
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox1;
        private ListBox listBox1;
        private Button buttonSearch;
    }
}
