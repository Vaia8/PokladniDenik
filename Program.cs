using System;
using System.IO;

/*
 Ukazka souboru - 1. radek pocatecni hodnota, ostatní radky castka a nazev oddelene strednikem

 500
 -200;kolo
 100;kino
*/
namespace pokladnidenik
{
    class Program
    {
        static double initialValue = 0;
        static double[] values = new double[0];
        static string[] names = new string[0];

        static void Main(string[] args)
        {
            ReadFile("C:\\texty\\denik.txt");
            while (true)
            {
                int operation = readOperation();

                switch (operation)
                {
                    case 1:
                        try
                        {
                            printCashDiary();
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Soubor nebyl nalezen.");
                        }
                        break;
                    case 2:
                        Array.Resize(ref values, values.Length + 1);
                        Array.Resize(ref names, names.Length + 1);
                        values[values.Length -1] = readValue();
                        names[names.Length-1] = readName();
                        break;
                    case 3:
                        values = new double[0];
                        names = new string[0];
                        Console.WriteLine("Seznam byl smazan.");
                        Console.WriteLine("Zadejte novou pocatecni hodnotu: ");
                        string s = Console.ReadLine();
                        bool actionSuccess = double.TryParse(s,out initialValue);
                        while (!actionSuccess)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Chybne zadani, zadejte platnou pocatecni hodnotu: ");
                            actionSuccess = double.TryParse(s, out initialValue);
                        }   
                        break;
                    case 4:
                        save("C:\\texty\\denik.txt");
                        System.Environment.Exit(0);
                        break;
                }
            }

        }
        
        static void printCashDiary()
        {
            double balance = initialValue;
            double positive = 0;
            double negative = 0;
            Console.WriteLine("Pocatecni hodnota: " + initialValue);
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine(balance + "  " + values[i] +  " " + names[i] + " " + (balance + values[i]));
                balance = balance + values[i];
                if (values[i] > 0)
                {
                    positive = positive + values[i];
                }
                if (values[i] < 0)
                {
                    negative = negative + values[i];
                }
            }
            Console.WriteLine("Konecna hodnota: " + balance);
            Console.WriteLine("Celkem kladne: " + positive);
            Console.WriteLine("Celkem zaporne: " + negative);
            Console.WriteLine("Celkem: " + (positive + negative));
            Console.WriteLine();
        }
        static void ParseLines(string[] lines)
        {
            Array.Resize(ref values, lines.Length - 1);
            Array.Resize(ref names, lines.Length - 1);
            initialValue = Double.Parse(lines[0]);
            for (int i=1; i<= lines.Length-1; i++)
            {
                string[] s = lines[i].Split(';');
                values[i-1] = Double.Parse(s[0]);
                names[i-1] = s[1];
            }
        }
       static string[] ReadLines(TextReader reader)
        {
            string[] lines = new string[10];
            int count = 0;
            string line = reader.ReadLine();
            while (line != null)
            {
                if (count >= lines.Length)
                {
                    Array.Resize(ref lines, count + 10);
                }
                lines[count] = line;
                line = reader.ReadLine();
                count++;
            }
            Array.Resize(ref lines, count);
            return lines;
        }
        static void ReadFile(string fileName)
        {
            string[] lines;
            using (TextReader reader = new StreamReader(fileName))
            {
               lines = ReadLines(reader);
            }
            ParseLines(lines);
        }
        static void save(string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(initialValue);
               for (int i=0; i<= values.Length-1; i++)
                {
                    writer.WriteLine(values[i]+";"+names[i]);
                }
                
            }
        }
       
        static bool tryReadInteger(out int result)
        {
            string s = Console.ReadLine();
            return int.TryParse(s, out result);
        }
        static int readOperation()
        {
            int result;
            PrintMenu();
            while (!tryReadInteger(out result) || result < 1 || result > 4)
            {
                Console.WriteLine();
                Console.WriteLine("Chybne zadani napiste platne cislo operace: ");
                PrintMenu();
            }
            return result;
        }
        static double readValue()
        {
            double result;
            Console.WriteLine("Zadejte castku");
            string s = Console.ReadLine();
            bool actionSuccess = double.TryParse(s, out result);
            while (!actionSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("Chybne zadani, zadejte castku");
                s = Console.ReadLine();
                actionSuccess = double.TryParse(s, out result);
                Console.WriteLine();
            }
            return result;
        }
        static string readName()
        {
            Console.WriteLine("Zadejte nazev polozky");
            return Console.ReadLine();
        }
       

        static void PrintMenu()
        {
            Console.WriteLine("1: Vypis");
            Console.WriteLine("2: Pridat polozku");
            Console.WriteLine("3: Smazat");
            Console.WriteLine("4: Konec");
        }
        
    }
}