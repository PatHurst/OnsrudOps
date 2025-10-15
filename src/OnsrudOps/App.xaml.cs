using OnsrudOps.Serial;
using OnsrudOps.src;
using OnsrudOps.UI;
using System.Configuration;
using System.IO;
using System.Windows;

namespace OnsrudOps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The Serial Port Connection Point
        /// </summary>
        internal static SerialWrapper SerialPort { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitializeComponent();
            SerialPort = new(Settings.SerialConnectionConfiguration);
            foreach (string arg in Environment.GetCommandLineArgs().Skip(1))
            {
                if (File.Exists(arg))
                {
                    switch (arg[arg.LastIndexOf('.')..])
                    {
                        case ".anc" or ".nc" or ".txt":
                        UI.MainWindow.ViewModel.CodeFiles.Add(new GCodeFile(arg));
                        break;
                        default:
                        break;
                    }
                }
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled Exception!!!\n" + e.Exception.Message + e.Exception.StackTrace);
            e.Handled = true;
        }
    }
}
