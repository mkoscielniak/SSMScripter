﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Commands.Scripter
{
    public class SmoAlterableObject : SmoScriptableObject
    {        
        public SmoAlterableObject(SqlSmoObject obj) : base(obj)
        {      
            
        }


        public override StringCollection Script(SmoScriptingContext context)
        {
            var textObject = ScriptedObject as ITextObject;
            
            if (textObject == null)
                throw new InvalidOperationException(
                    String.Format("ScriptedObject '{0}' cannot be scripted as alter", ScriptedObject.GetType().Name));

            StringCollection result = Script(textObject, context);

            return result;
        }


        private StringCollection Script(ITextObject obj, SmoScriptingContext ctx)
        {
            var output = new StringCollection();

            if (ctx.Options.ScriptDatabaseContext)
            {
                AddDatabaseContext(output, ctx);
                AddBatchSeparator(output, ctx);
                AddEmptyLine(output);
            }

            AddAnsiNulls(output, ctx);
            AddBatchSeparator(output, ctx);
            AddQuotedIdentifier(output, ctx);
            AddBatchSeparator(output, ctx);
            AddEmptyLine(output);
            AddHeader(output, obj);
            AddBody(output, obj);

            return output;
        }
        

        private void AddQuotedIdentifier(StringCollection output, SmoScriptingContext ctx)
        {
            bool? value = ctx.Metadata.QuotedIdentifierStatus;
            if (value.HasValue)
                output.Add(String.Format("SET QUOTED_IDENTIFIER {0}", value.Value ? "ON" : "OFF"));            
        }

        private void AddAnsiNulls(StringCollection output, SmoScriptingContext ctx)
        {
            bool? value = ctx.Metadata.AnsiNullsStatus;
            if(value.HasValue)
                output.Add(String.Format("SET ANSI_NULLS {0}", value.Value ? "ON" : "OFF"));            
        }


        private void AddBody(StringCollection output, ITextObject obj)
        {
            output.Add(obj.TextBody);
        }


        private void AddHeader(StringCollection output, ITextObject obj)
        {
            output.Add(obj.ScriptHeader(true));
        }


        private void AddDatabaseContext(StringCollection output, SmoScriptingContext ctx)
        {
            string batch = String.Format("USE [{0}]", ctx.Connection.DatabaseName);
            output.Add(batch);
        }
    }
}
