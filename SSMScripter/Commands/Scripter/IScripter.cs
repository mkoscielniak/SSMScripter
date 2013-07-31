using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter.Commands.Scripter
{
    interface IScripter
    {
        bool TryScript(ScripterInput input, out ScripterResult result);
    }

    
    public class ScripterInput
    {
        public string Schema { get; set; }
        public string Name { get; set; }
    }


    public class ScripterResult
    {
        public string Text { get; set; }
        public string Error { get; set; }
    }

}
