using Coding_Tracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Coding_Tracker.Modules.ConsoleUI;
using static Coding_Tracker.Modules.CrudCalls;

namespace Coding_Tracker.Modules
{
    public class App
    {
        public bool InSession { get; private set; } = false;
        private DateTimeOffset _sessionStarted;

        public async Task ApplyMigrations()
        {
            await using RecordDbContext db = new();
            await db.Database.MigrateAsync();
        }

        public async Task Run()
        {
            await ApplyMigrations();

            while (true)
            {
                Console.Clear();
                await DisplayTable();
                DisplayOptions();

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        break;
                    case '2':
                        if (InSession)
                        {
                            await EndSession();
                        }
                        else
                        {
                            StartSession();
                        }
                        break;
                    default: break;
                }
            }
        }
        public void StartSession()
        {
            InSession = true;
            _sessionStarted = DateTimeOffset.Now;
        }

        public async Task<string> EndSession()
        {
            if (!InSession)
            {
                throw new InvalidOperationException("Should not be possible");
            }

            InSession = false;
            return await AddSession(_sessionStarted, DateTimeOffset.Now);
        }
    }
}
