namespace Coding_Tracker.Infrastructure
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        //REMOVED FOR BEING A COMPOSITVE VALUE
        //public TimeSpan Duration { get; set; } 
    }
}