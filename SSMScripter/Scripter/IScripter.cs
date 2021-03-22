using SSMScripter.Integration;
using System;
using System.Data;

namespace SSMScripter.Scripter
{
    public interface IScripter
    {
        string Script(IHostDbConnection hostConn, ScripterInput input);
    }

    
    public class ScripterInput
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public bool ScriptDatabaseContext { get; set; }
    }    
}
