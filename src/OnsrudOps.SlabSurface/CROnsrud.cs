using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.SlabSurface;

/// <summary>
/// Implementation for CR Onsrud CNC machine
/// </summary>
class CROnsrud : CNCMachine
{
    /// <summary>
    /// List of available tools for this machine
    /// </summary>
    private static List<Tool> toolList =
    [
        new Tool(12, 2.0, 12000, 320.0)
    ];

    /// <summary>
    /// List of available tools for this machine
    /// </summary>
    protected override List<Tool> Tools
    {
        get { return toolList; }
    }

    /// <summary>
    /// Return a string representing a header to a g code file
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public override string[] Header(Parameters parameters)
    {
        return [
            $"%",
            $"//=================== INFO ===================//",
            $"// JOB NAME: {parameters.ProgramName}",
            $"// OUTPUT TIME: {DateTime.Now}",
            $"// SHEET SIZE: {parameters.PartWidth} x {parameters.PartLength} x {parameters.FinishedThickness}",
            $"// START PROGRAM",
            $"P{parameters.ProgramName.Replace(' ', '_')}",
            $"G90 G70 G40 G49 G56"
        ];
    }

    /// <summary>
    /// Defines a tool change operation
    /// </summary>
    /// <remarks>
    /// Side effect: changes the machines currently selected tool
    /// </remarks>
    /// <param name="tool"></param>
    /// <returns></returns>
    public override string[] ToolChange(int toolNumber)
    {
        SetTool(toolNumber);
        return [
            $"//------------TOOL CHANGE------------",
            $"M06 T{SelectedTool.Number}",
            $"M03 S{SelectedTool.RPM}"
        ];
    }

    public override string[] EndProgram()
    {
        return [
            "M20 A1+",
            "M02",
            "%"
        ];
    }
}
