namespace OnsrudOps.Serial.Tests;

[TestClass]
public sealed class SerialWrapperTests
{
    [TestMethod]
    public void AssertConnectionReturnsFalseWhenFailed()
    {
        SerialWrapper serialWrapper = new();
        bool connectionSucceeded = serialWrapper.ConnectAsync().Result;
        Assert.IsTrue(connectionSucceeded == serialWrapper.IsConnected);
    }
}
