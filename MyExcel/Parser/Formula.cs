namespace Parser
{
    public class Formula
    {
        public string RawData { get; set; }
        public Token[] Tokens { get; set; }
        public BinaryTree BinaryTree { get; set; }
        public dynamic Result { get; set; }
        public Formula(string rawData)
        {
            this.RawData = rawData ?? throw new ArgumentNullException(nameof(rawData));
        }

        public dynamic GetResult()
        {
            Result = BinaryTree?.GetResult();
            return Result;
        }

    }
}