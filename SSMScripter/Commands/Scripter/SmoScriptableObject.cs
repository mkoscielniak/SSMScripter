using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Commands.Scripter
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
        }


        public void AddEmptyLine(StringCollection output)
        {
            output.Add(String.Empty);
        }

    }
}
