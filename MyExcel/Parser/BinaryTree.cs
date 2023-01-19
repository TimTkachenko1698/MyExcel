using Operations;

namespace Parser
{
    public class BinaryTree
    {
        public Token token;
        public BinaryTree[] Next;

        public BinaryTree(Token value, BinaryTree first, BinaryTree second = null)
        {
            this.token = value;
            if (first == null && second == null)
            {
                Next = null;
                return;
            }
            Next = new BinaryTree[2] { first, second };
        }

        public dynamic GetResult()
        {
            if (token == null) throw new Exception(message: "One of the token is invalid");
            if (token is OperationToken)
            {
                var tempToken = (token as OperationToken).ToValue();
                if (tempToken is IBinaryOperation)
                {
                    var t = tempToken as IBinaryOperation;
                    return t.Evaluate(Next[0].GetResult(), Next[1].GetResult());
                }
                else if (tempToken is IUnaryOperation)
                {
                    var t = tempToken as IUnaryOperation;
                    return t.Evaluate(Next[1].GetResult());
                }
                else
                {
                    var t = tempToken as IBinaryLogicOperation;
                    return t.Evaluate(Next[0].GetResult(), Next[1].GetResult());
                }
            }
            return token.ToValue();
            
        }
    }
}