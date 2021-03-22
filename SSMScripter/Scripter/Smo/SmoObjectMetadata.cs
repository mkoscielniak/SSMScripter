using SSMScripter.Integration;
using System;
using System.Data;

namespace SSMScripter.Scripter.Smo
{
    public class SmoObjectMetadata
    {
        public SmoObjectType Type { get; protected set; }
        public string Schema { get; protected set; }
        public string Name { get; protected set; }
        public string ParentSchema { get; protected set; }
        public string ParentName { get; protected set; }

        public bool? AnsiNullsStatus { get; set; }
        public bool? QuotedIdentifierStatus { get; set; }


        public SmoObjectMetadata(SmoObjectType type, string schema, string name, string parentSchema, string parentName)
        {
            Type = type;
            Schema = schema;
            Name = name;
            ParentSchema = parentSchema;
            ParentName = parentName;
        }
    }
}
