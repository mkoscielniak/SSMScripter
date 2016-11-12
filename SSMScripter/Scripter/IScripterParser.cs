namespace SSMScripter.Scripter
{
    public interface IScripterParser
    {
        bool TryParse(ScripterParserInput input, out ScripterParserResult result);
    }

    public class ScripterParserInput
    {
        public string ContentLine { get; set; }
        public int Index { get; set; }
    }

    public class ScripterParserResult
    {
        public string Text { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
    }
}
