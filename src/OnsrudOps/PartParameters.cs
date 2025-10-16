using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Media.Converters;

namespace OnsrudOps.src;

/// <summary>
/// Holds the parameters necessary to construct a Surfacing Program.
/// </summary>
internal class PartParameters
{
    private string _partName = "Program";
    private float _partWidth = 48.0f;
    private float _partLength = 96.0f;
    private float _partThickness = 0.75f;

    public PartParameters() { }

    /// <summary>
    /// The name of the part
    /// </summary>
    public string PartName
    {
        get { return _partName; }
    }

    /// <summary>
    /// The width of the part
    /// </summary>
    public float PartWidth
    {
        get { return _partWidth; }
    }

    /// <summary>
    /// The length of the part
    /// </summary>
    public float PartLength
    {
        get { return _partLength; }
    }

    /// <summary>
    /// The thickness of the part
    /// </summary>
    public float PartThickness
    {
        get { return _partThickness; }
    }

    /// <summary>
    /// Build the parameter with the provided string arguments.
    /// Throws an exception if the string cannot be converted to floats.
    /// </summary>
    /// <param name="partName"></param>
    /// <param name="partWidth"></param>
    /// <param name="partLength"></param>
    /// <param name="partThickness"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Build(string partName, string partWidth, string partLength, string partThickness)
    {
        _partName = partName;
        bool[] success =
        [
            float.TryParse(partWidth, out _partWidth),
            float.TryParse(partLength, out _partLength),
            float.TryParse(partThickness, out _partThickness),
        ];
        if (success.Contains(false))
            throw new ArgumentException("Invalid Value");
    }
}
