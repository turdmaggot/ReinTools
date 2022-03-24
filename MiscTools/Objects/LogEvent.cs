namespace MiscTools.Objects
{
    public class LogEvent
    {
        public long Code { get; set; }
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }

        public LogEvent(long code, LogSeverity sev, string message)
        {
            Code = code;
            Severity = sev;
            Message = message;
        }
    }
}
