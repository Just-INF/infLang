using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infLang
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("infLang is a interpreter based language written in c# just for fun");
                Console.WriteLine("Please write your filename to start executing the code.");
                string filename = Console.ReadLine();//Console.ReadLine();
                string path = AppContext.BaseDirectory + filename + ".inf";

                List<string> lines = new List<string>();
                lines = File.ReadAllLines(path).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("import "))
                        saveImport(lines[i].Substring(7));
                    else if (lines[i].StartsWith("func "))
                        i = SaveFunction(i, lines);
                    else if (lines[i].StartsWith("string "))
                    {
                        string line = lines[i].Substring(7).Trim();
                        string[] split = line.Split('=');
                        string varName = split[0].Trim();
                        string varValue = split.Length == 2 ? split[1].Trim().Trim('"') : "";
                        data.globalVariables[varName] = varValue;
                    }
                    else if (lines[i].StartsWith("num "))
                    {
                        string line = lines[i].Substring(4).Trim();
                        string[] split = line.Split('=');

                        string varName = split[0].Trim();
                        int? varValue = null;
                        if (split.Length == 2)
                        {
                            int parsed;
                            if (int.TryParse(split[1].Trim(), out parsed))
                            {
                                varValue = parsed;
                            }
                        }
                        data.globalVariables[varName] = varValue;
                    }
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

        static int SaveFunction(int funcLine, List<string> lines)
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

        static HashSet<string> importedFiles = new HashSet<string>();
        static void saveImport(string filename) 
        {
            try
            {
                string path = AppContext.BaseDirectory + filename + ".inf";
                if (importedFiles.Contains(path)) return;
                importedFiles.Add(path);

                if (!File.Exists(path))
                {
                    Console.WriteLine($"[infLang] Import file not found: {path}");
                    return;
                }
                List<string> split = filename.Split('\\').ToList();
                string filePath = "";
                for (int i = 0; i < split.Count - 1; i++)
                {
                    filePath = filePath + split[i] + "\\";
                }

                List<string> lines = new List<string>();
                lines = File.ReadAllLines(path).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("import "))
                    {
                        string newImport = filePath + lines[i].Substring(7);
                        saveImport(newImport);
                    }
                    else if (lines[i].StartsWith("func "))
                        i = SaveFunction(i, lines);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[infLang]" + ex.ToString());
            }
        }
    }
}
