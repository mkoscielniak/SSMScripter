using SSMScripter.Integration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunContextProvider : IRunContextProvider
    {
        private IHostContext _hostCtx;
        private IRunConfigStorage _configSrc;

        public RunContextProvider(IHostContext hostCtx, IRunConfigStorage configSrc)
        {
            _hostCtx = hostCtx;
            _configSrc = configSrc;
        }


        public RunContext Get()
        {
            RunConfig config = _configSrc.Load();
            if (config.IsUndefined())
                throw new RunConfigUndefinedException();               

            string connectionStr = _hostCtx.GetCurrentConnectionString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionStr);

            return new RunContext()
            {
                Config = config,
                ConnectionString = builder,
            };
        }
    }
}
