using SSMScripter.Scripter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SSMScripter.Tests.Scripter
{
    public class ScripterParserInputTests
    {
        [Theory]
        [InlineData("ABC", 0)]
        [InlineData("ABC", 1)]
        [InlineData("ABC", 2)]
        public void Ctor_creates_object(string input, int index)
        {
            ScripterParserInput parserInput = new ScripterParserInput(input, index);

            Assert.Equal(input, parserInput.Content);
            Assert.Equal(index, parserInput.Index);
        }

        [Theory]
        [InlineData((string)null, 0, typeof(ArgumentException))]
        [InlineData("", 0, typeof(ArgumentException))]
        [InlineData("ABC", -1, typeof(IndexOutOfRangeException))]
        [InlineData("ABC", 3, typeof(IndexOutOfRangeException))]
        
        public void Ctor_throws_on_incorrect_args(string input, int index, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => new ScripterParserInput(input, index));
        }
    }
}
