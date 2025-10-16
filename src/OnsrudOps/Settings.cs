using OnsrudOps.Serial;
using System.IO.Ports;
using System.Windows;
using Microsoft.Win32;

namespace OnsrudOps.src;

/// <summary>
/// Static application settings
/// </summary>
/// <remarks>
/// Property names correspond to registry value desription names
/// </remarks>
static class Settings
{
    // The key path to where the settings are stored
    private static readonly string keyPath = "HKEY_CURRENT_USER\\Software\\Hurst Software\\Slab Surface";

    private static string _pathToEdytoryNC = GetValueFromRegistry<string>(nameof(PathToEdytorNC));

    private static string _pathToSyntaxHighlightingFile = GetValueFromRegistry<string>(nameof(PathToSyntaxHighlightingFile));

    // serial connection properties stored in registry
    private static string _portName = GetValueFromRegistry<string>("PortName");
    private static int _baudRate = GetValueFromRegistry<int>("BaudRate");
    private static int _dataBits = GetValueFromRegistry<int>("DataBits");
    private static int _stopBits = GetValueFromRegistry<int>("StopBits");
    private static int _parity = GetValueFromRegistry<int>("Parity");

    /// <summary>
    /// The path to the EdytorNC program
    /// </summary>
    public static string PathToEdytorNC
    {
        get => _pathToEdytoryNC ?? string.Empty;
        set
        {
            SetValueInRegistry(nameof(PathToEdytorNC), value);
            _pathToEdytoryNC = value;
        }
    }

    public static string PathToSyntaxHighlightingFile
    {
        get => _pathToSyntaxHighlightingFile ?? string.Empty;
        set
        {
            SetValueInRegistry(nameof(PathToSyntaxHighlightingFile), value);
            _pathToSyntaxHighlightingFile = value;
        }
    }

    /// <summary>
    /// Return the serial connection config stored in registry
    /// </summary>
    public static SerialConnectionConfiguration SerialConnectionConfiguration
    {
        get
        {
            return new SerialConnectionConfiguration(
                _portName, _baudRate, _dataBits, (StopBits)_stopBits, (Parity)_parity);
        }
        set
        {
            _portName = value.PortName;
            _baudRate = value.BaudRate;
            _dataBits = value.DataBits;
            _stopBits = (int)value.StopBits;
            _parity = (int)value.Parity;
            SetValueInRegistry("PortName", _portName);
            SetValueInRegistry("BaudRate", _baudRate);
            SetValueInRegistry("DataBits", _dataBits);
            SetValueInRegistry("StopBits", _stopBits);
            SetValueInRegistry("Parity", _parity);
        }
    }

    /// <summary>
    /// Get a value from the registry
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="valueName"></param>
    /// <returns>The value if it exists, else null</returns>
    private static T GetValueFromRegistry<T>(string valueName)
    {
        object? read;
        if ((read = Registry.GetValue(keyPath, valueName, null)) is null)
        {
            // if the value is null, initialize a key in the registry and alert the user
            if (typeof(T) == typeof(string))
                SetValueInRegistry(valueName, string.Empty);
            else if (typeof(T) == typeof(int))
                SetValueInRegistry(valueName, 0);

            MessageBox.Show($"No value for {valueName} found in registry!");
        }
        return read is null ? default! : (T)read!;
    }

    /// <summary>
    /// Set a value in the registry
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="valueName"></param>
    /// <param name="value"></param>
    private static void SetValueInRegistry<T>(string valueName, T value)
    {
        switch (value)
        {
            case int:
            Registry.SetValue(keyPath, valueName, value, RegistryValueKind.DWord);
            break;
            case string:
            Registry.SetValue(keyPath, valueName, value, RegistryValueKind.String);
            break;
        }
    }
}
