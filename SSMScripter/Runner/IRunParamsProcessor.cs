using System.Data.SqlClient;

namespace SSMScripter.Runner
{
    public interface IRunParamsProcessor
    {
        string Compose(string input, SqlConnectionStringBuilder connectionString);
    }
}