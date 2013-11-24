using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace SSMScripter.Commands.Scripter
{
    public class SmoObjectMetadata
    {
        public SmoObjectType Type { get; protected set; }
        public string Schema { get; protected set; }
        public string Name { get; protected set; }
        public bool? AnsiNullsStatus { get; protected set; }
        public bool? QuotedIdentifierStatus { get; protected set; }        
        public string ParentSchema { get; protected set; }
        public string ParentName { get; protected set; }


        public SmoObjectMetadata(string schema, string name)
        {
            Schema = schema;
            Name = name;
        }


        public string FullName
        {
            get
            {
                return String.IsNullOrEmpty(Schema) ? Name : String.Format("[{0}].[{1}]", Schema, Name);
            }
        }


        public void Initialize(IDbConnection connection)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                const string commandTemplate = @"
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

                cmd.CommandText = String.Format(commandTemplate, FullName);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new ArgumentException("Cannot find database object");

                    object tmp = null;
                    
                    string loadedType = Convert.ToString(reader["OBJ_TYPE"]);
                    string loadedName = Convert.ToString(reader["OBJ_NAME"]);
                    string loadedSchema = Convert.ToString(reader["OBJ_SCHEMA"]);
                    bool? loadedQuotedIdent = Convert.IsDBNull(tmp = reader["OBJ_QUOTED_IDENTIFIER_STATUS"] ?? DBNull.Value) ? 
                        (bool?)null : Convert.ToBoolean(tmp);
                    bool? loadedAnsiNulls = Convert.IsDBNull(tmp = reader["OBJ_ANSI_NULLS_STATUS"] ?? DBNull.Value) ? 
                        (bool?)null : Convert.ToBoolean(tmp);                    
                    string loadedParentName = Convert.ToString(reader["OBJ_PARENT_NAME"]);
                    string loadedParentSchema = Convert.ToString(reader["OBJ_PARENT_SCHEMA"]);

                    if (!String.IsNullOrEmpty(Schema))
                        if (Schema != loadedSchema)
                            throw new ArgumentException("Cannot find correct database object");

                    if (reader.Read())
                        throw new ArgumentException("Too many scriptable objects");

                    Type = ParseObjectType(loadedType);
                    Name = loadedName;
                    Schema = loadedSchema;
                    AnsiNullsStatus = loadedAnsiNulls;
                    QuotedIdentifierStatus = loadedQuotedIdent;
                    ParentName = loadedParentName;
                    ParentSchema = loadedParentSchema;
                }
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
