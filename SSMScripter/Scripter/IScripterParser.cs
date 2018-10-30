using System;

namespace SSMScripter.Scripter
{
    public interface IScripterParser
    {
        bool TryParse(ScripterParserInput input, out ScripterParserResult result);
    }

    public class ScripterParserInput
    {
        public ScripterParserInput(string content, int index)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content cannot be null or empty", "content");
            if (index < 0 || index >= content.Length)
                throw new IndexOutOfRangeException("Index is out of range in givent content");

            Content = content;
            Index = index;
        }

        public string Content { get; protected set; }
        public int Index { get; protected set; }
    }

    public class ScripterParserResult
    {
        public string Text { get; set; }
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
    }
}
