namespace Operations
{
    public interface IOperation
    {
    }

    public interface IUnaryOperation : IOperation
    {
        T Evaluate<T>(T expresion);
    }

    public interface IBinaryOperation : IOperation
    {
        TR Evaluate<TR, TL>(TR left, TL right);
    }

    public interface IBinaryLogicOperation : IOperation
    {
        bool Evaluate<TR, TL>(TR left, TL right);
    }

    public interface IMultyOperation : IOperation
    {
        T Evaluate<T>(params T[] list);
    }

    public class Exponentiate : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            long temp = 1;
            for (dynamic i = 0; i < r; i++)
            {
                if ((temp * l) / l != temp) throw new OverflowException();
                temp *= l;
            }
            return (dynamic)temp;
        }
    }

    public class Multiply : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            if ((r * l) / l != r) throw new OverflowException();
            return l * r;
        }
    }

    public class Divide : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            if (r == 0) throw new DivideByZeroException();
            return l / r;
        }

    }

    public class Mod : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l % r;
        }
    }

    public class Add : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l + r;
        }
    }

    public class Substruct : IBinaryOperation
    {
        public TR Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l - r;
        }
    }

    public class Increment : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            return e = e + 1;
        }
    }

    public class Decrement : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            return e = e - 1;
        }
    }

    public class Not : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            if (typeof(T) == typeof(bool))
            {
                return !e;
            }

            return -e;
        }
    }

    public class SmallerThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l < r;
        }
    }

    public class GraterThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l > r;
        }
    }

    public class SmallerEqualThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l <= r;
        }
    }

    public class GraterEqualThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l >= r;
        }
    }

    public class Equal : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l == r;
        }
    }

    public class NotEqual : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l != r;
        }
    }

    public class And : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            if (typeof(TR) != typeof(bool) || typeof(TR) != typeof(bool)) throw new Exception("Cant apply logical operations on ints");
            dynamic l = left;
            dynamic r = right;
            return l && r;
        }
    }

    public class Or : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            if (typeof(TR) != typeof(bool) || typeof(TR) != typeof(bool)) throw new Exception("Cant apply logical operations on ints");
            dynamic l = left;
            dynamic r = right;
            return l || r;
        }
    }
}
/*
 * ^
 * !, іnc, dec
 * *, /
 * %;
 * +, - 
 * <, > <=, >=, <> , ==
 * and
 * or
 * max(x,y), mіn(x,y)
 */
