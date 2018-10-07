using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Scripter
{
    public class ScripterConfigRegistryStorage : IScripterConfigStorage
    {
        private string _masterKey;

        private static readonly string ScriptDatabaseContextKey = "ScriptDatabaseContext";
        

        public ScripterConfigRegistryStorage(string masterKey)
        {
            _masterKey = masterKey;
        }

        public ScripterConfig Load()
        {
            ScripterConfig config = new ScripterConfig();
            config.ScriptDatabaseContext = Convert.ToBoolean(Registry.GetValue(_masterKey, ScriptDatabaseContextKey, config.ScriptDatabaseContext));
            return config;
        }

        public void Save(ScripterConfig config)
        {
            Registry.SetValue(_masterKey, ScriptDatabaseContextKey, config.ScriptDatabaseContext);            
        }
    }
}
