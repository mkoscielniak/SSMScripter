using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SSMScripter.Config;

namespace SSMScripter.Scripter.Smo
{
    public class SmoScriptingContext
    {
        public SmoObjectMetadata Metadata { get; protected set; }
        public ServerConnection Connection { get; protected set; }
        public Server Server { get; protected set; }
        public Database Database { get; protected set; }
        public Options Options { get; protected set; }

        public SmoScriptingContext(IDbConnection connection, SmoObjectMetadata metadata)
        {
            Connection = new ServerConnection((SqlConnection)connection);
            Server = new Server(Connection);
            Database = Server.Databases[Connection.DatabaseName];
            
            Metadata = metadata;

            Options = new Options();
            Options.Load();
        }
    }
}
