using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.CommandBars;
using SSMScripter.Integration;
using SSMScripter.Properties;
using SSMScripter.Runner;
using SSMScripter.Scripter;

namespace SSMScripter
{
    class RunCommand : ICommand
    {
        private readonly DTE2 _app;
        private readonly AddIn _addin;
        private readonly RunAction _runAction;
        

        public RunCommand(DTE2 app, AddIn addin, RunAction runAction)
        {
            Name = "SSMScripterRun";
            _app = app;
            _addin = addin;
            _runAction = runAction;
        }


        public string Name { get; protected set; }


        public void Bind()
        {
            var commandBars = (CommandBars)_app.CommandBars;
            var commands = (Commands2)_app.Commands;
            object[] guids = null;

            Command command = commands.AddNamedCommand2(
                _addin,
                Name,
                "Run tool",
                "Run tool",
                false,
                Resources.ScriptCommandIcon,
                ref guids,
                (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText,
                vsCommandControlType.vsCommandControlTypeButton);

            command.Bindings = new object[] { "Global::Ctrl+F12" };

            //CommandBar resultGridCommandBar = commandBars["SQL Results Grid Tab Context"];
            //command.AddControl(resultGridCommandBar);

            //CommandBar sqlEditorCommandBar = commandBars["SQL Files Editor Context"];
            //command.AddControl(sqlEditorCommandBar);
        }

        
        public string Execute()
        {
            return _runAction.Execute();
        }        
    }
}
