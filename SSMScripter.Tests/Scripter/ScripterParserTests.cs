using SSMScripter.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SSMScripter.Tests.Scripter
{
    public class ScripterParserTests
    {
        [Theory]
        [InlineData("[DB].[SCH].[OBJ]", "DB", "SCH", "OBJ")]
        [InlineData("[SCH].[OBJ]", (string)null, "SCH", "OBJ")]
        [InlineData("[OBJ]", (string)null, (string)null, "OBJ")]
        public void TryParse_return_correct_data(string input, string db, string sch, string name)
        {
            ScripterParserInput parserInput = new ScripterParserInput(input, input.Length / 2);            
            ScripterParser parser = new ScripterParser();

            ScripterParserResult result = null;
            bool res = parser.TryParse(parserInput, out result);

            Assert.True(res);
            Assert.Equal(db, result.Database);
            Assert.Equal(sch, result.Schema);
            Assert.Equal(name, result.Name);
        }        

        [Theory]
        [InlineData("]",0)]
        public void TryParse_error_on_input(string input, int index)
        {
            ScripterParserInput parserInput = new ScripterParserInput(input, index);
            ScripterParser parser = new ScripterParser();

            ScripterParserResult result = null;
            bool res = parser.TryParse(parserInput, out result);

            Assert.False(res);
        }
    }
}
