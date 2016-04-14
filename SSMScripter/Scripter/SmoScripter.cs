using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.UI.ConnectionDlg;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;

namespace SSMScripter.Scripter
{
    public class SmoScripter : IScripter
    {
        public SmoScripter()
        {            
        }


        public bool TryScript(ScripterInput input, out ScripterResult result)
        {
            result = new ScripterResult();

            try
            {
                result.Text = Script(input.Schema, input.Name);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return false;
            }

            return true;
        }


        private string Script(string schema, string name)
        {
            string result = null;

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();

                var metadata = new SmoObjectMetadata(schema, name);
                metadata.Initialize(connection);

                var context = new SmoScriptingContext(connection, metadata);                
                var factory = new SmoScriptableObjectFactory();                                
                
                SmoScriptableObject obj = factory.Create(context);
                StringCollection batches = obj.Script(context);
                
                var builder = new StringBuilder();

                foreach (string batch in batches)
                    builder.Append(batch);

                result = builder.ToString();
            }

            return result;
        }


        private IDbConnection CreateDbConnection()
        {
            CurrentlyActiveWndConnectionInfo connectionInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo;
            string databaseName = connectionInfo.UIConnectionInfo.AdvancedOptions["DATABASE"];

            SqlOlapConnectionInfoBase connectionBase = UIConnectionInfoUtil.GetCoreConnectionInfo(connectionInfo.UIConnectionInfo);

            var sqlConnectionInfo = (SqlConnectionInfo)connectionBase;
            sqlConnectionInfo.DatabaseName = databaseName;

            IDbConnection connection = sqlConnectionInfo.CreateConnectionObject();

            return connection;
        }
    }
}
