//------------------------------------------------------------------------------
// <copyright file="ScriptCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using SSMScripter.Integration;
using SSMScripter.Integration.DTE;
using EnvDTE;
using SSMScripter.Scripter;
using SSMScripter.Scripter.Smo;
using Microsoft.Win32;
using SSMScripter.Runner;

namespace SSMScripter.VSPackage
{
    internal sealed class ScriptCommandSet
    {
        public const int CommandScriptId = 0x0100;
        public const int CommandRunId = 0x0200;

        public static readonly Guid CommandSet = new Guid("863ad083-bf64-40ce-9b33-faf58aa24dec");

        private readonly Package _package;

        private readonly ScriptAction _scriptAction;
        private readonly RunAction _runAction;


        private ScriptCommandSet(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            _package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var scriptMenuCommandId = new CommandID(CommandSet, CommandScriptId);
                var scriptMenuCommand = new MenuCommand(this.MenuScriptCallback, scriptMenuCommandId);
                commandService.AddCommand(scriptMenuCommand);

                var runMenuCommandId = new CommandID(CommandSet, CommandRunId);
                var runMenuCommand = new MenuCommand(this.MenuRunCallback, runMenuCommandId);
                commandService.AddCommand(runMenuCommand);
            }

            DTE2 dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            IHostContext hostCtx = new HostContext(dte);
            IWindowsUser windowsUser = new WindowsUser();

            string registryMasterKey = Registry.CurrentUser.Name + "\\Software\\SSMScripter";

            IScripterParser scripterParser = new ScripterParser();
            IScripter scripter = new SmoScripter();
            IScripterConfigStorage scripterConfigStorage = new ScripterConfigRegistryStorage(registryMasterKey);
            _scriptAction = new ScriptAction(hostCtx, scripter, scripterParser, scripterConfigStorage);

            IRunConfigStorage runConfigStorage = new RunConfigRegistryStorage(registryMasterKey);
            IRunContextProvider runContextProvider = new RunContextProvider(hostCtx, runConfigStorage);
            IRunParamsProcessor runParamsProcessor = new RunParamsProcessor(windowsUser);
            IRunProcessStarter runProcessStarter = new RunProcessStarter();
            _runAction = new RunAction(runContextProvider, runParamsProcessor, runProcessStarter);
        }


        public static ScriptCommandSet Instance
        {
            get;
            private set;
        }


        private IServiceProvider ServiceProvider { get { return _package; } }


        public static void Initialize(Package package)
        {
            Instance = new ScriptCommandSet(package);
        }


        private void RunAction(Func<string> workAction)
        {
            string result = String.Empty;

            SetStatusBarText("Working");

            try
            {
                result = workAction();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            SetStatusBarText(result);
        }


        private void MenuScriptCallback(object sender, EventArgs e)
        {
            RunAction(_scriptAction.Execute);
        }


        private void MenuRunCallback(object sender, EventArgs e)
        {
            RunAction(_runAction.Execute);
        }


        private void SetStatusBarText(string status)
        {
            if (!String.IsNullOrEmpty(status))
                status = String.Format("SSMScripter: {0}", status);

            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));

            int frozen;
            statusBar.IsFrozen(out frozen);

            if (frozen != 0)
                statusBar.FreezeOutput(0);

            statusBar.SetText(status);
        }
    }
}
