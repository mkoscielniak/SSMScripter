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
            Editor editor = _context.GetCurrentEditor();
            EditedLine line = editor.GetEditedLine();
            
            ScripterParserInput parserInput = new ScripterParserInput
            {
                ContentLine = line.Line,
                Index = line.CaretPos
            };

            ScripterParserResult parserResult = null;
            
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
    }
}
