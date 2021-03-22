using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Integration.DTE
{
    class HostDbConnection : IHostDbConnection
    {
        private IDbConnection _dbconn;
        private SqlConnectionInfo _conninf;

        public HostDbConnection(SqlConnectionInfo connectionInfo)
        {
            _dbconn = connectionInfo.CreateConnectionObject();
            _conninf = connectionInfo;
        }

        public IDbConnection Connection => _dbconn;

        public object ConnectionInfo => _conninf;

        public void Open()
        {
            _dbconn.Open();
        }

        public void Close()
        {
            _dbconn.Close();
        }

        public void Dispose()
        {
            _dbconn?.Dispose();
        }
    }
}
