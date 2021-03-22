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


        public string Script(IHostDbConnection hostConn, ScripterInput input)
        {
            return Script(hostConn, input.Schema, input.Name);
        }


        private string Script(IHostDbConnection hostConn, string schema, string name)
        {            
            var metadata = new SmoObjectMetadata(schema, name);
            metadata.Initialize(hostConn.Connection);

            var context = new SmoScriptingContext(hostConn, metadata);            
                
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
