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
        private readonly SmoObjectMetadataFactory _metadataFactory;
        private readonly SmoScriptableObjectFactory _objectFactory;

        public SmoScripter()
        {
            _metadataFactory = new SmoObjectMetadataFactory();
            _objectFactory = new SmoScriptableObjectFactory();
        }


        public StringCollection Script(IServerConnection serverConn, ScripterInput input)
        {
            return Script(serverConn, input.Schema, input.Name);
        }


        private StringCollection Script(IServerConnection serverConn, string schema, string name)
        {
            var metadata = _metadataFactory.Create(serverConn, schema, name);
            var context = new SmoScriptingContext(serverConn, metadata);

            SmoScriptableObject obj = _objectFactory.Create(context);
            StringCollection batches = obj.Script(context);

            return batches;
        }
    }
}
