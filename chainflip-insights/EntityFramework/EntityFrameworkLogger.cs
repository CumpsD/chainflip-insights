namespace ChainflipInsights.EntityFramework
{
    using Microsoft.Extensions.Logging;

    public class EntityFrameworkLogger
    {
        private static readonly object LoggerLock = new();

        public static ILoggerFactory? LoggerFactory { get; set; }

        public EntityFrameworkLogger(
            ILoggerFactory loggerFactory)
        {
            lock (LoggerLock)
                LoggerFactory ??= loggerFactory;
        }
    }
}