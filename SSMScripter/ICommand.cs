using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter
{
    interface ICommand
    {
        string Name { get; }
        void Bind();
        string Execute();
    }
}
