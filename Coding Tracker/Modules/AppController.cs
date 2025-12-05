using Coding_Tracker.Infrastructure;
using static Coding_Tracker.Modules.CrudCalls;

namespace Coding_Tracker.Modules
{
    internal static class AppController
    {
        public static bool InSession => AppLogic.InSession;
        public static DateTimeOffset SessionStarted => AppLogic.SessionStarted;

        public static async Task<IEnumerable<CodingSession>> GetHistory()
        {
            return Cache;
        }

        public static void StartSession()
        {
            AppLogic.StartSession();
        }

        public static async Task EndSession()
        {
            await AppLogic.EndSession();
        }

        public static async Task AddSession(DateTimeOffset start, DateTimeOffset end)
        {
            await CrudCalls.AddSession(start, end);
        }

        public static async Task<int> DeleteSession(IEnumerable<CodingSession> sessions)
        {
            return await AppLogic.DeleteSession(sessions);
        }

        public static bool ConvertToSession(int month, int day, int year, int hour, int minute, string meridiem, out DateTimeOffset formattedTime)
        {
            return AppLogic.ConvertToSession(month, day, year, hour, minute, meridiem, out formattedTime);
        }

        public static bool VerifyDurationValidity(DateTimeOffset start, DateTimeOffset end)
        {
            return AppLogic.VerifyDurationValidity(start, end);
        }
    }
}
