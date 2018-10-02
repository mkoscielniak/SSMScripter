using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunConfig
    {
        public RunConfig()
        {
            RunTool = null;
            RunArgs = null;
        }

        public string RunTool {get; set;}

        public string RunArgs { get; set; }


        public static readonly RunConfig Undefined = new RunConfig();

        public bool IsUndefined()
        {
            return Undefined.Equals(this);
        }
    }
}
