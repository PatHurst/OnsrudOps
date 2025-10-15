using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace OnsrudOps.ReCut;

class GCodeBuilder : IGCodeBuilder
{
    /// <summary>
    /// The actual file string
    /// </summary>
    private StringBuilder gCodeString = new();

    /// <summary>
    /// The machine on which the GCode will be running
    /// </summary>
    private CNCMachine CNCMachine;

    /// <summary>
    /// The parameters to build the g code program
    /// </summary>
    private Parameters parameters;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="parameters">A valid parameter object</param>
    public GCodeBuilder(CNCMachine machine, Parameters parameters)
    {
        this.CNCMachine = machine;
        this.parameters = parameters;
    }

    public string Build()
    {
        Append(CNCMachine.Header(parameters));
        Append(CNCMachine.ToolChange(12));

        double verticalStep = GetStep();

        double z = parameters.StartingThickness;
        while (z >= parameters.FinishedThickness)
        {
            double x = 0.0, y = 0.0;
            Append(CNCMachine.RapidToXYZ(x, y, parameters.StartingThickness + 1.0));
            Append(CNCMachine.FeedToZ(z));

            while (x < parameters.PartWidth)
            {
                Append(CNCMachine.FeedToY(parameters.PartLength));
                x += CNCMachine.SelectedTool.Diameter - 0.25;
                Append(CNCMachine.FeedToX(x));
                Append(CNCMachine.FeedToY(0.0));
                x += CNCMachine.SelectedTool.Diameter - 0.25;
                Append(CNCMachine.FeedToX(x));
            }
            Append(CNCMachine.LiftHead());
            z = Math.Round(z - verticalStep, 3);
        }
        Append(CNCMachine.EndProgram());

        return gCodeString.ToString();
    }

    /// <summary>
    /// Calculate the amount of Z step for each pass
    /// </summary>
    /// <returns></returns>
    private double GetStep()
    {
        double heightToCut = parameters.StartingThickness - parameters.FinishedThickness;
        double roughCount = heightToCut / parameters.VerticalStep;
        int actualCountOfPasses = (int)Math.Ceiling(roughCount);
        return Math.Round(heightToCut / actualCountOfPasses, 3);
    }

    private void Append(string[] lines)
    {
        foreach (string line in lines)
            gCodeString.AppendLine(line);
    }

    private void Append(string line)
    {
        gCodeString.AppendLine(line);
    }
}
