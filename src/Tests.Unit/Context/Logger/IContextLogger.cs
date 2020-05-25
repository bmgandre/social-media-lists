using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace SocialMediaLists.Tests.Unit.Context.Logger
{
    public class IContextLogger
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