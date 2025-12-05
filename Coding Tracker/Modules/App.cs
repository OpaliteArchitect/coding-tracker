using Coding_Tracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Coding_Tracker.Modules.AppController;
using static Coding_Tracker.Modules.ConsoleUI;
using static Coding_Tracker.Modules.CrudCalls;

namespace Coding_Tracker.Modules
{
    public static class App
    {
        public static async Task ApplyMigrations()
        {
            await using RecordDbContext db = new();
            await db.Database.MigrateAsync();
        }

        public static async Task Run()
        {
            await ApplyMigrations();
            await LoadCacheFromDatabase();

            while (true)
            {
                Console.Clear();
                await MainLoop();
            }
        }

        private static async Task MainLoop()
        {
            if (InSession)
            {
                DisplaySessionStart();
            }
            string choice = DisplayOptions();

            switch (choice)
            {
                case "Display past 10 sessions":
                    await DisplayTable();
                    Console.ReadKey(true);
                    break;
                case "Start new session":
                    StartSession();
                    break;
                case "End current session":
                    await EndSession();
                    DisplayResult("Session logged");
                    Console.ReadKey(true);
                    break;
                case "Manually add session":
                    await ManuallyAddSession();
                    break;
                case "Delete session/s":
                    await DeleteSessions();
                    Console.ReadKey(true);
                    break;
                case "Exit app":
                    Environment.Exit(0);
                    break;
                default: break;
            }
        }
    }
}
