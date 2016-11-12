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
using Microsoft.Win32;

namespace SSMScripter16.VSPackage
{    
    internal sealed class ScriptCommand
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("863ad083-bf64-40ce-9b33-faf58aa24dec");

        private readonly Package _package;

        private readonly ScriptAction _scriptAction;


        private ScriptCommand(Package package)
        {
            if (package == null)        
                throw new ArgumentNullException("package");

            _package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }

            DTE2 dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            IHostContext hostCtx = new HostContext(dte);
            IScripterParser parser = new SimpleScripterParser();
            IScripter scripter = new SmoScripter();
            _scriptAction = new ScriptAction(hostCtx, scripter, parser);            
        }


        public static ScriptCommand Instance
        {
            get;
            private set;
        }

        
        private IServiceProvider ServiceProvider {  get { return _package; } }        

        
        public static void Initialize(Package package)
        {
            Instance = new ScriptCommand(package);
        }


        private void EnsurePluginReady()
        {
            string packKeyPath = String.Format(@"Packages\{{{0}}}", ScriptCommandPackage.PackageGuidString);
            using (RegistryKey key = VSRegistry.RegistryRoot(__VsLocalRegistryType.RegType_UserSettings, true))
            using (RegistryKey packKey = key.CreateSubKey(packKeyPath))
                packKey.SetValue("SkipLoading", 1, RegistryValueKind.DWord);
        }

        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            string result = String.Empty;

            try
            {
                EnsurePluginReady();
                result = _scriptAction.Execute();
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }

            SetStatusBarText(result);            
        }


        private void SetStatusBarText(string status)
        {
            if(!String.IsNullOrEmpty(status))
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
