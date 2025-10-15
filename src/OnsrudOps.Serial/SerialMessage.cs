namespace OnsrudOps.Serial;

/// <summary>
/// Structure represents a message from the serial port
/// </summary>
/// <remarks>
/// Construct a message
/// </remarks>
public readonly struct SerialMessage(SerialMessageType type, string message)
{

    /// <summary>
    /// The message type for this message
    /// </summary>
    public SerialMessageType MessageType { get; } = type;

    /// <summary>
    /// The message data
    /// </summary>
    public string? Message { get; } = message;
}
