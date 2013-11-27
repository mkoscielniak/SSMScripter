using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace SSMScripter.Config
{
    public class Options
    {
        private static readonly string MasterKey = Registry.CurrentUser.Name + "\\Software\\SSMScripter";
        
        private static readonly string ScriptDatabaseContextKey = "ScriptDatabaseContext";
        public bool ScriptDatabaseContext { get; set; }


        public Options()
        {
            ScriptDatabaseContext = true;
        }


        public void Load()
        {
            object firstValue = Registry.GetValue(MasterKey, ScriptDatabaseContextKey, ScriptDatabaseContext);
            
            if (firstValue == null)
                return;

            ScriptDatabaseContext = Convert.ToBoolean(firstValue);
        }


        public void Store()
        {
            Registry.SetValue(MasterKey, ScriptDatabaseContextKey, ScriptDatabaseContext);
        }
    }
}
