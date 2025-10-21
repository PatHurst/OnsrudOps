namespace OnsrudOps.Serial;

/// <summary>
/// Structure represents an error from the serial port.
/// </summary>
public readonly struct SerialError(Exception exception = null!, string message = "")
{
	/// <summary>
	/// The caught exception.
	/// </summary>
	public Exception? Exception { get; } = exception ?? new Exception(message);

	/// <summary>
	/// A message with additional information about the exception.
	/// </summary>
	public string? Message { get; } = message;
}
