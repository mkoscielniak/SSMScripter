using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using SSMScripter.Integration;

namespace SSMScripter.Integration
{
    class Context
    {
        public DTE2 Application { get; private set; }        
        public AddIn AddIn { get; private set; }

        public Context(DTE2 app, AddIn addin)
        {
            Application = app;
            AddIn = addin;
        }


        public Editor GetCurrentEditor()
        {
            TextDocument document = (TextDocument) Application.ActiveDocument.Object("");
            return new Editor(document);
        }


        public Editor CreateNewCurrentEditor()
        {
            ServiceCache.ScriptFactory.CreateNewBlankScript(ScriptType.Sql);
            return GetCurrentEditor();
        }
    }
}
