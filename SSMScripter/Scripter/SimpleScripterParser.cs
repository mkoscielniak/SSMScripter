using System;
using System.Linq;

namespace SSMScripter.Scripter
{
    public class SimpleScripterParser : IScripterParser
    {        
        private readonly char[] _additionalChars = new char[] { '[', ']', '.', '_' };               


        public bool TryParse(ScripterParserInput input, out ScripterParserResult result)
        {
            result = new ScripterParserResult();
            
            string content = input.ContentLine;
            int index = input.Index;
            
            Func<string, ScripterParserResult, bool> error = (msg, res) =>
            {
                res.Error = msg;
                return false;
            };            

            if(String.IsNullOrEmpty(content))
                return error("Empty or null content", result);
                                    
            if (index >= content.Length)
                index = content.Length - 1;

            int first = FindFirstAcceptableCharPos(content, index);
            int last = FindLastAcceptableCharPos(content, index);

            if (first == last)
                return error("Nothing found", result);

            string text = content.Substring(first, last - first + 1);
            string schema = null;
            string name = null;

            int dotPos = text.IndexOf('.');

            if (dotPos >= 0)
            {
                schema = text.Substring(0, dotPos).Trim('[',']');
                name = text.Substring(dotPos + 1).Trim('[', ']');
            }
            else
            {
                name = text.Trim('[',']');
            }

            result.Text = text;
            result.Schema = schema;
            result.Name = name;

            return true;
        }


        private bool IsAcceptableChar(char c)
        {
            return Char.IsLetterOrDigit(c) || _additionalChars.Contains(c);
        }


        private int FindLastAcceptableCharPos(string input, int index)
        {
            int pos = index;

            for (int i = index; i < input.Length; i++)
            {
                if (IsAcceptableChar(input[i]))
                    pos = i;
                else
                    break;
            }

            return pos;
        }


        private int FindFirstAcceptableCharPos(string input, int index)
        {
            int pos = index;

            for (int i = index; i >= 0; i--)
            {
                if (IsAcceptableChar(input[i]))
                    pos = i;
                else
                    break;
            }

            return pos;
        }
    }
}
