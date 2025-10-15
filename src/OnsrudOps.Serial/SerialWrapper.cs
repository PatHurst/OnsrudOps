using System.IO.Ports;

namespace OnsrudOps.Serial;

/// <summary>
/// Class for serial port connection.
/// </summary>
public sealed class SerialWrapper
{
    /// <summary>
    /// The serial port object
    /// </summary>
    private SerialPort _serialPort;

    /// <summary>
    /// The current state of the serial port
    /// </summary>
    private SerialState _state;

    /// <summary>
    /// The locker object for single thread access to the serial port.
    /// </summary>
    private readonly Lock _serialLock = new();

    /// <summary>
    /// Gets the status of the serial port connection
    /// </summary>
    public bool IsConnected => _serialPort.IsOpen;

    /// <summary>
    /// Get the current connection configuration.
    /// </summary>
    public SerialConnectionConfiguration Configuration { get; private set; }

    /// <summary>
    /// Invoked on serial exceptions and messages
    /// </summary>
    public event EventHandler<SerialMessage>? SerialMessage;

    /// <summary>
    /// Invoked when the connection changes, connected or disconnected.
    /// Includes the connection status and configuration.
    /// </summary>
    public event EventHandler<(bool, SerialConnectionConfiguration)>? ConnectionChanged;

    /// <summary>
    /// Invoked when a file is successfully sent through the serial port.
    /// Contains a message with the name of the file that was sent.
    /// </summary>
    public event EventHandler<string>? FileSent;

    /// <summary>
    /// Invoked when the progress of an operation changes.
    /// The float parameter represents a percentage 0% - 100%.
    /// </summary>
    public event EventHandler<float>? Progress;

    /// <summary>
    /// Invoked when data has been received over the serial port.
    /// </summary>
    public event EventHandler<string>? SerialDataReceived;

    /// <summary>
    /// Initialize port with default connection settings.
    /// </summary>
    public SerialWrapper() : this(SerialConnectionConfiguration.Default) { }

    /// <summary>
    /// Initialize port with custom settings.
    /// </summary>
    public SerialWrapper(SerialConnectionConfiguration config)
    {
        _serialPort = Create(config);
        Configuration = config;
        _serialPort.DataReceived += OnSerialDataReceived;
        _state = SerialState.Closed;
    }

    /// <summary>
    /// Class Destructor
    /// </summary>
    ~SerialWrapper()
    {
        DisConnect();
        ConnectionChanged?.Invoke(this, (false, Configuration));
        if (!_serialPort?.IsOpen == true)
            _serialPort?.Close();
        _serialPort?.Dispose();
    }

    /// <summary>
    /// Connect to the serial port.
    /// </summary>
    /// <returns>
    /// True if the connection succedded, false if it failed.
    /// </returns>
    public async Task<bool> ConnectAsync()
    {
        if (_serialPort.IsOpen)
            return true;

        SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Information, "Trying to Connect with settings: " + Configuration));
        return await Task.Run(() =>
        {
            try
            {
                _serialPort.Open();
                _state = SerialState.Open;
                ConnectionChanged?.Invoke(this, (true, Configuration));
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Information,
                    $"Connected Successfully! {this.Configuration}"));
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Unauthorized Access Exception: " + ex.Message));
                ConnectionChanged?.Invoke(this, (false, Configuration));
                return false;
            }
            catch (InvalidOperationException ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Invalid Operation Exception: " + ex.Message));
                ConnectionChanged?.Invoke(this, (false, Configuration));
                return false;
            }
            catch (IOException ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "IO Exception: " + ex.Message));
                ConnectionChanged?.Invoke(this, (false, Configuration));
                return false;
            }
            catch (Exception ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, ex.Message));
                ConnectionChanged?.Invoke(this, (false, Configuration));
                return false;
            }
        });
    }

    /// <summary>
    /// Connect to the serial port with custom configuration.
    /// </summary>
    /// <returns>
    /// True if the connection succedded, false if it failed.
    /// </returns>
    public async Task<bool> ConnectAsync(SerialConnectionConfiguration config)
    {
        DisConnect();
        _serialPort = Create(config);
        return await ConnectAsync();
    }

    /// <summary>
    /// Disconnect from port
    /// </summary>
    public void DisConnect()
    {
        if (_state == SerialState.Writing)
        {
            SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Serial port could not disconnect. Writing in progress."));
            return;
        }
        if (_serialPort?.IsOpen == true)
            _serialPort?.Close();
        _serialPort?.Dispose();
        ConnectionChanged?.Invoke(this, (false, Configuration));
        _state = SerialState.Closed;
    }

    /// <summary>
    /// Send data asynchronously through serial port.
    /// </summary>
    /// <returns>
    /// True if the transaction succeeded, else false.
    /// </returns>
    /// <param name="text"></param>
    public async Task<bool> SendTextAsync(string text)
    {
        bool succeeded = false;
        if (text is null)
            return false;

        // if the port was unable to be opened, bail out with an error.
        if (!ConnectAsync().Result)
        {
            SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Port is not opened! File could not be sent."));
            return false;
        }
              
        await Task.Run(() =>
        {
            try
            {
                // ensure single thread access to the serial port.
                lock (_serialLock)
                {
                    SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Information, $"Serial lock entered by thread: {Environment.CurrentManagedThreadId}"));
                    _state = SerialState.Writing;
                    _serialPort.Write(text);
                }
                succeeded = true;
            }
            catch (InvalidOperationException ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Invalid Operation! " + ex.Message));
            }
            catch (Exception ex)
            {
                SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, ex.Message));
            }
            finally
            {
                _state = SerialState.Open;
            }
        });
        return succeeded;
    }

    /// <summary>
    /// Add a collection of files to the queue
    /// </summary>
    /// <param name="files">A collection of GCode files</param>
    public void AddFilesToQueue(params IFile[] files)
    {
        Task.Run(() => SendFiles(files));
    }

    private async Task SendFiles(IFile[] files)
    {
        int countOfFilesToSend = files.Length;
        foreach (IFile file in files)
        {
            bool succeeded = await SendTextAsync(file.FileContents);
            if (succeeded)
            {
                if (file.FileName is not null)
                    FileSent?.Invoke(this, file.FileName);
                Progress?.Invoke(this, 1.0f / countOfFilesToSend--);
            }
        }
    }

    /// <summary>
    /// Invoked when data is received from serial port
    /// </summary>
    /// <remarks>
    /// If the data is not the typical XON character, it invokes a serial message with the data
    /// </remarks>
    private async void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        await Task.Run(() =>
        {
            SerialPort? serialPort = sender as SerialPort;
            string? data;
            lock (_serialLock)
            {
                data = serialPort?.ReadExisting();
            }
            if (data?.Contains((char)17) == false)
                SerialDataReceived?.Invoke(this, data);
        });
    }

    /// <summary>
    /// Create an instance of a serial port with the given configuration.
    /// </summary>
    /// <param name="config"></param>
    /// <returns>A Serial Port Object</returns>
    private SerialPort Create(SerialConnectionConfiguration config)
    {
        SerialPort port;
        try
        {
            port = new SerialPort()
            {
                PortName = config.PortName,
                BaudRate = config.BaudRate,
                DataBits = config.DataBits,
                StopBits = config.StopBits,
                Parity = config.Parity
            };
            Configuration = config;
        }
        catch (Exception ex)
        {
            SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Error Creating Serial Port: " + ex.Message));
            SerialMessage?.Invoke(this, new SerialMessage(SerialMessageType.Error, "Port Reconnecting with Default Settings"));
            port = Create(SerialConnectionConfiguration.Default);
        }
        return port;
    }

    /// <summary>
    /// The state of the serial connection port
    /// </summary>
    private enum SerialState
    {
        /// <summary>
        /// Port Open
        /// </summary>
        Open,
        /// <summary>
        /// Port Closed
        /// </summary>
        Closed,
        /// <summary>
        /// Port Writing
        /// </summary>
        Writing,
        /// <summary>
        /// Port Receiving
        /// </summary>
        Receiving
    }
}
