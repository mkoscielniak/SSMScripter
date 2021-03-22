using SSMScripter.Integration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Scripter.Smo
{
    public class SmoObjectMetadataFactory
    {
        private const string QUERY_TEMPLATE = @"
            select
	            rtrim(obj.type) OBJ_TYPE
	            , sch.name OBJ_SCHEMA
	            , obj.name OBJ_NAME
                , objectproperty(obj.object_id, 'IsAnsiNullsOn') OBJ_ANSI_NULLS_STATUS
                , objectproperty(obj.object_id, 'IsQuotedIdentOn') OBJ_QUOTED_IDENTIFIER_STATUS	                    
	            , isnull(sch_parent.name,'') OBJ_PARENT_SCHEMA
	            , isnull(obj_parent.name,'') OBJ_PARENT_NAME                        
            from sys.objects obj 
            join sys.schemas sch on sch.schema_id = obj.schema_id
            left join sys.objects obj_parent on obj_parent.object_id = obj.parent_object_id
            left join sys.schemas sch_parent on sch_parent.schema_id = obj_parent.schema_id
            where 
	            obj.object_id = object_id(N'{0}')";


        public SmoObjectMetadata Create(IServerConnection serverConn, string schema, string name)
        {
            string fullName = String.IsNullOrEmpty(schema) ? name : String.Format("[{0}].[{1}]", schema, name);
            string query = String.Format(QUERY_TEMPLATE, fullName);

            using (IDataReader reader = serverConn.ExecuteReader(query))
            {
                if (!reader.Read())
                    throw new ArgumentException("Cannot find database object");

                object tmp = null;

                string objType = Convert.ToString(reader["OBJ_TYPE"]);
                SmoObjectType type = ParseObjectType(objType);
                string objName = Convert.ToString(reader["OBJ_NAME"]);
                string objSchema = Convert.ToString(reader["OBJ_SCHEMA"]);
                bool? quotedIdent = Convert.IsDBNull(tmp = reader["OBJ_QUOTED_IDENTIFIER_STATUS"] ?? DBNull.Value) ?
                    (bool?)null : Convert.ToBoolean(tmp);
                bool? ansiNulls = Convert.IsDBNull(tmp = reader["OBJ_ANSI_NULLS_STATUS"] ?? DBNull.Value) ?
                    (bool?)null : Convert.ToBoolean(tmp);
                string parentName = Convert.ToString(reader["OBJ_PARENT_NAME"]);
                string parentSchema = Convert.ToString(reader["OBJ_PARENT_SCHEMA"]);

                if (!String.IsNullOrEmpty(schema))
                    if (schema != objSchema)
                        throw new ArgumentException("Cannot find correct database object");

                if (reader.Read())
                    throw new ArgumentException("Too many scriptable objects");

                return new SmoObjectMetadata(type, objSchema, objName, parentSchema, parentName)
                {
                    QuotedIdentifierStatus = quotedIdent,
                    AnsiNullsStatus = ansiNulls,
                };
            }

        }


        private SmoObjectType ParseObjectType(string type)
        {
            switch (type)
            {
                case "P":
                    return SmoObjectType.Procedure;
                case "TR":
                    return SmoObjectType.Trigger;
                case "FN":
                case "IF":
                case "TF":
                    return SmoObjectType.Function;
                case "U":
                    return SmoObjectType.Table;
                case "V":
                    return SmoObjectType.View;
                default:
                    throw new ArgumentException(
                        String.Format("Unknown object type '{0}'", type ?? "null"));
            }
        }
    }
}
