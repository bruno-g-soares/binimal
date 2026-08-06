namespace Binimal.Tests;

using Binimal;
using Xunit;

public sealed class StartupCommandTests
{
    [Fact]
    public void Matches_ReturnsTrueForExactlyQuotedCurrentExecutable()
    {
        Assert.True(StartupCommand.Matches("\"C:\\Apps\\Binimal.exe\"", @"C:\Apps\Binimal.exe"));
    }

    [Fact]
    public void Matches_ReturnsFalseForStaleExecutablePath()
    {
        Assert.False(StartupCommand.Matches("\"C:\\Old\\Binimal.exe\"", @"C:\Apps\Binimal.exe"));
    }

    [Theory]
    [InlineData("C:\\Apps\\Binimal.exe")]
    [InlineData("\"C:\\Apps\\Binimal.exe\" --minimized")]
    [InlineData(" \"C:\\Apps\\Binimal.exe\"")]
    [InlineData("")]
    [InlineData(null)]
    public void Matches_ReturnsFalseUnlessCommandIsExactlyQuoted(string? registeredCommand)
    {
        Assert.False(StartupCommand.Matches(registeredCommand, @"C:\Apps\Binimal.exe"));
    }
}
