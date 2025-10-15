using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.src
{
    interface IGCodeBuilder
    {
        /// <summary>
        /// Build the GCode file from a part object
        /// </summary>
        public GCodeFile Build(Part part);
    }
}
