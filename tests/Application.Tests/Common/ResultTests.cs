using RECAMAS.Application.Common;

namespace RECAMAS.Application.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Ok_SetsSuccessAndData()
    {
        var result = Result<int>.Ok(42, "done");

        Assert.True(result.Success);
        Assert.Equal(42, result.Data);
        Assert.Equal("done", result.Message);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public void Fail_SetsErrorCodeAndMessage()
    {
        var result = Result<int>.Fail("not found", "CASE-404");

        Assert.False(result.Success);
        Assert.Equal("not found", result.Message);
        Assert.Equal("CASE-404", result.ErrorCode);
    }
}
