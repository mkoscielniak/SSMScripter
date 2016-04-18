using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.Grid;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;

namespace SSMScripter.Integration
{
    class Editor
    {
        private TextDocument _document;

        public Editor(TextDocument document)
        {
            _document = document;
        }


        public EditedLine GetEditedLine()
        {
            TextSelection textSelection = _document.Selection;

            VirtualPoint point = textSelection.ActivePoint;
            EditPoint editPoint = point.CreateEditPoint();

            string line = editPoint.GetLines(point.Line, point.Line + 1);
            int caret = point.LineCharOffset - 1;

            return new EditedLine(line, caret);
        }


        public void SetContent(string content)
        {
            EditPoint start = _document.CreateEditPoint(_document.StartPoint);
            start.Delete(_document.EndPoint);
            start.Insert(content);
        }


        public ResultGrid GetFocusedResultGrid()
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
    }
}
