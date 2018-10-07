using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Scripter
{
    public interface IScripterConfigStorage
    {
        ScripterConfig Load();
        void Save(ScripterConfig config);
    }
}
