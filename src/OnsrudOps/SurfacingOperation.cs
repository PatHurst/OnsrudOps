
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.src;

internal class SurfacingOperation : Operation
{
    public SurfacingOperation(Part parent, Tool tool, PointF position, SurfacingOperationParameters operationParameters)
        :base(parent, tool, position)
    {
        BuildSurfacingOperation(operationParameters);
    }

    private void BuildSurfacingOperation(SurfacingOperationParameters param)
    {
        float step = GetStep(param);

        float z = param.OperationThickness + _parentPart.PartThickness - step;
        while (z > _parentPart.PartThickness)
        {
            //position head above start position
            Move(Position.X, Position.Y, param.OperationThickness + _parentPart.PartThickness + 1.0f, 320, MovementType.Rapid);
            // enter cutting range
            Move(Position.X, Position.Y, z);

            float x = Position.X, y = Position.Y;
            while (x <= Width)
            {

            }

        }

    }

    private float GetStep(SurfacingOperationParameters parameters)
    {
        float roughCount = parameters.OperationThickness / parameters.VerticalStep;
        return MathF.Round(parameters.OperationThickness / (int)Math.Ceiling(roughCount), 3);
    }
}
