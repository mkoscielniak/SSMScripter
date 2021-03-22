using SSMScripter.Integration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

namespace SSMScripter.Scripter
{
    public class ScriptAction
    {
        private IHostContext _hostCtx;
        private IScripter _scripter;
        private IScripterParser _parser;
        private IScripterConfigStorage _configStorage;

        public ScriptAction(IHostContext hostCtx, IScripter scripter, IScripterParser parser, IScripterConfigStorage configStorage)
        {
            if (hostCtx == null)
                throw new ArgumentNullException("hostCtx");
            if (scripter == null)
                throw new ArgumentNullException("scripter");
            if (parser == null)
                throw new ArgumentNullException("parser");

            _hostCtx = hostCtx;
            _scripter = scripter;
            _parser = parser;
            _configStorage = configStorage;
        }


        public string Execute()
        {
            ScripterParserInput parserInput = null;
            if (!TryGetParserInput(out parserInput))
                return "Cannot find any input";

            ScripterParserResult parserResult = null;
            if (!_parser.TryParse(parserInput, out parserResult))
                return parserResult.Error;

            ScripterConfig config = _configStorage.Load();

            var scripterInput = new ScripterInput()
            {
                Database = parserResult.Database,
                Schema = parserResult.Schema,
                Name = parserResult.Name,
                ScriptDatabaseContext = config.ScriptDatabaseContext,
            };

            string scriptResult = null;
            if (!TryScriptIntoEditor(scripterInput, out scriptResult))
                return scriptResult;
            
            return "Success";
        }


        private bool TryGetParserInput(out ScripterParserInput input)
        {
            input = null;

            IResultGrid grid = _hostCtx.GetFocusedResultGrid();
            if (grid != null)
                input = new ScripterParserInput(grid.GetSelectedValue(), 0);

            if (input == null)
            {
                IEditor editor = _hostCtx.GetCurrentEditor();
                if (editor != null)
                {
                    EditedLine line = editor.GetEditedLine();
                    input = new ScripterParserInput(line.Line, line.CaretPos);
                }
            }

            return input != null;
        }


        private bool TryScriptIntoEditor(ScripterInput input, out string result)
        {
            result = null;
            
            try
            {
                StringCollection content;

                using (IServerConnection serverConn = _hostCtx.CloneCurrentConnection(input.Database))
                {
                    serverConn.Connect();
                    content = _scripter.Script(serverConn, input);                    ;
                    serverConn.Disconnect();
                }

                IEditor editor = _hostCtx.GetNewEditor();
                editor.SetContent(content);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result == null;
        }
    }
}
