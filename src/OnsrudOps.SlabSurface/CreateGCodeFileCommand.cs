namespace OnsrudOps.SlabSurface;

public class CreateGCodeFileCommand
{
    private IGCodeBuilder _codeBuilder;

    public CreateGCodeFileCommand(Parameters parameters)
    {
        _codeBuilder = new GCodeBuilder(CNCMachine.Build(CNCType.CROnsrud), parameters);
    }

    public string BuildFile()
    {
        return _codeBuilder.Build(); 
    }
}
