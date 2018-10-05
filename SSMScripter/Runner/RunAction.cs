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
        private IRunContextProvider _contextProvider;        

        public RunAction(IRunContextProvider provider)
        {
            _contextProvider = provider;
        }


        public string Execute()
        {
            try
            {
                RunContext context = _contextProvider.Get();
            }
            catch(RunConfigUndefinedException ex)
            {
                return "Undefined";
            }
            catch(Exception ex)
            {
                throw;
            }

            return "OK";
        }
    }
}
