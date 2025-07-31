using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infLang
{
    internal class Program
    {
        static List<string> lines = new List<string>();
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("infLang is a interpreter based language written in c# just for fun");
                Console.WriteLine("Please write your filename to start executing the code.");
                string filename = "test";//Console.ReadLine();
                string path = AppContext.BaseDirectory + filename + ".inf";

                lines = File.ReadAllLines(path).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("func "))
                        i = SaveFunction(i);
                }

                // The main function doesn't exist
                if (!data.functions.ContainsKey("main"))
                {
                    Console.WriteLine("Main function does not exist");
                    return;
                }

                executor.startExecuting();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong!" + ex);
            }
        }

        static int SaveFunction(int funcLine)
        {
            string functionName = lines[funcLine].Substring(5).Trim();
            List<string> functionData = new List<string>();
            //Check starting bracket
            if(lines[++funcLine].Trim() != "{")
            {
                Console.WriteLine($"[infLang] Error at line {++funcLine}: Missing opening bracket for function '{functionName}'");
                return int.MaxValue;
            }

            int bracketCount = 1;
            while (true)
            {
                funcLine++;
                functionData.Add(lines[funcLine].Trim());

                if (lines[funcLine].Trim() == "{")
                    bracketCount++;
                else if (lines[funcLine].Trim() == "}")
                {

                    bracketCount--;
                    if (bracketCount != 0) continue;

                    functionData.RemoveAt(functionData.Count - 1);
                    data.functions.TryAdd(functionName, functionData);
                    return funcLine;
                }
                else if (lines[funcLine].StartsWith("func "))
                {
                    Console.WriteLine($"[infLang] Error at line {++funcLine}: Missing end bracket for function '{functionName}'");
                    return int.MaxValue;
                }
            }


        }
    }
}
