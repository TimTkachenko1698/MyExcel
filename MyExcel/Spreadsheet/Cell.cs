using MyExcel;
using Parser;

namespace MyExel.Spreadsheet
{
    public class Cell
    {
        public string Value;
        public int X;
        public int Y;
        public Formula formula;
        public delegate void Edited();
        public event Edited? Calculate;

        public void UnSubscribe()
        {
            Calculate = null;
        }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            formula = new Formula("");
        }



        public void EditCell(string data)
        {
            if (formula.RawData == data) return;
            formula.RawData = data;
            Value = "";
        }

        //Function: Evaluate
        public void Evaluate()
        {
            if (CheckRecursion())
            {
                UnSubscribe();
                throw new Exception("Recursion");
            }
            var GVCell = SpreadSheet.GridView.Rows[Y].Cells[X];
            if (formula.RawData == "")
            {
                GVCell.ErrorText = "";
                return;
            }
            Parser.Parser.Parse(this);
            Value = formula.GetResult().ToString();
            Calculate?.Invoke();
            Calculate = null;
            GVCell.ErrorText = "";
        }

        public static bool CheckRecursion()
        {
            System.Diagnostics.StackTrace myTrace = new System.Diagnostics.StackTrace();
            if (myTrace.FrameCount < 100)
                return false;
            System.IntPtr mh = myTrace.GetFrame(1).GetMethod().MethodHandle.Value;
            for (int iCount = 2; iCount < myTrace.FrameCount; iCount++)
            {
                System.Reflection.MethodBase m = myTrace.GetFrame(iCount).GetMethod();
                if (m.MethodHandle.Value == mh)
                {
                    return true;
                }
            }
            return false;
        }

    }
}