using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using OnsrudOps.src;
using OnsrudOps.Serial;
using System.Windows;

namespace OnsrudOps.Command
{
    internal class SendFilesCommand : ICommand
    {
        private List<GCodeFile> files = [];

        public SendFilesCommand(List<GCodeFile> filesToSend)
        {
            files.AddRange(filesToSend);
        }

        public SendFilesCommand(GCodeFile file)
        {
            //MessageBox.Show(file.FileContents);
            files.Add(file);
        }

        public void Execute()
        {
            App.SerialPort.AddFilesToQueue([.. files]);
        }
    }
}
