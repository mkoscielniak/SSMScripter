using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Scripter
{
    public class ScripterConfig
    {
        public ScripterConfig()
        {
            ScriptDatabaseContext = true;
        }

        public bool ScriptDatabaseContext { get; set; }
    }
}
