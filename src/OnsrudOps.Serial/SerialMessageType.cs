using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.Serial;

/// <summary>
/// The type of serial message
/// </summary>
public enum SerialMessageType
{
    /// <summary>
    /// General Information
    /// </summary>
    Information,
    /// <summary>
    /// Serial Error
    /// </summary>
    Error,
}