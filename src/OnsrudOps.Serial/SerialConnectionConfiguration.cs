using System.IO.Ports;

namespace OnsrudOps.Serial
{
    /// <summary>
    /// Data structure to hold serial connection settings
    /// </summary>
    /// <remarks>
    /// Initialize a settings configuration
    /// </remarks>
    public readonly struct SerialConnectionConfiguration(string portName, int baud, int dataBits, StopBits stopBits, Parity parity)
    {
        public string PortName { get; } = portName;
        public int BaudRate { get; } = baud;
        public int DataBits { get; } = dataBits;
        public StopBits StopBits { get; } = stopBits;
        public Parity Parity { get; } = parity;

        /// <summary>
        /// The default Settings Configuration.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>PortName: COM3</item>
        /// <item>Baud: 38400</item>
        /// <item>DataBites: 7</item>
        /// <item>StopBits: 2</item>
        /// <item>Parity: Even</item>
        /// </list>
        /// </remarks>
        public static SerialConnectionConfiguration Default
        {
            get
            {
                return new SerialConnectionConfiguration("COM3", 38400, 7, StopBits.Two, Parity.Even);
            }
        }

        public override string ToString()
        {
            return string.Format("PortName: {0}; BaudRate: {1}; Data Bits: {2}; Stop Bits: {3}; Parity: {4}",
                PortName, BaudRate, DataBits, StopBits, Parity);
        }
    }
}
