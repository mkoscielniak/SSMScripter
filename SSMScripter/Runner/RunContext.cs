using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunContext
    {
        public RunConfig Config { get; set; }

        public SqlConnectionStringBuilder ConnectionString { get; set; }
    }
}
