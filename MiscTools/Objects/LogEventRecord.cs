using System;
namespace MiscTools.Objects
{
    public class LogEventRecord
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public LogEvent Log { get; set; }
    }
}
