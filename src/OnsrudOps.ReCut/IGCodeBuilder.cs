using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.ReCut;

interface IGCodeBuilder
{
    /// <summary>
    /// Build the GCode file base on input parameters
    /// </summary>
    public string Build();

}
