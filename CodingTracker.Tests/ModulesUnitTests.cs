using Coding_Tracker.Modules;

namespace CodingTracker.Tests;

public class ModulesUnitTests
{
    [Fact]
    public void VerifyDurationValidity_StartEarlierThanEnd_ReturnsTrue()
    {
        var start = DateTimeOffset.Now;
        var end = start.AddHours(1);

        bool isValid = AppLogic.VerifyDurationValidity(start, end);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyDurationValidity_StartLaterThanEnd_ReturnsFalse()
    {
        var start = DateTimeOffset.Now;
        var end = start.AddHours(-1);

        bool isValid = AppLogic.VerifyDurationValidity(start, end);

        Assert.False(isValid);
    }

    [Fact]
    public void VerifyDurationValidity_StartAtTheSameTimeAsEnd_ReturnsFalse()
    {
        var now = DateTimeOffset.Now;
        var start = now;
        var end = now;

        bool isValid = AppLogic.VerifyDurationValidity(start, end);

        Assert.False(isValid);
    }
}
