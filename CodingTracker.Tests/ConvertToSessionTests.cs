using Coding_Tracker.Modules;

namespace CodingTracker.Tests;

public class ConvertToSessionTests
{
    [Fact]
    public void ConvertToSession_Valid12HourTime_ParsesCorrectly()
    {
        bool result = AppLogic.ConvertToSession(6, 28, 2026, 2, 30, "PM", out DateTimeOffset formattedTime);

        Assert.True(result);
        Assert.Equal(14, formattedTime.Hour);
        Assert.Equal(30, formattedTime.Minute);
        Assert.Equal(new DateTime(2026, 6, 28, 14, 30, 0), formattedTime.LocalDateTime);
    }

    [Fact]
    public void ConvertToSession_Midnight12Am_ParsesCorrectly()
    {
        bool result = AppLogic.ConvertToSession(1, 1, 2026, 12, 0, "AM", out DateTimeOffset formattedTime);

        Assert.True(result);
        Assert.Equal(0, formattedTime.Hour);
        Assert.Equal(0, formattedTime.Minute);
    }

    [Fact]
    public void ConvertToSession_Noontime12Pm_ParsesCorrectly()
    {
        bool result = AppLogic.ConvertToSession(1, 1, 2026, 12, 0, "PM", out DateTimeOffset formattedTime);

        Assert.True(result);
        Assert.Equal(12, formattedTime.Hour);
        Assert.Equal(0, formattedTime.Minute);
    }

    [Fact]
    public void ConvertToSession_InvalidMonth_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(13, 1, 2026, 1, 0, "AM", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_InvalidDayForMonth_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(2, 30, 2026, 1, 0, "AM", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_InvalidMeridiem_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(6, 28, 2026, 1, 0, "XX", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_LeapDayInLeapYear_ParsesCorrectly()
    {
        bool result = AppLogic.ConvertToSession(2, 29, 2024, 9, 15, "AM", out DateTimeOffset formattedTime);

        Assert.True(result);
        Assert.Equal(9, formattedTime.Hour);
        Assert.Equal(15, formattedTime.Minute);
    }

    [Fact]
    public void ConvertToSession_LeapDayInNonLeapYear_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(2, 29, 2023, 9, 15, "AM", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_InvalidHour_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(6, 28, 2026, 13, 0, "AM", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_InvalidMinute_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(6, 28, 2026, 1, 60, "AM", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_EmptyMeridiem_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(6, 28, 2026, 1, 0, "", out DateTimeOffset formattedTime);

        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }

    [Fact]
    public void ConvertToSession_NegativeMonth_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(-1, 1, 2026, 1, 0, "AM", out DateTimeOffset formattedTime);
    
        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }
    
    [Fact]
    public void ConvertToSession_NegativeDay_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(1, -1, 2026, 1, 0, "AM", out DateTimeOffset formattedTime);
    
        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }
    
    [Fact]
    public void ConvertToSession_NegativeYear_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(1, 1, -2026, 1, 0, "AM", out DateTimeOffset formattedTime);
    
        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }
    
    [Fact]
    public void ConvertToSession_NegativeHour_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(1, 1, 2026, -1, 0, "AM", out DateTimeOffset formattedTime);
    
        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }
    
    [Fact]
    public void ConvertToSession_NegativeMinute_ReturnsFalse()
    {
        bool result = AppLogic.ConvertToSession(1, 1, 2026, 1, -1, "AM", out DateTimeOffset formattedTime);
    
        Assert.False(result);
        Assert.Equal(default, formattedTime);
    }
}
