using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunConfigRegistryStorage : IRunConfigStorage
    {
        private string _masterKey;

        private static readonly string RunToolKey = "RunTool";
        private static readonly string RunArgsKey = "RunArgs";

        public RunConfigRegistryStorage(string masterKey)
        {
            _masterKey = masterKey;
        }

        public RunConfig Load()
        {
            object toolObj = Registry.GetValue(_masterKey, RunToolKey, null);
            if (toolObj == null)
                return RunConfig.Undefined;

            string tool = toolObj.ToString();
            if (String.IsNullOrWhiteSpace(tool))
                return RunConfig.Undefined;

            object argsObj = Registry.GetValue(_masterKey, RunArgsKey, String.Empty);

            return new RunConfig()
            {
                RunTool = tool,
                RunArgs = argsObj.ToString(),
            };
        }

        public void Save(RunConfig cfg)
        {
            Registry.SetValue(_masterKey, RunToolKey, cfg.RunTool ?? String.Empty);
            Registry.SetValue(_masterKey, RunArgsKey, cfg.RunArgs ?? String.Empty);
        }
    }
}
