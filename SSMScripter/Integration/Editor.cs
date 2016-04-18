using System.Reflection;
using EnvDTE;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;

namespace SSMScripter.Integration
{
    class Editor
    {
        private readonly TextDocument _document;        


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
    }
}
