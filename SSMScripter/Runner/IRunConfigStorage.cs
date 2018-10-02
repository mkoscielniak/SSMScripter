using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public interface IRunConfigStorage
    {
        RunConfig Load();
        void Save(RunConfig cfg);
    }
}
