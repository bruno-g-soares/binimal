using Binimal;
using Xunit;

namespace Binimal.Core.Tests;

public sealed class LaunchModeTests
{
    [Fact]
    public void SelfTestFlagSelectsDiagnosticMode()
    {
        Assert.True(LaunchMode.IsSelfTest(new[] { "--self-test" }));
    }

    [Fact]
    public void NormalLaunchDoesNotSelectDiagnosticMode()
    {
        Assert.False(LaunchMode.IsSelfTest(System.Array.Empty<string>()));
    }
}
