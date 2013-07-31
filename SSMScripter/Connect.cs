using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using SSMScripter.Commands;
using SSMScripter.Commands.Scripter;

namespace SSMScripter
{
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private Context _context;
        private Dictionary<string, ICommand> _commands;


        public Connect()
        {
        }


        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _context = new Context((DTE2)application, (AddIn)addInInst);
            _commands = CreateCommands().ToDictionary(cmd => String.Format("{0}.{1}", _context.AddIn.ProgID, cmd.Name));

            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                UnbindCommands();
                BindCommands();
            }
        }


        private IEnumerable<ICommand> CreateCommands()
        {
            yield return new ScriptCommand(_context);
        }


        private void BindCommands()
        {
            foreach (ICommand command in _commands.Values)
                command.Bind();
        }


        private void UnbindCommands()
        {
            Command bindedCommand = null;            

            foreach (string commandName in _commands.Keys)
            {
                try
                {
                    bindedCommand = _context.Application.Commands.Item(commandName, -1);
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
                    _context.Application.StatusBar.Text = command.Execute() ?? "None";
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


        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            if (RemoveMode == ext_DisconnectMode.ext_dm_HostShutdown)
                UnbindCommands();
        }


        public void OnStartupComplete(ref Array custom)
        {
        }
    }
}
