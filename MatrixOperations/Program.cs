using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MatrixOperations
{
    class Program
    {
        #region StaticMethods
        public static void AssemblyInfo()
        {
            Assembly assembly = Assembly.LoadFrom("Macierz.dll");

            Console.Write("======================================\n" + assembly.ManifestModule + " Assembly Info \n======================================\n");

            AssemblyDescriptionAttribute attribute = assembly
      .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
      .OfType<AssemblyDescriptionAttribute>()
      .SingleOrDefault();

            var verInfo = FileVersionInfo.GetVersionInfo(typeof(Macierz.Macierz).Assembly.Location);
            Console.Write("Class :" + verInfo.ProductName + " \n");
            Console.WriteLine("Assembly title: " + verInfo.FileDescription);
            Console.WriteLine("Assembly description:  " + attribute.Description);
            Console.WriteLine("Version: " + verInfo.FileVersion);
            Console.WriteLine("Author: " + verInfo.LegalTrademarks);
            Console.WriteLine("Company: " + verInfo.CompanyName);
            Module manifest = typeof(Macierz.Macierz).Assembly.ManifestModule;
            var types = manifest.GetTypes();
            foreach (var type in types)
            {
                Console.WriteLine("\n"+type);
                Console.Write("\n Fields: \n");
                foreach (var field in @type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    Console.WriteLine("   " + field.Attributes.ToString() + " " + field.ToString());
                }
                //    }
                Console.WriteLine();
                Console.Write(" Methods: \n");
                var meths = type.GetMethods();
                foreach (var m in meths)
                {
                    var attrs = m.Attributes;
                    var methodAttribues = attrs.ToString().Split(',');
                    Console.WriteLine("     Attributes: " + methodAttribues[1] + "\n     Name: " + m.Name + "()" + "\n     Return Parameter: " + m.ReturnParameter);
                    if (m.GetParameters().Length != 0)
                    {
                        Console.WriteLine("\n         Arguments (Parameters): ");
                        var s = m.GetParameters();
                        foreach (var ss in s)
                        {
                            Console.WriteLine("                 " + ss);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
        private static void InputResult(Macierz.Macierz a, Macierz.Macierz b, Macierz.Macierz c)
        {
            Console.WriteLine("\n===========Input=========");
            PromptMatrix('A', a);
            PromptMatrix('B', b);
            Console.WriteLine("\n===========Result=========");
            PromptMatrix('C', c);
            ExitMessage();
        }
        private static void MainMenu()
        {
            Console.WriteLine("\nOne or more matrices are empty. Sorry, cannot continue.");
            Console.Write("Press any key to return to main menu: ");
            Console.ReadKey();
            Console.Clear();
        }
        private static void NoSelection()
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("No such selection");
            Console.WriteLine("===================================");
        }
        private static void WrongValue()
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("You must type numeric value only!!!");
            Console.WriteLine("===================================");
        }
        private static void ExitMessage()
        {
           // Console.Clear();
            Console.WriteLine("Enter any key to return to main menu or Q to quit");
            var s = Console.ReadLine();
            if (s == "q" || s == "Q")
            {
                Environment.Exit(0);
            }
        }
        private static Macierz.Macierz ReadMatrix(string filename)
        {
            try
            {
                System.Collections.Generic.List<string> vals = new System.Collections.Generic.List<string>();
                StreamReader sr = new StreamReader(filename + ".txt");

                var firstLine = sr.ReadLine();
                string[] parts = firstLine.Split(' ');

                int m = 0;
                int n = 0;
                m = int.Parse(parts[0]);
                n = int.Parse(parts[1]);

                float[,] result = new float[m, n];
                for (int i = 0; i < m; i++)
                {
                    var str = sr.ReadLine();
                    string[] txt = str.Split('\t');
                    for (int j = 0; j < n; j++)
                    {
                        float f = float.Parse(txt[j]);
                        result[i, j] = f;
                    }
                }
                sr.Close();
                return new Macierz.Macierz(result);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found, try entering without extension !");
                return new Macierz.Macierz(0, 0);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception occured.");
                Console.WriteLine("Check if it file has appropriate formatting.");
                Console.WriteLine("Reading file failed.");
                return new Macierz.Macierz(0, 0);
            }
        }
        private static void WriteFile(Macierz.Macierz m)
        {
            if (!m.IsZero)
            {
                try
                {
                    for (;;)
                    {
                        bool fc = false;
                        Console.Write("Enter filename (without the .txt extension)[Q to break]: ");
                        var crl = Console.ReadLine();
                        crl.Trim();
                        if (crl == "Q" || crl == "q")
                        {
                            break;
                        }
                        char[] arr = crl.ToCharArray();
                        foreach (var a in arr)
                        {
                            if (a == '<' || a == '>' || a == '?' || a == '\\' || a == '/' || a == '/' || a == '*' || a == ':' || a == '?' || a == '|' || a == '"')
                            {
                                fc = true;
                                break;
                            }
                        }
                        if (fc)
                        {
                            Console.Write("String contains forbidden characters.\nTry entering a diffrent filename.\n");
                            continue;
                        }
                        if (File.Exists(crl + ".txt"))
                        {
                            Console.Write("File already exits, try diffrent name \n");
                            Console.Write("Type 'overwrite' to overwrite of any key to change name \n");
                            var crl1 = Console.ReadLine();
                            if (crl1?.Trim() != "overwrite")
                            {
                                continue;
                            }
                        }
                        StreamWriter sw = new StreamWriter(crl + ".txt");
                        sw.WriteLine(m.GetLength(0) + " " + m.GetLength(1));
                        var matat = m.ToString();
                        string[] lines = matat.Split('\n');
                        foreach (var l in lines)
                        {
                            sw.WriteLine(l);
                        }
                        sw.Close();
                        Console.Write("\n =============File saved.============\n");
                        break;
                    }
                }


                catch (Exception e)
                {
                    Console.Write("Exception occurred while writing file");
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine("Matrix is empty");
                Console.WriteLine("No files were created");
            }
        }
        private static Macierz.Macierz CreateMatrix()
        {
            Console.WriteLine("You are creating an m-by-n matrix.");
            Console.Write("Enter m: ");
            var choiceM = Console.ReadLine();
            int m = 0;
            if (int.TryParse(choiceM, out m) && m > 0)
            {
                Console.Write("Enter n: ");
                var choiceN = Console.ReadLine();
                int n = 0;
                if (int.TryParse(choiceN, out n) && n > 0)
                {
                    Macierz.Macierz matrix = new Macierz.Macierz(m, n);

                    Console.WriteLine("\n Type values (from left to right and top to bottom). Use \",\" as decimal point. Press 'Enter' after each value.");
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            float f = 0;
                            var val = Console.ReadLine();
                            if (float.TryParse(val?.Trim(), out f))
                            {
                                matrix[i, j] = f;
                            }
                            else
                            {
                                Console.Write("Ops, wrong value \n");

                                Console.Write("===============\n HINT\n =============== \n Read the above. Enter only numeric values. \n ------------------------------");
                                return new Macierz.Macierz(0, 0);
                            }
                        }
                    }
                    return matrix;
                }
                else return new Macierz.Macierz(0, 0);
            }
            else return new Macierz.Macierz(0, 0);
        }
        private static void PromptMatrix(char symbol, Macierz.Macierz m)
        {
            if (m.IsZero)
            {
                Console.WriteLine("\n Matrix " + symbol + " is empty.");
            }
            else
            {
                Console.WriteLine("\n Matrix " + symbol + ": ");
                Console.WriteLine("Dimensions: " + m.GetLength(0) + " x " + m.GetLength(1));
                Console.WriteLine(m.ToString());
                Console.WriteLine("Sum of all cells: " + m.Sum());
            }
        }
        private static void PromptMatrix(string text, Macierz.Macierz m)
        {
            Console.WriteLine("\n " + text + ": ");
            Console.WriteLine("Dimensions: " + m.GetLength(0) + " x " + m.GetLength(1));
            Console.WriteLine(m.ToString());
            Console.WriteLine("Sum of all cells: " + m.Sum());
        }
        #endregion
        static void Main(string[] args)
        {
            Macierz.Macierz matrixA = new Macierz.Macierz(0, 0);
            Macierz.Macierz matrixB = new Macierz.Macierz(0, 0);
            Macierz.Macierz matrixC = new Macierz.Macierz(0, 0);

            Console.WriteLine("Welcome to program called MatrixOperations.");
            for (;;)
            {
                //  string directory = System.Environment.CurrentDirectory;
                Console.WriteLine("\n Help on:");
                Console.WriteLine("  1. Calculate matrix operation using MatrixLib");
                Console.WriteLine("  2. Enter matrix from console and also read/write text files");
                Console.WriteLine("  3. Display .NET components");
                Console.WriteLine("  4. Author");
                Console.WriteLine("  5. Clear console");
                Console.WriteLine("  6. Help");
                Console.WriteLine("  7. Quit");

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

                            //If mememory is empty or sth
                            bool allGood = !matrixA.IsZero && !matrixB.IsZero;
                            bool w1 = matrixA.IsZero && !matrixB.IsZero;
                            bool w2 = !matrixA.IsZero && matrixB.IsZero;
                            bool w3 = !allGood;
                            bool w4 = w1 || w2 || w3;

                            if (w1)
                            {
                                Console.WriteLine("Matrix A is empty \n");
                                PromptMatrix('B', matrixB);
                                MainMenu();
                                break;
                            }
                            if (w2)
                            {
                                PromptMatrix('A', matrixA);
                                Console.WriteLine("");
                                Console.WriteLine("Matrix B is empty");
                                MainMenu();
                                break;
                            }
                            if (w3)
                            {
                                Console.WriteLine("Matrix A is empty \n");
                                Console.WriteLine("Matrix B is empty");
                                MainMenu();
                                break;
                            }
                            else
                            {
                                if (matrixC.IsZero)
                                {
                                    Console.WriteLine("\nMatrix C is currently empty");
                                    Console.WriteLine("\nContinue to calculate matrix C");
                                }
                                else
                                {
                                    PromptMatrix('C', matrixC);
                                    Console.WriteLine("\nContinue to overwrite matrix C");
                                }
                            }
                            //Else if is it's all good
                            if (allGood)
                            {
                                PromptMatrix('A', matrixA);
                                PromptMatrix('B', matrixB);

                                Console.WriteLine("\n\nHelp on:");
                                Console.WriteLine("  1. C = A + B / B + A");
                                Console.WriteLine("  2. C = A - B");
                                Console.WriteLine("  3. C = B - A");
                                Console.WriteLine("  4. C = A * B");
                                Console.WriteLine("  5. C = B * A");
                                Console.WriteLine("  6. Prompt A and B and their sums");
                                Console.WriteLine("  7. Quit");
                                Console.Write("Choose what u want to do: ");

                                var ans12 = Console.ReadLine();
                                var case12 = 0;

                                #region ifAllGood
                                if (int.TryParse(ans12, out case12))
                                {
                                    switch (case12)
                                    {
                                        case 1:
                                            matrixC = matrixA + matrixB;
                                            InputResult(matrixA, matrixB, matrixC);
                                            break;
                                        case 2:
                                            matrixC = matrixA - matrixB;
                                            InputResult(matrixA, matrixB, matrixC);

                                            break;
                                        case 3:
                                            matrixC = matrixB - matrixA;
                                            InputResult(matrixA, matrixB, matrixC);
                                            break;
                                        case 4:

                                            matrixC = matrixA * matrixB;
                                            InputResult(matrixA, matrixB, matrixC);
                                            break;
                                        case 5:
                                            matrixC = matrixB * matrixA;
                                            InputResult(matrixA, matrixB, matrixC);
                                            break;
                                        case 6:
                                            PromptMatrix('A', matrixA);
                                            PromptMatrix('B', matrixB);
                                            ExitMessage();
                                            break;
                                        case 7:
                                            ExitMessage();
                                            break;
                                        default:
                                            NoSelection();
                                            ExitMessage();
                                            break;
                                    }

                                }
                                else
                                {
                                    WrongValue();
                                }
                                #endregion
                                break;
                            }
                            break;
                        #endregion
                        #region case2
                        case 2:
                            Console.WriteLine("\n Help on:");
                            Console.WriteLine("1. Enter matrix from console to memory");
                            Console.WriteLine("2. Save matrix from memory to file");
                            Console.WriteLine("3. Read matrix from file to memory");
                            Console.WriteLine("4. Display memory state");
                            Console.Write("Selection: ");
                            var ans2 = Console.ReadLine();
                            int case2 = 0;
                            if (int.TryParse(ans2, out case2))
                            {
                                switch (case2)
                                {
                                    case 4:
                                        {
                                            PromptMatrix('A', matrixA);
                                            PromptMatrix('B', matrixB);
                                            PromptMatrix('C', matrixC);
                                            break;
                                        }
                                    case 2:
                                        Console.WriteLine("Type 'A' to save matrix A, 'B' to save matrix B or 'C' to save matrix C [Q to break]: ");
                                        var k2 = Console.ReadLine();
                                        if (k2 == "A" || k2 == "a")
                                        {
                                            WriteFile(matrixA);
                                            break;
                                        }
                                        else if (k2 == "B" || k2 == "b")
                                        {
                                            WriteFile(matrixB);
                                            break;
                                        }
                                        else if (k2 == "C" || k2 == "c")
                                        {
                                            WriteFile(matrixC);
                                            break;
                                        }
                                        else if (k2 == "Q" || k2 == "q")
                                        {
                                            break;
                                        }
                                        break;
                                    case 3:
                                        Console.WriteLine("Type the name of the file :");
                                        var k3 = Console.ReadLine();
                                        Macierz.Macierz temp = ReadMatrix(k3?.Trim());
                                        if (!temp.IsZero)
                                        {
                                            PromptMatrix("Matrix found in file: ", temp);
                                            Console.WriteLine("Type 'A' to overwrite matrix A, 'B' to overwrite matrix B [Q to break] :");
                                            var k31 = Console.ReadLine();
                                            if (k31 == "A" || k31 == "a")
                                            {
                                                matrixA = temp;
                                            }
                                            else if (k31 == "B" || k31 == "b")
                                            {
                                                matrixB = temp;
                                            }
                                            else if (k31 == "Q" || k31 == "q")
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    case 1:
                                        Console.WriteLine("Type 'A' to enter matrix A, 'B' to enter matrix B [Q to break) :");
                                        var kkk = Console.ReadLine();
                                        if (kkk == "A" || kkk == "a")
                                        {
                                            Console.WriteLine("Matrix A: ");
                                            matrixA = CreateMatrix();
                                            if (!matrixA.IsZero)
                                            {
                                                Console.WriteLine("\n==============================");
                                                Console.Write("\n Matrix was entered corretly, \n");
                                            }
                                            else
                                            {
                                                Console.Write("\n Matrix was NOT entered corretly, \n");
                                            }
                                        }
                                        else if (kkk == "B" || kkk == "b")
                                        {
                                            Console.WriteLine("Matrix B: ");
                                            matrixB = CreateMatrix();
                                            if (!matrixB.IsZero)
                                            {
                                                Console.WriteLine("\n==============================");
                                                Console.Write("\n Matrix was entered corretly, \n");
                                            }
                                            else
                                            {
                                                Console.Write("\n Matrix was NOT entered corretly, \n");
                                            }
                                        }
                                        else if (kkk == "q" || kkk == "Q")
                                        {
                                            break;
                                        }
                                        break;
                                    default:
                                        NoSelection();
                                        break;
                                }
                            }
                            else
                            {
                                WrongValue();
                                ExitMessage();
                                break;
                            }
                            break;
                        #endregion
                        case 3:
                            AssemblyInfo();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("===================================");
                            Console.WriteLine("The author of this program: \n \nŁukasz Kandziora, SSM Informatyka AEI, Politechnika Śląska \n");
                            Console.WriteLine("===================================");
                            break;
                        case 5:
                            Console.Clear();
                            break;
                        case 6:
                            Console.WriteLine("In menus use only numeric values like 1,2,3 ... to select the option of your preference.");
                            Console.WriteLine("The memory state doesn't change if you enter a matrix incorrectly.");
                            Console.ReadKey();
                            break;
                        case 7:
                            Environment.Exit(0);
                            break;
                        default:
                            NoSelection();
                            break;
                    }
                }
                else
                {
                    WrongValue();
                }
            }
        }
    }
}