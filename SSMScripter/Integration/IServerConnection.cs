using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Integration
{
    public interface IServerConnection : IDisposable
    {
        void Connect();
        void Disconnect();
        IDataReader ExecuteReader(string query);
        object Connection { get; }
    }
}
