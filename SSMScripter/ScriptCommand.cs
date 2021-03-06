﻿using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using Microsoft.VisualStudio.CommandBars;
using SSMScripter.Integration;
using SSMScripter.Properties;
using SSMScripter.Scripter;

namespace SSMScripter
{
    class ScriptCommand : ICommand
    {
        private readonly DTE2 _app;
        private readonly AddIn _addin;
        private readonly ScriptAction _scriptAction;
        

        public ScriptCommand(DTE2 app, AddIn addin, ScriptAction scriptAction)
        {
            Name = "SSMScripterScript";
            _app = app;
            _addin = addin;
            _scriptAction = scriptAction;
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
            return _scriptAction.Execute();
        }        
    }
}
