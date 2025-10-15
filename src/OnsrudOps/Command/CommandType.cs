using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.Command
{
    /// <summary>
    /// Action to execute with G Code file
    /// </summary>
    enum CommandType
    {
        SendToSerialPort,
        SaveToDisk
    }
}
