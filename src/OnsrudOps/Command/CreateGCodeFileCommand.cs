using OnsrudOps.src;

namespace OnsrudOps.Command
{
    class CreateGCodeFileCommand : ICommand
    {
        private IGCodeBuilder _codeBuilder;
        private CommandType _commandType;

        public CreateGCodeFileCommand(PartParameters parameters, CommandType commandType)
        {
            _codeBuilder = new OnsrudGCodeBuilder(); ;
            _commandType = commandType;
        }

        public void Execute()
        {
            _codeBuilder.Build(new Part(new PartParameters()));
            //switch (_commandType)
            //{
            //    case CommandType.SaveToDisk:
            //    _codeBuilder.Save();
            //    break;
            //    case CommandType.SendToSerialPort:
            //    _codeBuilder.Send();
            //    break;
            //    default:
            //    throw new ArgumentException("Command type not supported!");
            //}
        }
    }
}
