using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.src
{
    internal class Part(PartParameters parameters)
    {
        private List<Operation> operations = [];
        private PartParameters _partParameters = parameters;

        public float PartWidth => _partParameters.PartWidth;
        public float PartLength => _partParameters.PartLength;
        public float PartThickness => _partParameters.PartThickness;

        public void AddOperation(Operation operation) => operations.Add(operation);
    }
}
