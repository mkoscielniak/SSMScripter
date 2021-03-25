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
using Microsoft.Win32;
using SSMScripter.Runner;

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
            IWindowsUser windowsUser = new WindowsUser();

            string registryMasterKey = Registry.CurrentUser.Name + "\\Software\\SSMScripter";

            IScripterParser scripterParser = new ScripterParser();
            IScripter scripter = new SmoScripter();
            IScripterConfigStorage scripterConfigStorage = new ScripterConfigRegistryStorage(registryMasterKey);
            ScriptAction scriptAction = new ScriptAction(hostCtx, scripter, scripterParser, scripterConfigStorage);

            yield return new ScriptCommand(_app, _addin, scriptAction);

            IRunConfigStorage runConfigStorage = new RunConfigRegistryStorage(registryMasterKey);
            IRunContextProvider runContextProvider = new RunContextProvider(hostCtx, runConfigStorage);
            IRunParamsProcessor runParamsProcessor = new RunParamsProcessor(windowsUser);
            IRunProcessStarter runProcessStarter = new RunProcessStarter();
            RunAction runAction = new RunAction(runContextProvider, runParamsProcessor, runProcessStarter);


            yield return new RunCommand(_app, _addin, runAction);
        }


        private void BindCommands()
        {
            foreach (ICommand command in _commands.Values)
                command.Bind();
        }


        private void UnbindCommands()
        {
            var unbindable = _commands.Keys.Concat(new string[]
            {
                "SSMScripter.Connect.SSMScripter"
            });

            foreach (string names in unbindable)
            {
                try
                {
                    Command bindedCommand = _app.Commands.Item(names, -1);
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
                    SetStatusBarText("Working");
                    string result = null;

                    try
                    {
                        result = command.Execute() ?? "No result";
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }

                    SetStatusBarText(result);
                    handled = true;
                }
            }
        }


        private void SetStatusBarText(string status)
        {
            status = String.IsNullOrEmpty(status) ? String.Empty : String.Format("SSMScripter: {0}", status);
            _app.StatusBar.Text = status;
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
