using Spectre.Console;
using static Coding_Tracker.Modules.AppLogic;
using static Coding_Tracker.Modules.CrudCalls;

namespace Coding_Tracker.Modules
{
    internal static class ConsoleUI
    {
        private static readonly Color yellow = Color.PaleGreen1;
        private static readonly Color turquoise = Color.PaleTurquoise1;
        private static readonly Color golden = Color.LightGoldenrod1;

        public static async Task DisplayTable()
        {
            var codingSessions = await GetTable();
            foreach (var codingSession in codingSessions)
            {
                var duration = CalculateDuration(codingSession.StartTime, codingSession.EndTime);
            }
        }

        public static void DisplayOptions()
        {

        }
    }
}
