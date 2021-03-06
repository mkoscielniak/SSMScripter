﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter.Integration
{
    public class EditedLine
    {
        public string Line { get; protected set; }
        public int CaretPos { get; protected set; }

        public EditedLine(string line, int caretPos)
        {
            Line = line;
            CaretPos = caretPos;
        }

        public int Length { get { return Line.Length; } }
    }
}
