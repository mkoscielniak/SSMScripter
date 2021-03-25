using System;
using System.Collections.Specialized;
using MSmo = Microsoft.SqlServer.Management.Smo;

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
            var scripter = new MSmo.Scripter(context.Server);
            scripter.Options.IncludeDatabaseContext = context.ScriptDatabaseContext;
            
            StringCollection scriptingResult = scripter.Script(new[] {ScriptedObject});                       

            var result = new StringCollection();
            
            AddDatabaseContextIfNeeded(context, result, scriptingResult);

            foreach (string scriptedBatch in scriptingResult)
            {
                result.Add(scriptedBatch);
                AddLineEnding(result);
                AddLineEnding(result);
            }

            return result;
        }
        
        private void AddDatabaseContextIfNeeded(SmoScriptingContext context, StringCollection output, StringCollection scriptingResult)
        {
            if (scriptingResult.Count == 0)
                return;

            string firstLine = scriptingResult[0];

            if (String.IsNullOrEmpty(firstLine))
                return;

            if (firstLine.StartsWith("USE", StringComparison.InvariantCultureIgnoreCase))
                return;

            AddDatabaseContext(output, context);
            AddLineEnding(output);
        }
    }
}
