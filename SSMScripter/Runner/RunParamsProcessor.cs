using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSMScripter.Runner
{
    public class RunParamsProcessor : IRunParamsProcessor
    {
        private IWindowsUser _windowsUser;

        private Regex _tokenRegex = new Regex(@"\$\((?<tok>([^\)]+))\)", RegexOptions.Compiled);


        public RunParamsProcessor(IWindowsUser windowsUser)
        {
            _windowsUser = windowsUser;
        }


        public string Compose(string input, SqlConnectionStringBuilder connectionStr)
        {
            Dictionary<string, string> tokens = new Dictionary<string, string>();
            FetchTokens(tokens, connectionStr);

            string result = ReplaceTokens(input, tokens);

            return result;
        }


        private string ReplaceTokens(string input, Dictionary<string, string> tokens)
        {
            MatchEvaluator evaluator = m =>
            {
                string cap = m.Captures[0].Value;
                string tok = m.Groups["tok"].Value;
                string val = null;

                if (tokens.TryGetValue(tok, out val))
                {
                    val = val.Replace(@"""", @"\""");
                    return val;
                }

                return cap;
            };

            return _tokenRegex.Replace(input, evaluator);
        }


        private void FetchTokens(Dictionary<string, string> paramsCtx, SqlConnectionStringBuilder connstr)
        {
            string user = connstr.UserID ?? String.Empty;
            if (connstr.IntegratedSecurity)
                user = _windowsUser.Name;

            string password = connstr.Password ?? String.Empty;

            paramsCtx["Server"] = connstr.DataSource ?? String.Empty;
            paramsCtx["Database"] = connstr.InitialCatalog ?? String.Empty;
            paramsCtx["User"] = user;
            paramsCtx["Password"] = password;
            paramsCtx["ConnectionString"] = connstr.ConnectionString;
        }
    }
}
