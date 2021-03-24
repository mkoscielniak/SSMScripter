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
        [InlineData("[DB].[SCH].[OBJ]", 8, "DB", "SCH", "OBJ")]
        [InlineData("[SCH].[OBJ]", 5, (string)null, "SCH", "OBJ")]
        [InlineData("[OBJ]", 2, (string)null, (string)null, "OBJ")]
        [InlineData("[OBJ]", 4, (string)null, (string)null, "OBJ")]
        [InlineData("[OBJ]  ", 5, (string)null, (string)null, "OBJ")]
        [InlineData("O", 0, (string)null, (string)null, "O")]
        public void TryParse_return_correct_data(string input, int index, string db, string sch, string name)
        {
            ScripterParserInput parserInput = new ScripterParserInput(input, index);
            ScripterParser parser = new ScripterParser();

            ScripterParserResult result = null;
            bool res = parser.TryParse(parserInput, out result);

            Assert.True(res);
            Assert.Equal(db, result.Database);
            Assert.Equal(sch, result.Schema);
            Assert.Equal(name, result.Name);
        }

        [Theory]
        [InlineData("]", 0)]
        [InlineData("[]", 1)]
        [InlineData("DB.[].OBJ", 1)]
        [InlineData("OBJ  ", 4)]
        [InlineData(" ", 0)]
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
