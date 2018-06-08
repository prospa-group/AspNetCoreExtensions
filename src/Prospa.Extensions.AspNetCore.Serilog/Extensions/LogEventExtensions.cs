// ReSharper disable CheckNamespace
namespace Serilog.Events
    // ReSharper restore CheckNamespace
{
    public static class LogEventExtensions
    {
        public static void AddPropertyIfAbsentAndNotNull(this LogEvent logEvent, LogEventProperty logEventProperty)
        {
            if (logEventProperty != null)
            {
                logEvent.AddPropertyIfAbsent(logEventProperty);
            }
        }
    }
}
