using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using SSMScripter.Integration;
using System.Reflection;
using Microsoft.SqlServer.Management.UI.Grid;
using System.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.UI.ConnectionDlg;

namespace SSMScripter.Integration.DTE
{
    public class HostContext : IHostContext
    {
        public DTE2 _app;
        
        public HostContext(DTE2 app)
        {
            _app = app;
        }


        public IEditor GetCurrentEditor()
        {
            TextDocument document = (TextDocument) _app.ActiveDocument.Object("");
            return new Editor(document);
        }


        public IEditor GetNewEditor()
        {
            ServiceCache.ScriptFactory.CreateNewBlankScript(ScriptType.Sql);
            return GetCurrentEditor();
        }


        public IResultGrid GetFocusedResultGrid()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            IScriptFactory scriptFactor = ServiceCache.ScriptFactory;
            IVsMonitorSelection monitorSelection = ServiceCache.VSMonitorSelection;

            object editorControl = ServiceCache.ScriptFactory
                .GetType()
                .GetMethod("GetCurrentlyActiveFrameDocView", bindingFlags)
                .Invoke(scriptFactor, new object[] { monitorSelection, false, null });

            object resultsControl = editorControl
                .GetType()
                .GetField("m_sqlResultsControl", bindingFlags)
                .GetValue(editorControl);

            object resultsTabPage = resultsControl
                .GetType()
                .GetField("m_gridResultsPage", bindingFlags)
                .GetValue(resultsControl);

            IGridControl grid = (IGridControl)resultsTabPage
                .GetType()
                .BaseType
                .GetProperty("FocusedGrid", bindingFlags)
                .GetValue(resultsTabPage, null);

            if (grid == null)
                return null;

            return new ResultGrid(grid);
        }


        public IDbConnection CloneCurrentConnection()
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
