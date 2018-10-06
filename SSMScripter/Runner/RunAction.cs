using SSMScripter.Integration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunAction
    {
        private IRunContextProvider _contextProvider;
        private IRunParamsProcessor _paramsProcessor;
        private IRunProcessStarter _process;

        public RunAction(IRunContextProvider provider, IRunParamsProcessor processor, IRunProcessStarter process)
        {
            _contextProvider = provider;
            _paramsProcessor = processor;
            _process = process;
        }


        public string Execute()
        {
            try
            {
                RunContext context = _contextProvider.Get();
                RunConfig config = context.Config;
                string args = _paramsProcessor.Compose(config.RunArgs, context.ConnectionString);
                _process.Start(config.RunTool, config.RunArgs);
            }
            catch(RunConfigUndefinedException)
            {
                return "Undefined";
            }
            catch(Exception)
            {
                throw;
            }

            return "OK";
        }
    }
}
