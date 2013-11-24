using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.UI.ConnectionDlg;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;

namespace SSMScripter.Commands.Scripter
{
    class SimpleScripter : IScripter
    {        
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


        private enum ContentType
        {
            StoredProcedure, Trigger, Function
        }


        private string Script(string schema, string name)
        {
            string result = null;

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();
                var type = ContentType.StoredProcedure;
                GetDetails(connection, out type, ref schema, ref name);
                string content = GetContent(connection, type, schema, name);
                result = FormatContent(type, schema, name, content);                
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


        private void GetDetails(IDbConnection connection, out ContentType type, ref string schema, ref string name)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = CreateGetDetailsQuery(schema, name);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new ArgumentException("Cannot find database object");

                    type = ConvertToContentType(Convert.ToString(reader["OBJ_TYPE"]));
                    schema = Convert.ToString(reader["OBJ_SCHEMA"]);
                    name = Convert.ToString(reader["OBJ_NAME"]);

                    if (reader.Read())
                        throw new ArgumentException("Too many scriptable objects");
                }
            }
        }


        private ContentType ConvertToContentType(string val)
        {
            var type = ContentType.StoredProcedure;
            
            switch (val.Trim().ToUpper())
            {
                case "P":
                    type = ContentType.StoredProcedure;
                    break;
                case "TR":
                    type = ContentType.Trigger;
                    break;
                case "FN":
                case "IF":
                case "TF":
                    type = ContentType.Function;
                    break;
                default:
                    throw new ArgumentException("Unknown content type value");
            }

            return type;
        }


        private string CreateGetDetailsQuery(string schema, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty");

            string objectName = String.IsNullOrEmpty(schema) ? name : String.Format("[{0}].[{1}]", schema, name);

            const string commandBase = @"
                select 
	                obj.type OBJ_TYPE	
	                , sch.name OBJ_SCHEMA
	                , obj.name OBJ_NAME
                from sys.objects obj
                    join sys.schemas sch on sch.schema_id = obj.schema_id
                where 
	                object_id = object_id(N'{0}')";

            return String.Format(commandBase, objectName);
        }



        private string GetContent(IDbConnection connection, ContentType type, string schema, string name)
        {
            string content = null;

            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = FormatGetContentCommand(schema, name);
                IDataReader reader = cmd.ExecuteReader();
                var sb = new StringBuilder();
                
                while (reader.Read())
                    sb.Append(reader.GetString(0));
                
                content = sb.ToString();
            }

            return content;
        }


        private string FormatGetContentCommand(string schema, string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty");

            string objectName = String.IsNullOrEmpty(schema) ? name : String.Format("[{0}].[{1}]", schema, name);

            const string commandBase = @"
                select text
                from syscomments
                where id = (select object_id('{0}'))";

            return String.Format(commandBase, objectName);
        }


        private string FormatContent(ContentType type, string schema, string name, string content)
        {
            string commandType = GetCommandTypeString(type);
            string createCommand = "CREATE " + commandType;
            string alterCommand = "ALTER " + commandType;

            int pos = content.IndexOf(createCommand, 0, StringComparison.InvariantCultureIgnoreCase);
            
            if (pos >= 0)            
                content = content.Remove(pos, createCommand.Length).Insert(pos, alterCommand);

            return content;
        }


        private string GetCommandTypeString(ContentType type)
        {
            string value = null;

            switch (type)
            {
                case ContentType.StoredProcedure:
                    value = "PROCEDURE";
                    break;
                case ContentType.Trigger:
                    value = "TRIGGER";
                    break;
                case ContentType.Function:
                    value = "FUNCTION";
                    break;
            }

            return value;
        }        
    }
}
