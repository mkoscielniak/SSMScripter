using System;
using System.Data;

namespace SSMScripter.Scripter
{
    public interface IScripter
    {
        string Script(IDbConnection connection, ScripterInput input);
    }

    
    public class ScripterInput
    {
        public string Schema { get; set; }
        public string Name { get; set; }
    }    
}
