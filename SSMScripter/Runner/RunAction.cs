using SSMScripter.Integration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunAction
    {
        private IHostContext _hostCtx;

        public RunAction(IHostContext hostCtx)
        {
            _hostCtx = hostCtx;
        }


        public string Execute()
        {            
            return "OK";
        }
    }
}
