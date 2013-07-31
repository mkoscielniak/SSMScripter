using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace SSMScripter
{
    class Context
    {
        public DTE2 Application { get; private set; }        
        public AddIn AddIn { get; private set; }

        public Context(DTE2 app, AddIn addin)
        {
            Application = app;
            AddIn = addin;
        }
    }
}
