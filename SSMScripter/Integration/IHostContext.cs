using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSMScripter.Integration
{
    public interface IHostContext
    {
        IEditor GetCurrentEditor();

        IEditor GetNewEditor();

        IResultGrid GetFocusedResultGrid();
        
        IServerConnection CloneCurrentConnection(string database);

        string GetCurrentConnectionString();
    }
}
