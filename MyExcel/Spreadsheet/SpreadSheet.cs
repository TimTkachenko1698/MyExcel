using MyExcel;

namespace MyExel.Spreadsheet
{
    static class SpreadSheet
    {
        public static DataGridView ?GridView { get; set; }
        public static Cell[,] cells;
        public static int ColumnCount;
        public static int RowCount;

        public static string filePath = "";

        public static void CreateSpreadSheet()
        {
            GridView = Program.mainForm.dataGridView1;
            cells = new Cell[1, 1];
            cells[0, 0] = new Cell(0, 0);
            ColumnCount = 0;
            RowCount = 0;
            AddColumn();
            AddRow();

        }

        private static void ExpandSpreadsheet()
        {
            Cell[,] temp_cells = new Cell[ColumnCount, RowCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    try
                    {
                        temp_cells[i, j] = cells[i, j];
                    }
                    catch
                    {
                        temp_cells[i, j] = new Cell(i, j);
                    }
                }
            }
            cells = temp_cells;
            UpdateGridView();
        }

        private static void NarrowSpreadsheet()
        {
            Cell[,] temp_cells = new Cell[ColumnCount, RowCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    temp_cells[i, j] = cells[i, j];
                    
                         
                }
            }
            cells = temp_cells;
            
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    try
                    {
                        cells[i, j].Evaluate();
                        UpdateGridView();
                    }
                    catch (Exception ex)
                    {
                        GridView.Rows[j].Cells[i].Value = ex.Message;
                        GridView.Rows[j].Cells[i].ErrorText = ex.Message;
                    }
                }
            }
        }



        public static void AddRow()
        {
            GridView.Rows.Add();
            RowCount++;
            ExpandSpreadsheet();
        }

        private static bool DeleteDialog()
        {
            if (MessageBox.Show("Ви впевнені?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                return true;
            }
            return false;
        }
        public static void DeleteRow()
        {
            if (RowCount == 1)
            {
                MessageBox.Show("Must be at least 1 row", "ERROR");
                return;
            }
            if (DeleteDialog())
            {
                GridView.Rows.RemoveAt(RowCount - 1);
                RowCount--;
                NarrowSpreadsheet();
            }
        }

        public static void AddColumn()
        {
            ColumnCount++;
            string newColumnName = findColumName(ColumnCount);
            GridView.Columns.Add(newColumnName, newColumnName);
            ExpandSpreadsheet();
        }

        public static void DeleteColumn()
        {
            if (ColumnCount == 1)
            {
                MessageBox.Show("Must be at least 1 row", "ERROR");
                return;
            }
            if (DeleteDialog())
            {
                GridView.Columns.RemoveAt(ColumnCount - 1);
                ColumnCount--;
                NarrowSpreadsheet();
            }
        }

        private static string findColumName(int index)
        {
            string result = "";
            int i = 0;
            if (index % 26 == 0) i++;
            for (; i < (int)index / 26; i++)
            {
                result += "A";
            }

            return result + (index % 26 == 0 ? "Z" : (char)(index % 26 + 64));

        }

        public static void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var tempCell = cells[e.ColumnIndex, e.RowIndex];
            if (tempCell.formula == null) return;
            var GVCell = GridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            GVCell.Value = tempCell.formula.RawData;
        }

        public static void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cell = cells[e.ColumnIndex, e.RowIndex];
            var GVCell = GridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string data;
            if(GVCell.Value == null)
            {
                data = "";
            }
            else
            {
                data = GVCell.Value.ToString();
            }
            try
            {
                cell.EditCell(data);
                cell.Evaluate();
                SpreadSheet.UpdateGridView();
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                GVCell.Value = "Cant resolve operation for diferent types";
                GVCell.ErrorText = "Cant resolve operation for diferent types";
                MessageBox.Show("Cant do operation on diferent types(bool + bool, bool + 1, ets )");
            }
            catch (Exception ex)
            {
                GVCell.Value = ex.Message;
                GVCell.ErrorText = ex.Message;
            }

        }

        public static void UpdateGridView()
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    if (GridView.Rows[j].Cells[i].ErrorText == "") 
                        GridView.Rows[j].Cells[i].Value = cells[i,j].Value;
                    
                }
            }
        }

        public static void SaveFile()
        {
            TextWriter writer = new StreamWriter(filePath);
            writer.WriteLine(SpreadSheet.cells.GetLength(0).ToString());
            writer.WriteLine(SpreadSheet.cells.GetLength(1).ToString());
            for (int i = 0; i < SpreadSheet.cells.GetLength(0); i++)
            {
                for (int j = 0; j < SpreadSheet.cells.GetLength(1); j++)
                {
                    var data = SpreadSheet.cells[i, j].formula;
                    if (data == null)
                    {
                        writer.WriteLine("");
                    }
                    else
                    {
                        writer.WriteLine(data.RawData);
                    }
                }
            }
            writer.Close();
        }

        public static void OpenFile()
        {
            GridView.Rows.Clear();
            GridView.Columns.Clear();
            OpenDialog();
            if (SpreadSheet.filePath == "") return;
            TextReader textReader = new StreamReader(SpreadSheet.filePath);
            ColumnCount = Int32.Parse(textReader.ReadLine());
            RowCount = Int32.Parse(textReader.ReadLine());
            cells = new Cell[ColumnCount,RowCount];

            for (int i = 1; i < ColumnCount + 1; i++)
            {
                string newColumnName = findColumName(i);
                GridView.Columns.Add(newColumnName, newColumnName);
            }

            for (int i = 0; i < RowCount; i++)
            {
                GridView.Rows.Add();
            }

            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    cells[i, j] = new Cell(i,j);
                    var t = textReader.ReadLine();
                    if (t == "") continue;
                    cells[i, j].formula = new Parser.Formula(t);
                    cells[i, j].Evaluate();
                }
            }
            

            
            UpdateGridView();
        }

        public static void OpenDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "TXT|*.txt";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SpreadSheet.filePath = openFileDialog.FileName;
                }
                else
                {
                    return;
                }
                if (openFileDialog.FileName == "") throw new Exception("Incorect Name");
            }
        }

        public static void SaveDialog()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "TXT|*.txt";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SpreadSheet.filePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
                if (saveFileDialog.FileName == "") throw new Exception("Incorect Name");
            }
        }


        public static void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
