using Coding_Tracker.Infrastructure;
using System.Globalization;

namespace Coding_Tracker.Modules
{
    internal static class CrudCalls
    {
        public static async Task<List<CodingSession>> GetTable()
        {
            using RecordDbContext db = new();
            var sessions = db.Sessions;
            return sessions.ToList();
        }

        public static async Task<string> AddSession(int startMonth, int startDay, int startYear, int startHour, int startMinute, string startMeridiem,
            int endMonth, int endDay, int endYear, int endHour, int endMinute, string endMeridiem)
        {
            const string format = "MM/dd/yyyy hh:mm tt";

            string startStr = $"{startMonth:D2}/{startDay:D2}/{startYear:D2} {startHour:D2}:{startMinute:D2} {startMeridiem}";
            string endStr = $"{endMonth:D2}/{endDay:D2}/{endYear:D2} {endHour:D2}:{endMinute:D2} {endMeridiem}";

            DateTimeOffset start;
            DateTimeOffset end;

            try
            {
                start = DateTimeOffset.ParseExact(startStr, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                end = DateTimeOffset.ParseExact(endStr, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            }
            catch (FormatException)
            {
                return "Input date/time format error";
            }

            if (end <= start)
            {
                return "Session end cannot be earlier than session start";
            }

            return await AddSession(start, end);
        }

        public static async Task<string> AddSession(DateTimeOffset start, DateTimeOffset end)
        {
            CodingSession cs = new()
            {
                StartTime = start,
                EndTime = end
            };
            await using RecordDbContext db = new();
            await db.Sessions.AddAsync(cs);
            await db.SaveChangesAsync();

            return "Session logged";
        }
    }
}
