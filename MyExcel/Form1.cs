using MyExel.Spreadsheet;
using System.Diagnostics;
using Parser;

namespace MyExcel
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellValueChanged);
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(SpreadSheet.CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(SpreadSheet.CellEndEdit);
        }



        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Size textSize = TextRenderer.MeasureText(rowIdx, this.Font);
            if (grid.RowHeadersWidth < textSize.Width + 40)
            {
                grid.RowHeadersWidth = textSize.Width + 40;
            }
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void AddRow(object sender, EventArgs e)
        {
            SpreadSheet.AddRow();

        }

        private void DeleteRow(object sender, EventArgs e)
        {
            SpreadSheet.DeleteRow();
        }

        private void AddColumn(object sender, EventArgs e)
        {
            SpreadSheet.AddColumn();
        }

        private void DeleteColumn(object sender, EventArgs e)
        {
            SpreadSheet.DeleteColumn();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new();
            about.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Show();
            //Process.Start(new ProcessStartInfo { FileName = @"https://github.com/LironeA/MyExcel", UseShellExecute = true });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (MessageBox.Show("Зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (SpreadSheet.filePath == "") saveAsToolStripMenuItem_Click(null, null);
                    else saveToolStripMenuItem1_Click(null, null);
                }
                System.Windows.Forms.Application.Exit();
            }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (SpreadSheet.filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
            Application.Restart();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SpreadSheet.SaveDialog();
                SpreadSheet.SaveFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (SpreadSheet.filePath == "") SpreadSheet.SaveDialog();
                SpreadSheet.SaveFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        


        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (SpreadSheet.filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
            SpreadSheet.OpenFile();
        }

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Хочете зберегти файл?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (SpreadSheet.filePath == "") saveAsToolStripMenuItem_Click(null, null);
                else saveToolStripMenuItem1_Click(null, null);
            }
        }
    }
}
