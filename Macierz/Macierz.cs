using System;

namespace MacierzLib
{
    public class Macierz
    {
        private int m; //the number of rows
        private int n; //the number of columns
        private float[,] values;
        public bool IsZero
        {
            get
            {
                if (m == 0 || n == 0)
                    return true;
                else return false;
            }
        }
        #region Constructors
        //Constructor
        public Macierz(float[,] _values)
        {
            m = _values.GetLength(0);
            n = _values.GetLength(1);
            values = _values;
        }
        public Macierz(int _m, int _n)
        {
            values = new float[_m, _n];
            m = _m;
            n = _n;
        }
        #endregion
        #region Basic Matrix Returns
        public static string ValueAsString(double value, int decimalPlaces)
        {
            return value.ToString($"F{decimalPlaces}");
        }
        public override string ToString()
        {
            string vals = string.Empty;
            int decimalplace = 0;
            foreach (var v in values)
            {               
                var str = v.ToString();

                if (str.Contains(","))
                {
                    string[] strs = str.Split(',');

                    if (strs[1].Length > decimalplace)
                    {
                        decimalplace = strs[1].Length;
                    }
                }
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    vals += ( ValueAsString(values[i, j], decimalplace) +  '\t');
                }
                if (i < m - 1) vals += "\n";
            }
            return vals;
        }
        public int GetLength(int index)
        {
            return values.GetLength(index);
        }
        public float Sum()
        {
            float sum = 0;

            foreach (float f in this)
            {
                sum += f;
            }
            return sum;
        }

        #endregion
        #region Overridden operators
        //Indexing
        public float this[int _m, int _n]
        {
            get
            {
                return values[_m, _n];
            }
            set
            {
                values[_m, _n] = value;
            }
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }
        //Adding
        public static Macierz operator +(Macierz a, Macierz b)
        {
            try
            {
                canIAddorSubtract(a, b);
                Macierz newValues = new Macierz(a.m, a.n);
                for (int i = 0; i < a.m; i++)
                {
                    for (int j = 0; j < a.n; j++)
                    {
                        newValues[i, j] = a.values[i, j] + b.values[i, j];
                    }
                }
                return newValues;
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                return new Macierz(0, 0);
            }
        }
        //Subtracting
        public static Macierz operator -(Macierz a, Macierz b)
        {
            try
            {
                canIAddorSubtract(a, b);
                Macierz newValues = new Macierz(a.m, a.n);
                for (int i = 0; i < a.m; i++)
                {
                    for (int j = 0; j < a.n; j++)
                    {
                        newValues[i, j] = a.values[i, j] - b.values[i, j];
                    }
                }
                return newValues;
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                return new Macierz(0, 0);
            }
        }
        //Multication
        public static Macierz operator *(Macierz a, Macierz b)
        {
            try
            {
                canIMultiply(a, b);
                Macierz newValues = new Macierz(a.n, b.m);

                for (int i = 0; i < a.n; i++)
                {
                    for (int j = 0; j < newValues.GetLength(0); j++)
                    {
                        float w = 0;
                        for (int k = 0; k < newValues.GetLength(1); k++)
                            w += a[i, k] * b[k, j];
                        newValues[i, j] = w;
                    }

                }
                return newValues;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                return new Macierz(0, 0);
            }
        }
        #endregion            
        #region Booleans
        private static void canIAddorSubtract(Macierz a, Macierz b)
        {
            if (a.m != b.m || a.n != b.n)
            {
                throw new InvalidOperationException("It is not possible to subtract those matrices, make sure you input two matrices of the same dimensions.");
            }
        }
        private static void canIMultiply(Macierz a, Macierz b)
        {
            if (!(a.n == b.m))
            {
                throw new InvalidOperationException("It is not possible to multipy those matrices, make sure you input correct matrices.");
            }

        }
        #endregion
    }
}
