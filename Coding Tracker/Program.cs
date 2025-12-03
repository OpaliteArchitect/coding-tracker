using Coding_Tracker.Modules;

namespace Coding_Tracker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            App app = new();
            await app.Run();
        }
    }
}
