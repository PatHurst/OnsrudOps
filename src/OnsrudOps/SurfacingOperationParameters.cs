using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.src
{
    internal class SurfacingOperationParameters
    {
        private string _operationName = "Surface";
        private float _operationWidth = 12.0f;
        private float _operationLength = 12.0f;
        private float _operationThickness = 1.0f;
        private float _verticalStep = 0.06f;

        public SurfacingOperationParameters() { }

        /// <summary>
        /// The name of the Operation
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
        }

        /// <summary>
        /// The width of the Operation
        /// </summary>
        public float OperationWidth
        {
            get { return _operationWidth; }
        }

        /// <summary>
        /// The length of the Operation
        /// </summary>
        public float OperationLength
        {
            get { return _operationLength; }
        }

        /// <summary>
        /// The thickness of the Operation
        /// </summary>
        public float OperationThickness
        {
            get { return _operationThickness; }
        }

        public float VerticalStep => _verticalStep;

        /// <summary>
        /// Build a parameters object from the provided string values
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="operationWidth"></param>
        /// <param name="operationLength"></param>
        /// <param name="operationThickness"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Build(string operationName, string operationWidth, string operationLength, string operationThickness, string verticalStep)
        {
            _operationName = operationName;
            bool[] success =
            [
                float.TryParse(operationWidth, out _operationWidth),
                float.TryParse(operationLength, out _operationLength),
                float.TryParse(operationThickness, out _operationThickness),
                float.TryParse(verticalStep, out _verticalStep)
            ];
            if (success.Contains(false))
                throw new ArgumentException("Invalid Value");
        }
    }
}
