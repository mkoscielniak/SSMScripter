using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter.Commands.Scripter
{
    interface IScripterParser
    {
        bool TryParse(ScripterParserInput input, out ScripterParserResult result);
    }

    class ScripterParserInput
    {
        public string ContentLine { get; set; }
        public int Index { get; set; }
    }

    class ScripterParserResult
    {
        public string Text { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
    }
}
