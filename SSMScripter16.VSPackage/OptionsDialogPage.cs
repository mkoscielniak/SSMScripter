using Microsoft.VisualStudio.Shell;
using SSMScripter.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSMScripter16.VSPackage
{
    public class OptionsDialogPage : DialogPage
    {
        protected override IWin32Window Window
        {
            get
            {
                OptionsControl control = new OptionsControl();
                return control;
            }
        }
    }
}
