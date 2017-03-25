using System.Collections.Specialized;

namespace SSMScripter.Scripter.Smo
{
    public class SmoCreatableObject : SmoScriptableObject
    {
        public SmoCreatableObject(Microsoft.SqlServer.Management.Smo.SqlSmoObject obj)
            : base(obj)
        {
        }
        

        public override StringCollection Script(SmoScriptingContext context)
        {
            var scripter = new Microsoft.SqlServer.Management.Smo.Scripter(context.Server);
            Microsoft.SqlServer.Management.Smo.ScriptingOptions options = scripter.Options;

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
