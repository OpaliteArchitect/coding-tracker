using Coding_Tracker.Infrastructure;
using System.Globalization;
using static Coding_Tracker.Modules.CrudCalls;

namespace Coding_Tracker.Modules
{
    public static class AppLogic
    {
        public static bool InSession { get; private set; } = false;
        public static DateTimeOffset SessionStarted { get; private set; }

        public static void StartSession()
        {
            InSession = true;
            SessionStarted = DateTimeOffset.Now;
        }

        public static async Task EndSession()
        {
            if (!InSession)
            {
                throw new InvalidOperationException("Should not be possible");
            }

            InSession = false;
            await AddSession(SessionStarted, DateTimeOffset.Now);
        }
        public static async Task<int> DeleteSession(IEnumerable<CodingSession> sessions)
        {
            var ids = sessions.Select(s => s.Id);
            return await CrudCalls.DeleteSession(ids);
        }

        public static bool ConvertToSession(int month, int day, int year, int hour, int minute, string meridiem, out DateTimeOffset formattedTime)
        {
            const string format = "MM/dd/yyyy hh:mm tt";

            string timeString = $"{month:D2}/{day:D2}/{year:D2} {hour:D2}:{minute:D2} {meridiem}";

            try
            {
                formattedTime = DateTimeOffset.ParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                return true;
            }
            catch (FormatException)
            {
                formattedTime = default;
                return false;
            }
        }

        public static bool VerifyDurationValidity(DateTimeOffset start, DateTimeOffset end)
        {
            return end > start;
        }
    }
}
