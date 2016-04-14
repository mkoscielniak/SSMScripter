using System;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Scripter
{
    public abstract class SmoScriptableObject
    {
        public SqlSmoObject ScriptedObject { get; protected set; }


        protected SmoScriptableObject(SqlSmoObject obj)
        {
            ScriptedObject = obj;
        }


        public abstract StringCollection Script(SmoScriptingContext context);


        public void AddBatchSeparator(StringCollection output, SmoScriptingContext ctx)
        {
            output.Add(ctx.Connection.BatchSeparator);
            AddLineEnding(output);
        }


        public void AddLineEnding(StringCollection output)
        {
            output.Add(Environment.NewLine);
        }

    }
}
