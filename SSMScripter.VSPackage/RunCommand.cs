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
using SSMScripter.Runner;
using Microsoft.Win32;

namespace SSMScripter.VSPackage
{    
    internal sealed class RunCommand
    {
        public const int CommandId = 0x0200;

        public static readonly Guid CommandSet = new Guid("59dfb010-0dd3-4b6f-9a0b-ec01c64e270e");
     
        private readonly Package _package;

        private readonly RunAction _runAction;


        private RunCommand(Package package)
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
            _runAction = new RunAction(hostCtx);
        }


        public static RunCommand Instance
        {
            get;
            private set;
        }

        
        private IServiceProvider ServiceProvider {  get { return _package; } }        

        
        public static void Initialize(Package package)
        {
            Instance = new RunCommand(package);
        }
        
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            string result = String.Empty;

            try
            {                
                result = _runAction.Execute();
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
                status = String.Format("SSMScripter run: {0}", status);

            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));

            int frozen;
            statusBar.IsFrozen(out frozen);

            if (frozen != 0)
                statusBar.FreezeOutput(0);

            statusBar.SetText(status);
        }
    }
}
