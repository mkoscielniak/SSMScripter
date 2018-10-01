//------------------------------------------------------------------------------
// <copyright file="ScriptCommandPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SSMScripter.VSPackage
{    
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(ScriptCommandPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(OptionsDialogPage), "SSMScripter", "General", 0, 0, true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class ScriptCommandPackage : Package
    {        
        public const string PackageGuidString = "b3d9e32b-4b3a-462b-a1bf-9e102f67ba11";

        public ScriptCommandPackage()
        {        
        }

        
        protected override void Initialize()
        {
            ScriptCommandSet.Initialize(this);            

            base.Initialize();

            Timer timer = new Timer();
            timer.Tick += (o, e) =>
            {
                timer.Stop();
                AddSkipLoadingEntry();
            };
            timer.Interval = 1000;
            timer.Start();
        }


        private void AddSkipLoadingEntry()
        {
            string packKeyPath = String.Format(@"Packages\{{{0}}}", ScriptCommandPackage.PackageGuidString);
            using (RegistryKey key = VSRegistry.RegistryRoot(__VsLocalRegistryType.RegType_UserSettings, true))
            using (RegistryKey packKey = key.CreateSubKey(packKeyPath))
                packKey.SetValue("SkipLoading", 1, RegistryValueKind.DWord);
        }
    }
}
