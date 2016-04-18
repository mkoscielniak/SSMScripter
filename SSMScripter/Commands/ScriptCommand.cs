using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.CommandBars;
using SSMScripter.Integration;
using SSMScripter.Properties;
using SSMScripter.Scripter;

namespace SSMScripter.Commands
{
    class ScriptCommand : ICommand
    {
        private readonly Context _context;
        private readonly IScripterParser _parser;
        private readonly IScripter _scripter;

        public ScriptCommand(Context context, IScripterParser parser, IScripter scripter)
        {
            Name = GetType().Name;
            _context = context;
            _parser = parser;
            _scripter = scripter;
        }


        public string Name { get; protected set; }


        public void Bind()
        {
            var commandBars = (CommandBars)_context.Application.CommandBars;
            var commands = (Commands2)_context.Application.Commands;
            object[] guids = null;

            Command command = commands.AddNamedCommand2(
                _context.AddIn,
                Name,
                "Script...",
                "Script...",
                false,
                Resources.ScriptCommandIcon,
                ref guids,
                (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText,
                vsCommandControlType.vsCommandControlTypeButton);

            command.Bindings = new object[] { "SQL Query Editor::F12" };

            CommandBar resultGridCommandBar = commandBars["SQL Results Grid Tab Context"];
            command.AddControl(resultGridCommandBar);

            CommandBar sqlEditorCommandBar = commandBars["SQL Files Editor Context"];
            command.AddControl(sqlEditorCommandBar);
        }

        

        public string Execute()
        {
            Editor editor = _context.GetCurrentEditor();

            ScripterParserInput parserInput = null;
            ScripterParserResult parserResult = null;

            if (!TryGetParserInput(editor, out parserInput))
                return "Cannot find any input";

            if (!_parser.TryParse(parserInput, out parserResult))
                return parserResult.Error;

            var scripterInput = new ScripterInput()
            {
                Schema = parserResult.Schema,
                Name = parserResult.Name,
            };

            ScripterResult scripterResult = null;

            if (!_scripter.TryScript(scripterInput, out scripterResult))
                return scripterResult.Error;

            Editor newEditor = _context.CreateNewCurrentEditor();
            newEditor.SetContent(scripterResult.Text);

            return "Success";
        }


        private bool TryGetParserInput(Editor editor, out ScripterParserInput input)
        {
            input = null;

            string value = String.Empty;
            int caret = 0;

            ResultGrid grid = editor.GetFocusedResultGrid();

            if (grid != null)
            {
                value = grid.GetSelectedValue();
                caret = 0;
            }
            else
            {
                EditedLine line = editor.GetEditedLine();
                value = line.Line;
                caret = line.CaretPos;
            }

            input = new ScripterParserInput()
            {
                ContentLine = value, Index = caret
            };

            return true;
        }
    }
}
