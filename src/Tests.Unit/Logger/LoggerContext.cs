using Serilog;

namespace SocialMediaLists.Tests.Logger
{
    internal class LoggerContext
    {
        public ILogger Logger { get; private set; }

        public LoggerContext()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}