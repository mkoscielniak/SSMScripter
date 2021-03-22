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
            Document activeDocument = _app.ActiveDocument;
            if (activeDocument == null)
                return null;

            TextDocument document = (TextDocument)activeDocument.Object("");
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

            FieldInfo resultControlField = editorControl.GetType()
                .GetField("m_sqlResultsControl", bindingFlags);

            if (resultControlField == null)
                return null;

            object resultsControl = resultControlField.GetValue(editorControl);

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
        

        public IServerConnection CloneCurrentConnection(string database)
        {
            SqlConnectionInfo connectionInfo = GetCurrentSqlConnectionInfo(database);
            IServerConnection serverConn = new HostServerConnection(connectionInfo);
            return serverConn;
        }

        public string GetCurrentConnectionString()
        {
            SqlConnectionInfo connectionInfo = GetCurrentSqlConnectionInfo(null);
            return connectionInfo.ConnectionString;
        }


        private SqlConnectionInfo GetCurrentSqlConnectionInfo(string databaseName)
        {
            CurrentlyActiveWndConnectionInfo connectionInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo;
            databaseName = databaseName ?? connectionInfo.UIConnectionInfo.AdvancedOptions["DATABASE"];

            if (String.IsNullOrEmpty(databaseName))
                throw new ConnectionInfoException("No database context");

            SqlOlapConnectionInfoBase connectionBase = UIConnectionInfoUtil.GetCoreConnectionInfo(connectionInfo.UIConnectionInfo);

            SqlConnectionInfo sqlConnectionInfo = (SqlConnectionInfo)connectionBase;
            sqlConnectionInfo.DatabaseName = databaseName;

            return sqlConnectionInfo;
        }
    }
}
