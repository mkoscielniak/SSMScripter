using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace SSMScripter.Commands.Scripter
{
    public class SmoScriptableObjectFactory
    {
        private delegate TObject ObjectFactory<out TObject>(SmoScriptingContext context);        

        private readonly Dictionary<SmoObjectType, ObjectFactory<SmoScriptableObject>> _factories;


        public SmoScriptableObjectFactory()
        {
            _factories = new Dictionary<SmoObjectType, ObjectFactory<SmoScriptableObject>>
            {
                {SmoObjectType.Procedure, Alterable(StoredProcedureObj)},
                {SmoObjectType.Function, Alterable(FunctionObj)},
                {SmoObjectType.Trigger, Alterable(TriggerObj)},
                {SmoObjectType.View, Alterable(ViewObj)},
                {SmoObjectType.Table, Creatable(TableObj)}
            };
        }


        public SmoScriptableObject Create(SmoScriptingContext context)
        {
            ObjectFactory<SmoScriptableObject> factory = null;
            SmoObjectType type = context.Metadata.Type;

            if (!_factories.TryGetValue(type, out factory))
                throw new InvalidOperationException("Unknown type of object");

            SmoScriptableObject obj = factory(context);

            return obj;
        }


        private ObjectFactory<SmoScriptableObject> Alterable(ObjectFactory<SqlSmoObject> factory)
        {
            return ctx => new SmoAlterableObject(factory(ctx));
        }


        private ObjectFactory<SmoScriptableObject> Creatable(ObjectFactory<SqlSmoObject> factory)
        {
            return ctx => new SmoCreatableObject(factory(ctx));
        }


        private SqlSmoObject StoredProcedureObj(SmoScriptingContext ctx)
        {
            var storedProcedure = new StoredProcedure(ctx.Database, ctx.Metadata.Name, ctx.Metadata.Schema);
            storedProcedure.Refresh();

            return storedProcedure;
        }


        private SqlSmoObject TriggerObj(SmoScriptingContext ctx)
        {
            var table = new Table(ctx.Database, ctx.Metadata.ParentName, ctx.Metadata.ParentSchema);
            table.Refresh();

            var trigger = new Trigger(table, ctx.Metadata.Name);
            trigger.Refresh();
            
            return trigger;
        }


        private SqlSmoObject FunctionObj(SmoScriptingContext ctx)
        {
            var function = new UserDefinedFunction(ctx.Database, ctx.Metadata.Name, ctx.Metadata.Schema);
            function.Refresh();
            
            return function;
        }


        private SqlSmoObject TableObj(SmoScriptingContext ctx)
        {
            var table = new Table(ctx.Database, ctx.Metadata.Name, ctx.Metadata.Schema);
            table.Refresh();

            return table;
        }

        private SqlSmoObject ViewObj(SmoScriptingContext ctx)
        {
            var view = new View(ctx.Database, ctx.Metadata.Name, ctx.Metadata.Schema);
            view.Refresh();
            
            return view;
        }
    }
}
