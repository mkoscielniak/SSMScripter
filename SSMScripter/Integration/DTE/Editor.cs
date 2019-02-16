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

namespace SSMScripter.Integration.DTE
{
    public class Editor : IEditor
    {
        private TextDocument _document;

        public Editor(TextDocument document)
        {
            _document = document;
        }


        public EditedLine GetEditedLine()
        {
            TextSelection textSelection = _document.Selection;
            if (!String.IsNullOrEmpty(textSelection.Text))
                return new EditedLine(textSelection.Text, 0);

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
    }
}
