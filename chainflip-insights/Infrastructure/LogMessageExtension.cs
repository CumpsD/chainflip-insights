namespace ChainflipInsights.Infrastructure
{
    using Discord;
    using Microsoft.Extensions.Logging;

    public static class LogMessageExtension
    {
        public static void Log(
            this LogMessage message,
            ILogger logger)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    logger.LogCritical(message.Exception, message.Message);
                    break;
                    
                case LogSeverity.Error:
                    logger.LogError(message.Exception, message.Message);
                    break;
                    
                case LogSeverity.Warning:
                    logger.LogWarning(message.Exception, message.Message);
                    break;
                    
                case LogSeverity.Info:
                    logger.LogInformation(message.Exception, message.Message);
                    break;
                    
                case LogSeverity.Verbose:
                    logger.LogTrace(message.Exception, message.Message);
                    break;
                    
                case LogSeverity.Debug:
                    logger.LogDebug(message.Exception, message.Message);
                    break;
                    
                default:
                    logger.LogDebug(message.Exception, message.Message);
                    break;
            }
        }
    }
}