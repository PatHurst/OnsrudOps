using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace OnsrudOps.src;

class OnsrudGCodeBuilder : IGCodeBuilder
{
//        /// <summary>
//        /// The actual file string
//        /// </summary>
//        private StringBuilder gCodeString = new();

//        /// <summary>
//        /// The machine on which the GCode will be running
//        /// </summary>
//        private CNCMachine CNCMachine;

//        /// <summary>
//        /// The parameters to build the g code program
//        /// </summary>
//        private PartParameters parameters;

//        /// <summary>
//        /// Class constructor
//        /// </summary>
//        /// <param name="parameters">A valid parameter object</param>
//        public OnsrudGCodeBuilder(CNCMachine machine, PartParameters parameters)
//        {
//            this.CNCMachine = machine;
//            this.parameters = parameters;
//        }

public GCodeFile Build(Part p) { return new GCodeFile(); }
//        {
//            Append(CNCMachine.Header(parameters));
//            Append(CNCMachine.ToolChange(12));

//            float verticalStep = GetStep();

//            float z = parameters.StartingThickness;
//            while (z >= parameters.FinishedThickness)
//            {
//                float x = 0.0, y = 0.0;
//                Append(CNCMachine.RapidToXYZ(x, y, parameters.StartingThickness + 1.0));
//                Append(CNCMachine.FeedToZ(z));

//                while (x < parameters.PartWidth)
//                {
//                    Append(CNCMachine.FeedToY(parameters.PartLength));
//                    x += CNCMachine.SelectedTool.Diameter - 0.25;
//                    Append(CNCMachine.FeedToX(x));
//                    Append(CNCMachine.FeedToY(0.0));
//                    x += CNCMachine.SelectedTool.Diameter - 0.25;
//                    Append(CNCMachine.FeedToX(x));
//                }
//                Append(CNCMachine.LiftHead());
//                z = Math.Round(z - verticalStep, 3);
//            }
//            Append(CNCMachine.EndProgram());
//            return new("");
//        }

//        public async void Save()
//        {
//            SaveFileDialog saveFileDialog = new()
//            {
//                Title = "Save G Code File",
//                DefaultDirectory = $"C:\\users\\{Environment.UserName}\\DropBox\\",
//                AddExtension = true,
//                DefaultExt = ".anc",
//                FileName = $"{parameters.PartName}.anc"
//            };
//            if (saveFileDialog.ShowDialog() == true)
//            {
//                FileStream stream;
//                int i = 0;
//                try
//                {
//                    byte[] bufferToWrite = new byte[gCodeString.Length];
//                    foreach (char c in gCodeString.ToString())
//                    {
//                        bufferToWrite[i++] = (byte)c;
//                    }
//                    stream = File.Create(saveFileDialog.FileName);
//                    await stream.WriteAsync(bufferToWrite);
//                    stream.Close();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message);
//                }
//                System.Diagnostics.Process.Start(Settings.PathToEdytorNC, saveFileDialog.FileName);
//                Application.Current.Shutdown();
//            }
//        }

//        public void Send()
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Calculate the amount of Z step for each pass
//        /// </summary>
//        /// <returns></returns>
//        private float GetStep()
//        {
//            float heightToCut = parameters.StartingThickness - parameters.FinishedThickness;
//            float roughCount = heightToCut / parameters.VerticalStep;
//            int actualCountOfPasses = (int)Math.Ceiling(roughCount);
//            return Math.Round(heightToCut / actualCountOfPasses, 3);
//        }

//        private void Append(string[] lines)
//        {
//            foreach (string line in lines)
//                gCodeString.AppendLine(line);
//        }

//        private void Append(string line)
//        {
//            gCodeString.AppendLine(line);
//        }
}
