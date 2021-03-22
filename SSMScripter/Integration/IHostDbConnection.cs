using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Integration
{
    public interface IHostDbConnection : IDisposable
    {
        IDbConnection Connection { get; }
        object ConnectionInfo { get; }

        void Open();
        void Close();
    }
}
