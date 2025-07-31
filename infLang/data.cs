using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infLang
{
    internal class data
    {
        //Function Name -> Function Code
        public static ConcurrentDictionary<string, List<string>>functions = new ConcurrentDictionary<string, List<string>>();
        //Function Name -> Variable Name -> Variable Value ( local variables )
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>> variables = new ConcurrentDictionary<string, ConcurrentDictionary<string, dynamic>>();
        //Global variables
        public static ConcurrentDictionary<string, dynamic> globalVariables = new ConcurrentDictionary<string, dynamic>();
    }
}
