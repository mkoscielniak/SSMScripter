using SSMScripter.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Integration
{
    public class WindowsUser : IWindowsUser
    {
        private WindowsIdentity _identity;

        public WindowsUser()
        {
            _identity = WindowsIdentity.GetCurrent();
        }

        public string Name => _identity.Name ?? String.Empty;
    }
}
