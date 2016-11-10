using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSMScripter.Scripter
{
    interface IScripterFactory
    {
        IScripter CreateScripter(IDbConnection connection);
    }
}
