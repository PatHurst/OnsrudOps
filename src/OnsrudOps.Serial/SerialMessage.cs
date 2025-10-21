namespace OnsrudOps.Serial;

/// <summary>
/// Structure represents a message from the serial port
/// </summary>
public readonly struct SerialMessage(string message)
{
    /// <summary>
    /// The message data
    /// </summary>
    public string Message { get; } = message;
}
