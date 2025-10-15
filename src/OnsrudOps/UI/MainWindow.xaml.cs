using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using OnsrudOps.Serial;
using OnsrudOps.src;
using OnsrudOps.Command;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using OnsrudOps.ReCut;

namespace OnsrudOps.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    internal static ViewModel ViewModel = new();

    public MainWindow()
    {
        InitializeComponent();
        this.CodeFile_LstBx.ItemsSource = ViewModel.CodeFiles;
        this.textEditor.DataContext = ViewModel.CurrentFile;
        this.DataContext = ViewModel;

        this.textEditor.SyntaxHighlighting = LoadSyntaxHighlightingDefinition();

        App.SerialPort.ConnectionChanged += OnSerialConnectionChanged;
        App.SerialPort.SerialMessage += OnSerialMessage;

        App.SerialPort.FileSent += OnSerialQueue_FileSent;
        App.SerialPort.Progress += OnSerialQueue_ProgressChanged;
    }

    private void OnSerialQueue_ProgressChanged(object? sender, float e)
    {
        Dispatcher.Invoke(() => ProgressBar.Value = e * 100);
    }

    private void OnSerialQueue_FileSent(object? sender, string e)
    {
        Dispatcher.Invoke(() =>
        {
            Run run = new(e + " sent successfully!");
            Paragraph p = new(run)
            {
                Foreground = Application.Current.Resources["SuccessBrush"] as Brush
            };
            SerialPortDisplay.Blocks.Add(p);
            SerialPort_TxtBlck.ScrollToEnd();
        });
    }

    private void CreateCode_Btn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Parameters parameters = new();
            parameters.Build(ProgramName_TxtBx.Text, PartWidth_TxtBx.Text, PartLength_TxtBx.Text, StartingThickness_TxtBx.Text, FinishedThickness_TxtBx.Text, VerticalStep_TxtBx.Text);
            GCodeFile file = new(new OnsrudOps.ReCut.CreateGCodeFileCommand(parameters).BuildFile());
            ViewModel.CodeFiles.Add(file);
            TabControl.SelectedIndex = 0;
            CodeFile_LstBx.SelectedIndex = ViewModel.CodeFiles.Count - 1;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        new OpenFilesCommand().Execute();
        CodeFile_LstBx.SelectedIndex = 0;
    }

    private async void SaveCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        foreach (GCodeFile file in ViewModel.CodeFiles.Where(f => f.Modified))
            await file.SaveAsync();
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void SettingsMenu_Click(object sender, RoutedEventArgs e)
    {
        SettingsWindow sw = new(this);
        sw.ShowDialog();
    }

    private async void ConnectToSerialPort_Btn_Click(object sender, RoutedEventArgs e)
    {
        await App.SerialPort.ConnectAsync();
    }

    private void OnSerialConnectionChanged(object? sender, (bool, SerialConnectionConfiguration) cfg)
    {
        // Event handler may be invoked from thread connecting to serial port. 
        // Need to use dispatcher to request UI to update item.
        Dispatcher.Invoke(() =>
        {
            if (cfg.Item1)
            {
                SerialStatusLabel.Foreground = Application.Current.Resources["SuccessBrush"] as Brush;
                SerialStatusLabel.Content = "Serial Port Connected";
                SerialConnectionConfigLabel.Foreground = Application.Current.Resources["SuccessBrush"] as Brush;
            }
            else
            {
                SerialStatusLabel.Foreground = Application.Current.Resources["ErrorBrush"] as Brush;
                SerialStatusLabel.Content = "Serial Port Disconnected...";
                SerialConnectionConfigLabel.Foreground = Application.Current.Resources["ErrorBrush"] as Brush;
            }
            ConnectToSerialPort_Btn.IsEnabled = !cfg.Item1;
            SerialConnectionConfigLabel.Content = cfg.Item2;
        });
    }

    /// <summary>
    /// Called when a message is received from the serial wrapper
    /// </summary>
    private void OnSerialMessage(object? sender, SerialMessage message)
    {
        // Event handler may be invoked from thread connecting to serial port. 
        // Need to use dispatcher to request UI to update item.
        Dispatcher.Invoke(() =>
        {
            Run run = new(message.Message);
            Paragraph p = new(run)
            {
                Foreground = message.MessageType == SerialMessageType.Error ? Application.Current.Resources["ErrorBrush"] as Brush :
                    Application.Current.Resources["SuccessBrush"] as Brush
            };
            SerialPortDisplay.Blocks.Add(p);
            SerialPort_TxtBlck.ScrollToEnd();
        });
    }

    private void SendFiles_Btn_Click(object sender, RoutedEventArgs e)
    {
        new SendFilesCommand([.. ViewModel.CodeFiles]).Execute();
    }

    private void CodeFile_LstBx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        // update the current file to be equal to the selected index
        if (CodeFile_LstBx.SelectedIndex == -1)
        {
            textEditor.Text = string.Empty;
            return;
        }
        ViewModel.CurrentFile = ViewModel.CodeFiles[CodeFile_LstBx.SelectedIndex];
        textEditor.Text = ViewModel.CurrentFile.FileContents;
        textEditor.ScrollToHome();
    }

    /// <summary>
    /// Invoked when the X is click on a list box items
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            if (CodeFile_LstBx.SelectedIndex != -1)
            {
                bool savedSuccessfully = false;
                if (ViewModel.CodeFiles[CodeFile_LstBx.SelectedIndex].Modified)
                {
                    if (MessageBox.Show($"Do you want to save your changes to {ViewModel.CodeFiles[CodeFile_LstBx.SelectedIndex].FileName}?", 
                        "Save Changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        savedSuccessfully = await ViewModel.CodeFiles[CodeFile_LstBx.SelectedIndex].SaveAsync();
                    }
                }
                if (!savedSuccessfully)
                    return;
                ViewModel.CodeFiles.RemoveAt(CodeFile_LstBx.SelectedIndex);
            }
            if (ViewModel.CodeFiles.Count == 0)
                ViewModel.CurrentFile = GCodeFile.EmptyFile;
            else
                CodeFile_LstBx.SelectedIndex = ViewModel.CodeFiles.Count - 1;
        }
        catch (Exception) { } // might as well just swallow the exception
    }

    private void SendSingleFileButton_Click(object sender, RoutedEventArgs e)
    {
        new SendFilesCommand(ViewModel.CurrentFile).Execute();
    }

    private async void Window_Closing(object sender, CancelEventArgs e)
    {
        // Check if the user has unsaved files. prompt for saving
        bool unsavedFiles = ViewModel.CodeFiles.Select(f => f.Modified).Where(m => m).Any();
        if (unsavedFiles)
        {
            if (MessageBox.Show("You have unsaved files. Do you want to save them?", "Files Unsaved", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (GCodeFile file in ViewModel.CodeFiles.Where(f => f.Modified))
                    await file.SaveAsync();
            }
        }
    }

    private void TextEditor_TextChanged(object sender, EventArgs e)
    {
        if (ViewModel.CurrentFile == GCodeFile.EmptyFile)
            return;
        ViewModel.CurrentFile.FileContents = textEditor.Text;
    }

    private static IHighlightingDefinition LoadSyntaxHighlightingDefinition()
    {
        IHighlightingDefinition definition = null;
        try
        {
            // load the file saved in settings
            using var reader = new XmlTextReader(
                new FileStream(Settings.PathToSyntaxHighlightingFile, FileMode.Open));
            definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error Loading {Settings.PathToSyntaxHighlightingFile}:{Environment.NewLine}{ex.Message}");
        }
        return definition ?? HighlightingManager.Instance.HighlightingDefinitions.First();
    }

    private void EditorUndoButton_Click(object sender, RoutedEventArgs e)
    {
        if (textEditor.CanUndo)
            textEditor.Undo();
    }

    private void EditorScrollToEndButton_Click(object sender, RoutedEventArgs e)
    {
        textEditor.ScrollToEnd();
    }

    private void EditorScrollToTopButton_Click(object sender, RoutedEventArgs e)
    {
        textEditor.ScrollToHome();
    }

    private void EditorRedoButton_Click(object sender, RoutedEventArgs e)
    {
        if (textEditor.CanRedo)
            textEditor.Redo();
    }
}