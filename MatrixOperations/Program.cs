using System;
using System.IO;
using MacierzLib;
namespace MatrixOperations
{
    class Program
    {
        private static void sth()
        {
        }
        private static void ExitMessage()
        {
            Console.WriteLine("Press any key to return to main menu or Q to quit");
            var s = Console.Read();
            if (s == 'q' && s == 'Q')
            {
                Environment.Exit(0);
            }
        }
        private static Macierz ReadMatrix(string dir, string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(dir + '\\' + filename);
                sr.ReadLine();

                return new Macierz(1, 2);
            }
            catch
            {
                return null;
            }
        }

        private static void WriteFile(string dir, string filename, Macierz m)
        {
            try
            {
                StreamWriter sw = new StreamWriter(dir + "\\" + filename + ".txt");
                sw.WriteLine(m.GetLength(1) + " " + m.GetLength(0));
                sw.Write(m.ToString());
                sw.Close();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }
        private static Macierz CreateMatrix()
        {
            Console.WriteLine("You are creating an m-by-n matrix.");
            Console.Write("Enter m: ");
            var choice_m = Console.ReadLine();
            //  choice_m = Console.ReadLine();
            int m = 0;
            if (int.TryParse(choice_m, out m) && m > 0)
            {
                Console.Write("Enter n: ");
                var choice_n = Console.ReadLine();
                int n = 0;
                if (int.TryParse(choice_n, out n) && n > 0)
                {
                    Macierz matrix = new Macierz(m, n);

                    Console.WriteLine("\n Type values (from left to right and top to bottom). Use \",\" as decimal point.");
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            float f = 0;
                            var val = Console.ReadLine();
                            if (float.TryParse(val, out f))
                            {
                                matrix[i, j] = f;
                            }
                            else
                            {
                                Console.Write("Ops, wrong value \n");
                                return null;
                            }
                        }
                    }
                    return matrix;
                }
                else return null;
            }
            else return null;
        }
        private static void promptMatrix(char symbol, Macierz m)
        {
            Console.WriteLine("\n Matrix " + symbol + ": ");
            Console.WriteLine("Dimenstions: " + m.GetLength(0) + " x " + m.GetLength(1));
            Console.WriteLine(m.ToString());
            Console.WriteLine("Sum of all cells: " + m.Sum());
        }
        //  private static void ReadMatrix() { 


        static void Main(string[] args)
        {
            Macierz matrixA = new Macierz(0, 0);
            Macierz matrixB = new Macierz(0, 0);
            Macierz matrixC = new Macierz(0, 0);
            for (;;)
            {
                string directory = System.Environment.CurrentDirectory;
                Console.WriteLine("\n Help on:");
                Console.WriteLine("  1. Calculate matrix operation using MatrixLib");
                Console.WriteLine("  2. Read/write files");
                Console.WriteLine("  3. Display .NET components");
                Console.WriteLine("  4. Author");
                Console.WriteLine("  5. Quit");

                Console.Write("Selection: ");
                var ans0 = Console.ReadLine();
                int case0 = 0;
                if (int.TryParse(ans0, out case0))
                {
                    switch (case0)
                    {
                        #region case1
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Welcome. \n This is a program created for the purpose of calculating some basic matrix operations. \n You can add, subtract or multipy. \n");

                            Console.WriteLine("Current memory state: ");
                            if (matrixA.IsNull)
                            {
                                Console.WriteLine("A is empty");
                                if (matrixB.IsNull)
                                {
                                    Console.WriteLine("B is empty");
                                }
                                else
                                {
                                    promptMatrix('B', matrixB);
                                }
                                ExitMessage();
                                break;
                            }
                            else
                            {
                                promptMatrix('A', matrixA);
                                promptMatrix('B', matrixB);
                                #region commented
                                /*
                                var ans11 = Console.ReadLine();
                                int case11 = 0;
                                bool wrongSelect = false;
                                if (int.TryParse(ans11, out case11))
                                {
                                    switch (case11)
                                    {
                                        case 1:

                                            break;
                                        case 2:

                                            Console.Write("Press any key to continue: ");

                                            Console.ReadKey();
                                            break;
                                        case 3: break;
                                        default:
                                            Console.WriteLine("No such selection!!!" + Environment.NewLine + "Press any kay for exit");
                                            wrongSelect = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("You must type numeric value only!!!" + Environment.NewLine + "Press any kay for exit");
                                    wrongSelect = true;

                                }
                                */
                                #endregion

                                Console.WriteLine("\n\nHelp on:");
                                Console.WriteLine("  1. A + B / B + A");
                                Console.WriteLine("  2. A - B");
                                Console.WriteLine("  3. B - A");
                                Console.WriteLine("  4. A * B");
                                Console.WriteLine("  5. B * A");
                                Console.WriteLine("  6. Prompt A and B and their sums");
                                Console.WriteLine("  7. Quit");
                                Console.Write("Choose what u want to do: ");

                                var ans12 = Console.ReadLine();
                                var case12 = 0;

                                #region ifParseSuccess
                                if (int.TryParse(ans12, out case12))
                                {
                                    switch (case12)
                                    {
                                        case 1:
                                            try
                                            {
                                                matrixC = matrixA + matrixB;
                                                Console.WriteLine("\n===========Input=========");
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                Console.WriteLine("\n===========Result=========");
                                                promptMatrix('C', matrixC);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 2:
                                            try
                                            {
                                                matrixC = matrixA - matrixB;
                                                Console.WriteLine("\n===========Input=========");
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                Console.WriteLine("\n===========Result=========");
                                                promptMatrix('C', matrixC);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 3:
                                            try
                                            {
                                                matrixC = matrixB - matrixA;
                                                Console.WriteLine("\n===========Input=========");
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                Console.WriteLine("\n===========Result=========");
                                                promptMatrix('C', matrixC);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 4:
                                            try
                                            {
                                                matrixC = matrixA * matrixB;
                                                Console.WriteLine("\n===========Input=========");
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                Console.WriteLine("\n===========Result=========");
                                                promptMatrix('C', matrixC);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 5:
                                            try
                                            {
                                                matrixC = matrixB * matrixA;
                                                Console.WriteLine("\n===========Input=========");
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                Console.WriteLine("\n===========Result=========");
                                                promptMatrix('C', matrixC);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 6:
                                            try
                                            {
                                                promptMatrix('A', matrixA);
                                                promptMatrix('B', matrixB);
                                                ExitMessage();
                                            }
                                            catch (InvalidOperationException e)
                                            {
                                                Console.WriteLine(e.Message);
                                                ExitMessage();
                                            }
                                            break;
                                        case 7:
                                            ExitMessage();
                                            break;
                                        default:
                                            Console.WriteLine("No such selection!!!");
                                            ExitMessage();
                                            break;

                                    }

                                }
                                #endregion
                                break;
                            }
                        #endregion
                        case 2:
                            Console.WriteLine("1. Enter matrix from console to memory");
                            Console.WriteLine("2. Save matrix from memory to file");
                            Console.WriteLine("3. Read matrix from file to memory");
                            Console.WriteLine("4. Enter matrix from console to memory");
                            var ans2 = Console.ReadLine();
                            int case2 = 0;

                            if (int.TryParse(ans2, out case2))
                            {
                                switch (case2)
                                {
                                    case 1:
                                        Console.WriteLine("Matrix A: ");
                                       Macierz mmm = CreateMatrix();
                                        break;
                                    case 2: break;
                                    case 3:
                                        for (;;)
                                        {
                                            matrixA = CreateMatrix();
                                            if(matrixA == null)
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    case 4:
                                     
                                        Console.WriteLine("Type 'A' to enter matrix A, 'B' to enter matrix B (Q to break) :");
                                        var kkk = Console.Read();
                                        if (kkk == 'A' || kkk == 'a')
                                        {
                                            //Console.ReadKey();
                                            Console.WriteLine("Matrix A: ");
                                            matrixA = CreateMatrix();
                                            if (matrixA == null || !matrixA.IsNull)
                                            {
                                                Console.Write("\n Matrix was not entered corretly, \n");
                                                matrixA = new Macierz(0, 0);
                                            }

                                        }
                                        if (kkk == 'B' || kkk == 'b')
                                        {
                                            Console.WriteLine("Matrix B: ");
                                            matrixB = CreateMatrix();
                                            if (matrixB == null || matrixB.IsNull)
                                            {
                                                Console.WriteLine("\n==============================");
                                                Console.Write("\n Matrix was not entered corretly, \n");
                                            }
                                        }
                                        if (kkk == 'q' || kkk == 'Q')
                                        {
                                            break;
                                        }
                                        break;
                                        
                                        
                                    default: break;
                                }
                            }
                            break;

                        case 3:
                            Console.WriteLine("The for:\n");
                            Console.Write("for(init; condition; iteration)");
                            Console.WriteLine(" statement;");
                            break;
                        case 4:
                            Console.WriteLine("The while:\n");
                            Console.WriteLine("while(condition) statement;");
                            break;
                        case 5:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Unknown statement");
                            break;
                    }
                }
            }
        }
    }
}