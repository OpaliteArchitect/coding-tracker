using Coding_Tracker.Infrastructure;
using Spectre.Console;
using System.Globalization;
using static Coding_Tracker.Modules.AppController;

namespace Coding_Tracker.Modules
{
    internal static class ConsoleUI
    {
        /*
            palegreen1
            paleturquoise1
            lightgoldenrod1
            salmon1
        */

        private static string GetDuration(CodingSession codingSession)
        {
            var duration = codingSession.Duration;
            var days = Int32.Parse(duration.ToString("dd"));
            var hours = Int32.Parse(duration.ToString("hh"));
            var minutes = Int32.Parse(duration.ToString("mm"));

            var daysStr = days is 0
                ? ""
                : days is 1
                ? $"{days} day, "
                : $"{days} days, ";

            var hoursStr = hours is 0
                ? ""
                : hours is 1
                ? $"{hours} hour, "
                : $"{hours} hours, ";

            var minutesStr = minutes is 0
                ? ""
                : minutes is 1
                ? $"{minutes} minute"
                : $"{minutes} minutes";

            string formattedDuration = $"{daysStr}{hoursStr}{minutesStr}";
            formattedDuration = formattedDuration is "" ? "Less than a minute" : formattedDuration;

            return formattedDuration;
        }

        public static async Task DisplayTable()
        {
            var codingSessions = await GetHistory();
            Table table = new();
            table.AddColumns("Session Start", "Session End", "Duration");
            foreach (var codingSession in codingSessions)
            {
                table.AddRow($"{codingSession.StartTime:F}", $"{codingSession.EndTime:F}", $"{GetDuration(codingSession)}");
            }
            AnsiConsole.Write(table);
        }

        public static string DisplayOptions()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select Option:")
                .AddChoices(
                [
                    "Display past 10 sessions",
                    InSession ? "End current session" : "Start new session",
                    "Manually add session",
                    "Delete session/s",
                    "Exit app"
                ]));

            return choice;
        }

        public static void DisplaySessionStart()
        {
            Panel display = new($"Session started: {SessionStarted}");
            AnsiConsole.Write(display);
        }

        public static void DisplayResult(string result)
        {
            Panel display = new Panel(result)
                .Header("Info");
            AnsiConsole.Write(display);
        }

        private static bool CreateSession(out DateTimeOffset dateAndTime)
        {
            string verification = string.Empty;
            do
            {
                int year = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter year: ")
                    .Validate(y => y switch
                    {
                        < 1900 or > 3000 => ValidationResult.Error("[red]Enter valid year[/]"),
                        _ => ValidationResult.Success()
                    }
                    ));

                int month = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter month (1-12): ")
                    .Validate(M => M switch
                    {
                        < 1 or > 12 => ValidationResult.Error("[red]Enter valid month[/]"),
                        _ => ValidationResult.Success()
                    }
                    ));
                var monthName = DateTimeFormatInfo.InvariantInfo.MonthNames[month - 1];

                int day = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter day: ")
                    .Validate(d =>
                    {
                        int maxDays = DateTime.DaysInMonth(year, month);
                        return d < 1 || d > maxDays
                            ? ValidationResult.Error("[red]Enter valid day[/]")
                            : ValidationResult.Success();
                    }
                    ));

                int hour = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter hour (12-hour format): ")
                    .Validate(h => h switch
                    {
                        < 1 or > 12 => ValidationResult.Error("[red]Enter valid hour[/]"),
                        _ => ValidationResult.Success()
                    }
                    ));

                int minute = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter minute")
                    .DefaultValue(0)
                    .Validate(m => m switch
                    {
                        < 0 or > 59 => ValidationResult.Error("[red]Enter valid minute[/]"),
                        _ => ValidationResult.Success()
                    }
                    ));

                string meridiem = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("AM/PM?")
                    .AddChoices(["AM", "PM"]));

                if (ConvertToSession(month, day, year, hour, minute, meridiem, out dateAndTime))
                {
                    verification = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title($"[paleturquoise1]Is this correct: {Markup.Escape($"[{dateAndTime:F}]")}[/]")
                        .AddChoices(["Yes", "No, let me retry", "No, exit"]));
                }
            } while (verification is "No, let me retry");

            return verification is "Yes";
        }

        public static async Task<string> ManuallyAddSession()
        {
            string verification = string.Empty;
            DateTimeOffset start;
            DateTimeOffset end;

            do
            {
                Rule header = new("[palegreen1]Enter session start[/]");
                AnsiConsole.Write(header);

                if (!CreateSession(out start))
                {
                    return "Cancelled";
                }

                Panel panel = new Panel($"[palegreen1]{start:F}[/]")
                    .Header("[palegreen1]Start time[/]")
                    .HeaderAlignment(Justify.Center);
                AnsiConsole.Write(panel);

                header = new("[lightgoldenrod1]Enter session end[/]");
                AnsiConsole.Write(header);

                if (!CreateSession(out end))
                {
                    return "Cancelled";
                }

                if (VerifyDurationValidity(start, end)) break;
                else
                {
                    verification = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[salmon1]Start time cannot be later than end time![/]")
                        .AddChoices(["Retry", "Exit"]));
                }
            } while (verification is "Retry");

            if (verification is "Exit")
            {
                return "Cancelled";
            }

            await AddSession(start, end);
            return "Session manually added";
        }

        private static bool RunConfirmationPrompt(string message)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                .Title(message)
                .AddChoices(false, true)
            );
        }

        public static async Task DeleteSessions()
        {
            var history = await GetHistory();
            if (!history.Any())
            {
                AnsiConsole.Write(new Panel("[palegreen1]No sessions to delete[/]"));
                return;
            }

            var prompt = new MultiSelectionPrompt<CodingSession>()
                .Title("Select Option:")
                .Required()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to navigate)[/]")
                .InstructionsText("[grey](Press [lightgoldenrod1]<space>[/] to mark session, press [red]<enter>[/] to delete)[/]")
                .AddChoices(history)
                .UseConverter(session =>
                {
                    var start = session.StartTime.ToString("F");
                    var end = session.EndTime.ToString("F");
                    var duration = GetDuration(session);

                    return $"[palegreen1]{start}[/] | [paleturquoise1]{end}[/] | [lightgoldenrod1]{duration}[/]";
                });

            var choices = AnsiConsole.Prompt(prompt);

            Panel panel;
            if (RunConfirmationPrompt($"Delete these sessions?") && RunConfirmationPrompt("Are you sure?"))
            {
                var deleted = await DeleteSession(choices);
                panel = new($"[salmon1]Succesfully deleted [red]{deleted} session{(deleted > 1 ? "s" : "")}[/] from history[/]");
            }
            else
            {
                panel = new($"[palegreen1]Delete cancelled[/]");
            }

            AnsiConsole.Write(panel);
        }
    }
}

