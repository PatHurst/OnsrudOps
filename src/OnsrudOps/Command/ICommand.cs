using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.Command
{
    interface ICommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        public void Execute();
    }
}
