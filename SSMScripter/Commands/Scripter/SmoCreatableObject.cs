using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Smo = Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Commands.Scripter
{
    public class SmoCreatableObject : SmoScriptableObject
    {
        public SmoCreatableObject(Smo.SqlSmoObject obj)
            : base(obj)
        {
        }
        

        public override StringCollection Script(SmoScriptingContext context)
        {
            var scripter = new Smo.Scripter(context.Server);
            Smo.ScriptingOptions options = scripter.Options;

            options.IncludeDatabaseContext = context.Options.ScriptDatabaseContext;
            
            StringCollection scriptingResult = scripter.Script(new[] {ScriptedObject});
            
            var result = new StringCollection();

            foreach (string scriptedBatch in scriptingResult)
            {
                result.Add(scriptedBatch);
                AddLineEnding(result);
                AddLineEnding(result);
            }

            return result;
        }
    }
}
