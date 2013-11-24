using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.CommandBars;
using SSMScripter.Commands;
using SSMScripter.Properties;

namespace SSMScripter.Commands.Scripter
{
    class ScriptCommand : ICommand
    {
        private Context _context;
        private IScripterParser _parser;
        private IScripter _scripter;

        public ScriptCommand(Context context)
        {
            Name = GetType().Name;
            _context = context;
            _parser = new SimpleScripterParser();
            _scripter = new SmoScripter();
        }


        public string Name { get; protected set; }


        public void Bind()
        {
            var commandBars = (CommandBars)_context.Application.CommandBars;
            CommandBar commandBar = commandBars["SQL Files Editor Context"];
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
            
            command.Bindings = new object[] { "Global::F12" };

            command.AddControl(commandBar);
        }


        public string Execute()
        {
            ScripterParserInput parserInput = null;
            ScripterParserResult parserResult = null;

            if (!TryGetParserInput(out parserInput))
                return "Cannot find any input";

            if (!_parser.TryParse(parserInput, out parserResult))
                return parserResult.Error;

            ScripterInput scripterInput = new ScripterInput()
            {
                Schema = parserResult.Schema,
                Name = parserResult.Name
            };

            ScripterResult scripterResult = null;
            string status = String.Empty;

            if (!_scripter.TryScript(scripterInput, out scripterResult))
                return scripterResult.Error;

            DisplayResult(scripterResult.Text);

            return "Success";
        }


        private void DisplayResult(string content)
        {
            ServiceCache.ScriptFactory.CreateNewBlankScript(ScriptType.Sql);
            TextDocument document = (TextDocument)((DTE2)ServiceCache.ExtensibilityModel).ActiveDocument.Object(String.Empty);
            document.EndPoint.CreateEditPoint().Insert(content);
        }


        private bool TryGetParserInput(out ScripterParserInput input)
        {
            input = new ScripterParserInput() { ContentLine = String.Empty, Index = 0 };

            Document document = _context.Application.ActiveDocument;
            if (document == null)
                return false;

            TextSelection textSelection = (TextSelection)_context.Application.ActiveDocument.Selection;

            VirtualPoint point = textSelection.ActivePoint;
            EditPoint editPoint = point.CreateEditPoint();

            input.ContentLine = editPoint.GetLines(point.Line, point.Line + 1);
            input.Index = point.LineCharOffset - 1;

            return true;
        }
    }
}
