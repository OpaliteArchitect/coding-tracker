namespace Coding_Tracker.Infrastructure
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
    }
}