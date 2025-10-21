using System.IO.Ports;
using System.Threading.Tasks;

namespace OnsrudOps.Serial.Tests;

[TestClass]
public sealed class SerialWrapperTests
{
    [TestMethod]
    public void AssertConnectionMethodResultReturnMatchesProperty()
    {
        SerialWrapper serialWrapper = new();
        bool connectionSucceeded = serialWrapper.ConnectAsync().Result;
        Assert.IsTrue(connectionSucceeded == serialWrapper.IsConnected);
    }

    [TestMethod]
    public async Task ConfigurationArgumentMatchesClassProperty()
    {
        SerialConnectionConfiguration config = new("COM3", 110, 7, StopBits.None, Parity.None);
        SerialWrapper serialWrapper = new(config);
        Assert.AreEqual(serialWrapper.Configuration, config);
        await serialWrapper.ConnectAsync(SerialConnectionConfiguration.Default);
        Assert.AreEqual(serialWrapper.Configuration, SerialConnectionConfiguration.Default);
    }
}
