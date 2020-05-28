using SocialMediaLists.Tests.Logger;
using System;
using TechTalk.SpecFlow;

namespace SocialMediaLists.Tests.Hooks
{
    [Binding]
    internal class ExceptionHook
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LoggerContext _contextLogger;

        public ExceptionHook(ScenarioContext context,
            LoggerContext contextLogger)
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