namespace Coding_Tracker.Modules
{
    internal class AppLogic
    {
        public static TimeSpan CalculateDuration(DateTimeOffset start, DateTimeOffset end)
        {
            return end - start;
        }
    }
}
