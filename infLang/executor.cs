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
                string content = line.Substring(5, line.Length - 7);

                if (data.variables.ContainsKey(funcName))
                {
                    foreach (var kv in data.variables[funcName])
                    {
                        content = content.Replace("{" + kv.Key + "}", kv.Value.ToString());
                    }
                }

                foreach (var kv in data.globalVariables)
                {
                    content = content.Replace("{" + kv.Key + "}", kv.Value.ToString());
                }

                Console.WriteLine(content);
            }
            else if (line.StartsWith("string "))
            {
                line = line.Substring(7).Trim();
                string[] split = line.Split('=');

                string varName = split[0].Trim();
                string varValue = split.Length == 2 ? split[1].Trim().Trim('"') : "";

                if (!data.variables.ContainsKey(funcName))
                    data.variables[funcName] = new ConcurrentDictionary<string, dynamic>();

                data.variables[funcName][varName] = varValue;

                //Console.WriteLine(split[0].Trim() + " is now a string variable with value: " + data.variables[funcName][split[0].Trim()]);
            } 
            else if (line.StartsWith("num "))
            {
                line = line.Substring(4).Trim();
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

                if (!data.variables.ContainsKey(funcName))
                    data.variables[funcName] = new ConcurrentDictionary<string, dynamic>();

                data.variables[funcName][varName] = varValue;

                //Console.WriteLine(split[0].Trim() + " is now a int variable with value: " + data.variables[funcName][split[0].Trim()]);
            }
        }
    }
}
