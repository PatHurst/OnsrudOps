using OnsrudOps.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OnsrudOps.ReCut;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.UI;

/// <summary>
/// Provides the data for the UI
/// </summary>
internal class ViewModel
{
    /// <summary>
    /// A list of the code files opened in the editor
    /// </summary>
    public ObservableCollection<GCodeFile> CodeFiles = [];

    /// <summary>
    /// The currently opened file
    /// </summary>
    public GCodeFile CurrentFile { get; set; } = GCodeFile.EmptyFile;

    public PartParameters PartParameters { get; set; } = new();

    public Parameters Parameters { get; set; } = new();

    public SurfacingOperationParameters OperationParameters { get; set; } = new();

    public string Test { get; set; } = "Foo";
}
