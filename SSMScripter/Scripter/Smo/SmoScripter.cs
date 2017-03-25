using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.UI.ConnectionDlg;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using SSMScripter.Integration;

namespace SSMScripter.Scripter.Smo
{
    public class SmoScripter : IScripter
    {
        private readonly SmoScriptableObjectFactory _objectFactory;
        
        public SmoScripter()
        {
            _objectFactory = new SmoScriptableObjectFactory();
        }


        public string Script(IDbConnection connection, ScripterInput input)
        {
            return Script(connection, input.Schema, input.Name);
        }


        private string Script(IDbConnection connection, string schema, string name)
        {            
            var metadata = new SmoObjectMetadata(schema, name);
            metadata.Initialize(connection);

            var context = new SmoScriptingContext(connection, metadata);            
                
            SmoScriptableObject obj = _objectFactory.Create(context);
            StringCollection batches = obj.Script(context);
                
            var builder = new StringBuilder();

            foreach (string batch in batches)
                builder.Append(batch);

            string result = builder.ToString();

            return result;
        }
    }
}
