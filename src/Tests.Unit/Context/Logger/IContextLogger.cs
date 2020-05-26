using Serilog;

namespace SocialMediaLists.Tests.Unit.Context.Logger
{
    internal class IContextLogger
    {
        public ILogger Logger { get; private set; }

        public IContextLogger()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}