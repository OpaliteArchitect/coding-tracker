using Coding_Tracker.Modules;
using System.Reflection;

namespace CodingTracker.Tests;

public class ModulesUnitTests
{
    private static void SetInSessionState(bool value)
    {
        var prop = typeof(AppLogic).GetProperty(
            nameof(AppLogic.InSession),
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
        );
        prop!.SetValue(null, value);
    }

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

    [Fact]
    public void StartSession_SetsInSessionToTrue()
    {
        // Arrange
        AppLogic.StartSession();

        // Act
        bool inSession = AppLogic.InSession;

        // Assert
        Assert.True(inSession);
    }


    [Fact]
    public void StartSession_SetsSessionStartedToCurrentTime()
    {
        // Arrange
        var beforeStart = DateTimeOffset.Now;
        AppLogic.StartSession();
        var afterStart = DateTimeOffset.Now;

        // Act
        DateTimeOffset sessionStarted = AppLogic.SessionStarted;

        // Assert
        Assert.InRange(sessionStarted, beforeStart, afterStart);
    }

    [Fact]
    public async Task EndSession_WhenNotInSession_ThrowsInvalidOperationException()
    {
        // Arrange
        SetInSessionState(false);
        Assert.False(AppLogic.InSession);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => AppLogic.EndSession());
    }
}
