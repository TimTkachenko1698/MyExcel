using MyExel.Spreadsheet;

namespace Parser
{
    public abstract class Token
    {
        public string RawData { get; set; }

        private int valueLenth => RawData.Length;

        public int Priority;
        public Cell thisCell;
        public Token(string value)
        {
            RawData = value.ToLower();
        }

        public abstract dynamic ToValue();
    }

    public class IntValueToken : Token
    {
        public IntValueToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Int64.Parse(RawData);
        }
    }

    public class BoolValueToken : Token
    {
        public BoolValueToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Boolean.Parse(RawData); ;
        }
    }

    public class SumbolToken : Token
    {
        public SumbolToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Char.Parse(RawData);
        }
    }

    public class PointerToken : Token
    {

        public PointerToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            int columnIndex = 0;
            string row = "";
            for (int i = 0; i < RawData.Length; i++)
            {
                var c = RawData[i];
                if ((int)c >= 97 && (int)c <= 122)
                {
                    columnIndex += ((int)c - 96) * (int)Math.Pow(26, i);
                }
                else
                {
                    row += c;
                }
            }
            var cell = SpreadSheet.cells[columnIndex - 1, Int32.Parse(row) - 1];
            cell.Calculate += thisCell.Evaluate;
            if (CheckRecursion())
            {
                cell.UnSubscribe();
                //throw new Exception("Recursion");
            }
            var v = cell.Value;
            if(v == null) return null;
            if (v == "True" || v == "False" || v == "true" || v == "false") return Boolean.Parse(v);
            else return Int64.Parse(v);
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

    public class OperationToken : Token
    {

        public dynamic TokenOpertion;

        public OperationToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return TokenOpertion;
        }
    }



    public enum TokenType
    {
        Symbol, // 22
        Constant,// 21
        Pointer, // 20
        Operator,// 0 -  19
        Invalid
    }
}