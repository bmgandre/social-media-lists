using SocialMediaLists.Tests.Context.Logger;
using System;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Hooks
{
    [Binding]
    internal class ExceptionHook
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IContextLogger _contextLogger;

        public ExceptionHook(ScenarioContext context,
            IContextLogger contextLogger)
        {
            _scenarioContext = context;
            _contextLogger = contextLogger;
        }

        [AfterScenario]
        public void LogException()
        {
            if (_scenarioContext.ContainsKey("Exception"))
            {
                _contextLogger.Logger.Error(_scenarioContext["Exception"] as Exception, _scenarioContext.ScenarioInfo.Title);
            }
        }
    }
}