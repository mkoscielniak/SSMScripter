using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using SSMScripter.Integration;
using SSMScripter.Scripter;
using SSMScripter.Scripter.Smo;
using SSMScripter.Integration.DTE;

namespace SSMScripter
{
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private DTE2 _app;
        private AddIn _addin;
        private Dictionary<string, ICommand> _commands;
        

        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _app = (DTE2)application;
            _addin = (AddIn)addInInst;
            
            _commands = CreateCommands()
                .ToDictionary(cmd => String.Format("{0}.{1}", _addin.ProgID, cmd.Name));

            if (connectMode == ext_ConnectMode.ext_cm_Startup)
            {
                UnbindCommands();
                BindCommands();
            }
        }


        private IEnumerable<ICommand> CreateCommands()
        {
            IHostContext hostCtx = new HostContext(_app);

            IScripterParser parser = new SimpleScripterParser();
            IScripter scripter = new SmoScripter();
            ScriptAction scriptAction = new ScriptAction(hostCtx, scripter, parser);
            yield return new ScriptCommand(_app, _addin, scriptAction);
        }


        private void BindCommands()
        {
            foreach (ICommand command in _commands.Values)
                command.Bind();
        }


        private void UnbindCommands()
        {
            foreach (string commandName in _commands.Keys)
            {
                try
                {
                    Command bindedCommand = _app.Commands.Item(commandName, -1);
                    bindedCommand.Delete();
                }
                catch (ArgumentException ae)
                {
                    //cannot find command
                }
            }            
        }


        public void Exec(string commandName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled)
        {
            handled = false;

            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                ICommand command = null;
                if (_commands.TryGetValue(commandName, out command))
                {
                    string result = command.Execute() ?? "None";
                    _app.StatusBar.Text = String.Format("{0}: {1}", command.Name, result);
                    handled = true;
                }
            }
        }


        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (_commands.ContainsKey(commandName))
                {
                    statusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }


        public void OnAddInsUpdate(ref Array custom)
        {
        }


        public void OnBeginShutdown(ref Array custom)
        {
        }


        public void OnDisconnection(ext_DisconnectMode removeMode, ref Array custom)
        {
            if (removeMode == ext_DisconnectMode.ext_dm_HostShutdown)
                UnbindCommands();
        }


        public void OnStartupComplete(ref Array custom)
        {
        }
    }
}
