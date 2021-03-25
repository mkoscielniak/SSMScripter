using System;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Scripter.Smo
{
    public abstract class SmoScriptableObject
    {
        public SqlSmoObject ScriptedObject { get; protected set; }


        protected SmoScriptableObject(SqlSmoObject obj)
        {
            ScriptedObject = obj;
        }


        public abstract StringCollection Script(SmoScriptingContext context);


        protected void AddBatchSeparator(StringCollection output, SmoScriptingContext ctx)
        {
            output.Add(ctx.Connection.BatchSeparator);
            AddLineEnding(output);
        }


        protected void AddLineEnding(StringCollection output)
        {
            output.Add(Environment.NewLine);
        }


        protected void AddDatabaseContext(StringCollection output, SmoScriptingContext ctx)
        {
            string batch = String.Format("USE [{0}]", ctx.Connection.DatabaseName);
            output.Add(batch);
            AddLineEnding(output);
        }
    }
}
