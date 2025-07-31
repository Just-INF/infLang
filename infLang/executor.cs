using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infLang
{
    internal class executor
    {
        public static void startExecuting()
        {
            loopFunction(data.functions["main"], "main");
        }

        static void loopFunction(List<string> currentFunc, string funcName)
        {
            for (int i = 0; i < currentFunc.Count; i++)
            {
                execLine(currentFunc[i], i, funcName);
            }
        }

        static void execLine(string line, int lineNum, string funcName)
        {
            line = line.Trim();
            if (line.StartsWith("//")) // Handle comments
                return;


            if (line.StartsWith("call "))
            {
                line = line.Substring(4).Trim();
                string newFunc = line.Split('(')[0];
                if (!data.functions.ContainsKey(newFunc))
                {
                    Console.WriteLine("Function " + line + " was not found!");
                    return;
                }

                loopFunction(data.functions[newFunc], newFunc);
            }
            else if (line.StartsWith("say(\""))
            {
                line = line.Substring(5, line.Length - 7);
                Console.WriteLine(line);
            }
            else if (line.StartsWith("string "))
            {
                line = line.Substring(7).Trim();
                Console.WriteLine(line + " is a string variable, but it is not assigned to anything yet.");
                string[] split = line.Split('=');
                if (split.Length == 2)
                {
                    split[0].Trim();
                    if(!data.variables.ContainsKey(funcName))
                        data.variables[funcName] = new ConcurrentDictionary<string, dynamic>();

                    ConcurrentDictionary<string, dynamic> funcVars = data.variables[funcName];
                    funcVars.TryAdd(split[0].Trim(), split[1].Trim().Trim('"')); // Store the string variable
                    data.variables[funcName] = funcVars; // Update the dictionary
                }
            } 
        }
    }
}
