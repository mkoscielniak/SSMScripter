using FakeItEasy;
using SSMScripter.Runner;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SSMScripter.Tests.Runner
{
    public class RunParamsProcessorTests
    {
        [Fact]
        public void Compose_when_user_password()
        {
            string input = "$(Server) $(Database) $(User) $(Password)";
            string expected = "127.0.0.1 MYDATABASE user1 user1pass";

            SqlConnectionStringBuilder connstr = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "MYDATABASE",
                UserID = "user1",
                Password = "user1pass",
                IntegratedSecurity = false
            };

            IWindowsUser windowsUser = A.Fake<IWindowsUser>();
            RunParamsProcessor processor = new RunParamsProcessor(windowsUser);

            string result = processor.Compose(input, connstr);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compose_when_integrated_security()
        {
            string input = "$(Server) $(Database) $(User) $(Password)";
            string expected = "127.0.0.1 MYDATABASE user1 ";

            SqlConnectionStringBuilder connstr = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "MYDATABASE",
                IntegratedSecurity = true,
            };

            IWindowsUser windowsUser = A.Fake<IWindowsUser>();
            A.CallTo(() => windowsUser.Name).Returns("user1");

            RunParamsProcessor processor = new RunParamsProcessor(windowsUser);

            string result = processor.Compose(input, connstr);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compose_when_unknown_token()
        {
            string input = "$(Server) $(Database) $(User) $(UnknownToken123) $(Password)";
            string expected = "127.0.0.1 MYDATABASE user1 $(UnknownToken123) user1pass";

            SqlConnectionStringBuilder connstr = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "MYDATABASE",
                UserID = "user1",
                Password = "user1pass",
                IntegratedSecurity = false
            };

            IWindowsUser windowsUser = A.Fake<IWindowsUser>();
            RunParamsProcessor processor = new RunParamsProcessor(windowsUser);

            string result = processor.Compose(input, connstr);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compose_with_connection_string()
        {
            string input = "$(ConnectionString)";
            string expected = "Data Source=127.0.0.1;Initial Catalog=MYDATABASE;Integrated Security=False;User ID=user1;Password=user1pass";

            SqlConnectionStringBuilder connstr = new SqlConnectionStringBuilder()
            {
                DataSource = "127.0.0.1",
                InitialCatalog = "MYDATABASE",
                UserID = "user1",
                Password = "user1pass",
                IntegratedSecurity = false
            };

            IWindowsUser windowsUser = A.Fake<IWindowsUser>();
            RunParamsProcessor processor = new RunParamsProcessor(windowsUser);

            string result = processor.Compose(input, connstr);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compose_escapes_quotation_mark_in_values()
        {
            string input = "$(ConnectionString)";
            string expected = @"Password=\""user1 pass\""";

            SqlConnectionStringBuilder connstr = new SqlConnectionStringBuilder()
            {
                Password = "user1 pass",
            };

            IWindowsUser windowsUser = A.Fake<IWindowsUser>();
            RunParamsProcessor processor = new RunParamsProcessor(windowsUser);

            string result = processor.Compose(input, connstr);

            Assert.Equal(expected, result);
        }
    }
}
