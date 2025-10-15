using Microsoft.Win32;
using OnsrudOps.src;
using OnsrudOps.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.Command
{
    /// <summary>
    /// Command to open G Code files
    /// </summary>
    class OpenFilesCommand : ICommand
    {
        public void Execute()
        {
            OpenFileDialog ofd = new()
            {
                Title = "Select G Code Files",
                Filter = "Code Files|*.anc;*.txt;*.nc",
                Multiselect = true
            };

            if (ofd.ShowDialog() != true)
                return;

            foreach (string file in ofd.FileNames)
            {
                if (!MainWindow.ViewModel.CodeFiles.Select(f => f.FullName).Contains(file))
                    MainWindow.ViewModel.CodeFiles.Add(new GCodeFile(file));
            }
        }
    }
}
