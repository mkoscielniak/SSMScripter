using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMScripter.Integration
{
    public interface IEditor
    {
        EditedLine GetEditedLine();

        void SetContent(string content);        
    }
}
