using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Integration
{
    public class ConnectionInfoException : Exception
    {
        public ConnectionInfoException(string message) : base(message)
        {
        }
    }
}
