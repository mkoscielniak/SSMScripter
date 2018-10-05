using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public interface IRunContextProvider
    {
        RunContext Get();
    }
}
