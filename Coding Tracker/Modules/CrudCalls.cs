using Coding_Tracker.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Coding_Tracker.Modules
{
    internal static class CrudCalls
    {
        public static IEnumerable<CodingSession> Cache { get; private set; } = [];
        public static async Task LoadCacheFromDatabase()
        {
            await using RecordDbContext db = new();
            Cache = await db.Sessions.ToListAsync();
        }

        public static async Task AddSession(DateTimeOffset start, DateTimeOffset end)
        {
            CodingSession cs = new()
            {
                StartTime = start,
                EndTime = end
            };
            await using RecordDbContext db = new();
            await db.Sessions.AddAsync(cs);
            await db.SaveChangesAsync();

            await LoadCacheFromDatabase();
        }

        public static async Task<int> DeleteSession(IEnumerable<int> ids)
        {
            await using RecordDbContext db = new();
            var query = db.Sessions.Where(s => ids.Contains(s.Id));
            int rowsDeleted = await query.ExecuteDeleteAsync();

            await LoadCacheFromDatabase();

            return rowsDeleted;
        }
    }
}
