using Microsoft.Win32;
using OnsrudOps.Serial;
using OnsrudOps.src;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OnsrudOps.UI;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    private bool _settingsModified;

    readonly int[] baudRates =
    [
        110,
        300,
        600,
        1200,
        2400,
        4800,
        9600,
        14400,
        19200,
        38400,
        57600,
        115200,
        128000,
        256000
    ];

    readonly RadioButton[] dataBitsRadioButtons;
    //readonly RadioButton[] stopBitsRadioButtons;
    //readonly RadioButton[] parityRadioButtons;

    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="parent"></param>
    public SettingsWindow(MainWindow parent)
    {
        InitializeComponent();
        InitializeFieldsFromRegistry();
        dataBitsRadioButtons = [DataBits7RadioButton, DataBits8RadioButton];
        //stopBitsRadioButtons = [StopBitsNoneRadioButton, StopBitsOneRadioButton, StopBitsTwoRadioButton];
        //parityRadioButtons = [ParityNoneRadioButton, ParityOddRadioButton, ParityEvenRadioButton];
        _settingsModified = false;
    }

    private async void Close_Btn_Click(object sender, RoutedEventArgs e)
    {
        SerialConnectionConfiguration configuration = GetEnteredValues();
        if (_settingsModified)
        {
            MessageBoxResult result = MessageBox.Show("Save Settings?", "Save", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SaveSettingsToRegistry(configuration);
                TestConnectionLabel.Content = "Trying to Connect... Please Wait.";
                this.Close_Btn.IsEnabled = false;
                await App.SerialPort.ConnectAsync(configuration);
            }
        }
        this.Close();
    }

    private void ChangeSyntaxFilePath_Btn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        if (ofd.ShowDialog() == true)
        {
            SyntaxPathLabel.Content = ofd.FileName;
            Settings.PathToSyntaxHighlightingFile = ofd.FileName;
        }
    }

    private void ChangeEdytorNCPath_Btn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new();
        if (ofd.ShowDialog() == true)
        {
            EdytorNCPath_Lbl.Content = ofd.FileName;
            Settings.PathToEdytorNC = ofd.FileName;
        }
    }

    private void SaveSettingsToRegistry(SerialConnectionConfiguration config)
    {
        Settings.SerialConnectionConfiguration = config;
    }

    private void InitializeFieldsFromRegistry()
    {
        EdytorNCPath_Lbl.Content = Settings.PathToEdytorNC;
        SyntaxPathLabel.Content = Settings.PathToSyntaxHighlightingFile;

        this.PortNameTextBox.Text = Settings.SerialConnectionConfiguration.PortName;
        this.BaudRateComboBox.SelectedIndex = getIndexOf(Settings.SerialConnectionConfiguration.BaudRate);

        switch (Settings.SerialConnectionConfiguration.DataBits)
        {
            case 7:
            this.DataBits7RadioButton.IsChecked = true;
            break;
            case 8:
            this.DataBits8RadioButton.IsChecked = true;
            break;
        }

        switch (Settings.SerialConnectionConfiguration.StopBits)
        {
            case StopBits.None:
            this.StopBitsNoneRadioButton.IsChecked = true;
            break;
            case StopBits.One:
            this.StopBitsOneRadioButton.IsChecked = true;
            break;
            case StopBits.Two:
            this.StopBitsTwoRadioButton.IsChecked = true;
            break;
        }

        switch (Settings.SerialConnectionConfiguration.Parity)
        {
            case Parity.None:
            this.ParityNoneRadioButton.IsChecked = true;
            break;
            case Parity.Odd:
            this.ParityOddRadioButton.IsChecked = true;
            break;
            case Parity.Even:
            this.ParityEvenRadioButton.IsChecked = true;
            break;
        }

        int getIndexOf(int baud)
        {
            for (int i = 0; i < baudRates.Length; i++)
            {
                if (baudRates[i] == baud)
                    return i;
            }
            throw new ArgumentException($"{baud} is not a valid Baud Rate!", nameof(baud));
        }

    }

    private async void TryConnectButton_Click(object sender, RoutedEventArgs e)
    {
        TestConnectionLabel.Content = "Trying to Connect... Please Wait.";
        bool succeeded = await App.SerialPort.ConnectAsync(GetEnteredValues());
        if (succeeded)
        {
            TestConnectionLabel.Content = "Connection Succeeded!";
            TestConnectionLabel.Foreground = Application.Current.Resources["SuccessBrush"] as Brush;
        }
        else
        {
            TestConnectionLabel.Content = "Connection Failed :(";
            TestConnectionLabel.Foreground = Application.Current.Resources["ErrorBrush"] as Brush;
        }
    }

    private SerialConnectionConfiguration GetEnteredValues()
    {
        // I ented up using ifs instead of a cast, casting the tags to an enum was buggy.

        StopBits stopBits;
        if (StopBitsNoneRadioButton.IsChecked == true)
            stopBits = StopBits.None;
        else if (StopBitsOneRadioButton.IsChecked == true)
            stopBits = StopBits.One;
        else if (StopBitsTwoRadioButton.IsChecked == true)
            stopBits = StopBits.Two;
        else
            stopBits = StopBits.None;

        Parity parity;
        if (ParityNoneRadioButton.IsChecked == true)
            parity = Parity.None;
        else if (ParityOddRadioButton.IsChecked == true)
            parity = Parity.Odd;
        else if (ParityEvenRadioButton.IsChecked == true)
            parity = Parity.Even;
        else
            parity = Parity.None;

        int dataBits = int.Parse((string)dataBitsRadioButtons.Where(b => b.IsChecked == true).First().Tag);
        return new(PortNameTextBox.Text, baudRates[BaudRateComboBox.SelectedIndex], dataBits, stopBits, parity);
    }

    private void BaudRateComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) => _settingsModified = true;

    private void PortNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => _settingsModified = true;

    private void DataBitsRadioButton_Checked(object sender, RoutedEventArgs e) => _settingsModified = true;

    private void StopBitsRadioButton_Checked(object sender, RoutedEventArgs e) => _settingsModified = true;

    private void ParityRadioButton_Checked(object sender, RoutedEventArgs e) => _settingsModified = true;
}
