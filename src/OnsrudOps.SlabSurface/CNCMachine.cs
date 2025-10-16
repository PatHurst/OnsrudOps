using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.SlabSurface;

/// <summary>
/// Abstract class providing the base functionality for all CNC machines
/// </summary>
abstract class CNCMachine
{
    /// <summary>
    /// The currently selected tool
    /// </summary>
    private Tool _selectedTool;

    /// <summary>
    /// Class constructor returns the defualt tool
    /// </summary>
    public CNCMachine()
    {
        _selectedTool = Tools.First();
    }
    
    /// <summary>
    /// The currently selected tool for the machine
    /// </summary>
    public Tool SelectedTool
    {
        get { return _selectedTool; }
    }

    /// <summary>
    /// List of avalable tools for this machine
    /// </summary>
    protected abstract List<Tool> Tools
    {
        get;
    }

    /// <summary>
    /// Set the currently selected tool by tool number
    /// </summary>
    /// <param name="toolNumber"></param>
    /// <exception cref="ArgumentException"></exception>
    protected virtual void SetTool(int toolNumber)
    {
        if (Tools.Find(t => t.Number == toolNumber) is Tool tool)
            _selectedTool = tool;
        else
            throw new ArgumentException("Tool Number is not valid!");
    }

    /// <summary>
    /// Builder method to return a CNC object
    /// </summary>
    /// <param name="type"></param>
    /// <returns>A CNC object, of a type derived from this class</returns>
    /// <exception cref="ArgumentException">Throws if enum type is not supported in switch expression</exception>
    public static CNCMachine Build(CNCType type)
    {
        switch (type)
        {
            case CNCType.CROnsrud:
            return new CROnsrud();
            default:
            throw new ArgumentException("CNC type not supported in builder method!");
        }
    }

    #region Machine Actions

    public abstract string[] Header(Parameters parameters);

    public abstract string[] ToolChange(int toolNumber);

    public abstract string[] EndProgram();

    public virtual string FeedToXYZ(double x, double y, double z) => $"G01 X{x} Y{y} Z{z} F{SelectedTool.FeedRate}";

    public virtual string RapidToXYZ(double x, double y, double z) => $"G00 X{x} Y{y} Z{z}";

    public virtual string FeedToXY(double x, double y) => $"G01 X{x} Y{y} F{SelectedTool.FeedRate}";

    public virtual string FeedToXZ(double x, double z) => $"G01 X{x} Z{z} F{SelectedTool.FeedRate}";

    public virtual string FeedToYZ(double y, double z) => $"G01 Y{y} Z{z} F{SelectedTool.FeedRate}";

    public virtual string FeedToX(double x) => $"G01 X{x} F{SelectedTool.FeedRate}";

    public virtual string FeedToY(double y) => $"G01 Y{y} F{SelectedTool.FeedRate}";

    public virtual string FeedToZ(double z, double feed = 320.0) => $"G01 Z{z} F{SelectedTool.FeedRate}";

    public virtual string LiftHead() => "G00 Z8.0";

    public virtual string[] GoToHome() => ["G00 Z8.0", "G00 X0.0 Y0.0 Z8.0"];

    #endregion

}
