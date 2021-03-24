using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMScripter.Scripter
{
    public class ScripterParser : IScripterParser
    {
        private readonly char[] _partsChars = new char[] { '.' };
        private readonly char[] _partsTrim = new char[] { '[', ']' };
        private readonly char[] _additionalChars = new char[] { '[', ']', '.', '_' };

        public bool TryParse(ScripterParserInput input, out ScripterParserResult result)
        {
            result = new ScripterParserResult();

            string content = input.Content;
            int index = input.Index;

            Func<string, ScripterParserResult, bool> error = (msg, res) =>
            {
                res.Error = msg;
                return false;
            };

            if (String.IsNullOrEmpty(content))
                return error("Empty or null content", result);

            int first = FindFirstAcceptableCharPos(content, index);
            int last = FindLastAcceptableCharPos(content, index);

            if (first == -1 && last == -1 && content[index] == ' ')
            {
                first = FindFirstAcceptableCharPos(content, index - 1);
                last = FindLastAcceptableCharPos(content, index - 1);
            }

            if (first == -1 && last == -1)
                return error("Nothing found", result);

            string text = content.Substring(first, last - first + 1);
            result.Text = text;

            string[] parts = text.Split(_partsChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim(_partsTrim);
                if (String.IsNullOrEmpty(parts[i]))
                    return error("Unknown content format", result);
            }

            switch (parts.Length)
            {
                case 0:
                    return error("Empty or null content", result);
                case 1:
                    result.Name = parts[0];
                    return true;
                case 2:
                    result.Schema = parts[0];
                    result.Name = parts[1];
                    return true;
                case 3:
                    result.Database = parts[0];
                    result.Schema = parts[1];
                    result.Name = parts[2];
                    return true;
                default:
                    return error("Unknown content format", result);
            }
        }


        private bool IsAcceptableChar(char c)
        {
            return Char.IsLetterOrDigit(c) || _additionalChars.Contains(c);
        }


        private int FindLastAcceptableCharPos(string input, int index)
        {
            int pos = -1;

            if (index < 0 || index >= input.Length)
                return pos;

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
            int pos = -1;

            if (index < 0 || index >= input.Length)
                return pos;

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
