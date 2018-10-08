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
    public class RunActionTest
    {
        [Fact]
        public void Execute_calls_new_process()
        {
            string expectedTool = "Tool1";
            string inputArgs = "Arg1 Arg2";
            string expectedArgs = "ABC DEF";

            var runctx = new RunContext()
            {
                Config = new RunConfig()
                {
                    RunTool = expectedTool,
                    RunArgs = inputArgs,
                },
                ConnectionString = new SqlConnectionStringBuilder()
                {
                    DataSource = "127.0.0.1",
                    InitialCatalog = "MYDATABASE",
                    UserID = "user1",
                    Password = "user1pass",
                    IntegratedSecurity = false
                },
            };

            IRunContextProvider contextProvider = A.Fake<IRunContextProvider>();
            A.CallTo(() => contextProvider.Get()).Returns(runctx);            

            IRunParamsProcessor paramsProcessor = A.Fake<IRunParamsProcessor>();
            A.CallTo(() => paramsProcessor.Compose(inputArgs, runctx.ConnectionString)).Returns(expectedArgs);

            IRunProcessStarter processStarter = A.Fake<IRunProcessStarter>();

            RunAction runAction = new RunAction(contextProvider, paramsProcessor, processStarter);
            runAction.Execute();

            A.CallTo(() => processStarter.Start(expectedTool, expectedArgs)).MustHaveHappenedOnceExactly();
        }
    }
}
