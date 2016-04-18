using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter.Integration
{
    class EditedLine
    {
        public string Line { get; protected set; }
        public int CaretPos { get; protected set; }

        public EditedLine(string line, int caretPos)
        {
            Line = line;
            CaretPos = caretPos;
        }
    }
}
