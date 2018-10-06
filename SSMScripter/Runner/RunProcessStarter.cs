using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunProcessStarter : IRunProcessStarter
    {
        public void Start(string file, string args)
        {
            Process.Start(file, args);
        }
    }
}
