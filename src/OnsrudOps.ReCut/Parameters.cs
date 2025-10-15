namespace OnsrudOps.ReCut;

/// <summary>
/// Holds the parameters necessary to construct a Surfacing Program.
/// Serves as data binding source for UI
/// </summary>
public class Parameters
{
    private string _programName = "Slab Program";
    private double _partWidth = 48.0;
    private double _partLength = 96.0;
    private double _startingThickness = 3.0;
    private double _finishedThickness = 2.0;
    private double _verticalStep = 0.05;

    public Parameters() { }

    /// <summary>
    /// The name of the program
    /// </summary>
    public string ProgramName
    {
        get { return _programName; }
    }

    /// <summary>
    /// The width of the part
    /// </summary>
    public double PartWidth
    {
        get { return _partWidth; }
    }

    /// <summary>
    /// The length of the part
    /// </summary>
    public double PartLength
    {
        get { return _partLength; }
    }

    /// <summary>
    /// The thickness to begin cutting
    /// </summary>
    public double StartingThickness
    {
        get { return _startingThickness; }
    }

    /// <summary>
    /// The desired finished thickness
    /// </summary>
    public double FinishedThickness
    {
        get { return _finishedThickness; }
    }

    /// <summary>
    /// The amount the tool removes with each pass
    /// </summary>
    public double VerticalStep
    {
        get { return _verticalStep; }
    }

    public void Build(string programName, string partWidth, string partLength, string startingThickness, string finishedThickness, string verticalStep)
    {
        _programName = programName;
        bool[] success =
        [
            double.TryParse(partWidth, out _partWidth),
            double.TryParse(partLength, out _partLength),
            double.TryParse(startingThickness, out _startingThickness),
            double.TryParse(finishedThickness, out _finishedThickness),
            double.TryParse(verticalStep, out _verticalStep)
        ];
        if (success.Contains(false))
            throw new ArgumentException("Invalid Value");
    }
}
