using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SSMScripter.Config;
using SSMScripter.Integration;

namespace SSMScripter.Scripter.Smo
{
    public class SmoScriptingContext
    {
        public SmoObjectMetadata Metadata { get; protected set; }
        public ServerConnection Connection { get; protected set; }
        public Server Server { get; protected set; }
        public Database Database { get; protected set; }        

        public SmoScriptingContext(IHostDbConnection hostConn, SmoObjectMetadata metadata)
        {            
            Connection = new ServerConnection((SqlConnectionInfo)hostConn.ConnectionInfo);            
            Server = new Server(Connection);
            Database = Server.Databases[Connection.DatabaseName];
            
            Metadata = metadata;
            ScriptDatabaseContext = true;
        }

        public bool ScriptDatabaseContext { get; set; }
    }
}
