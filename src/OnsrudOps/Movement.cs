
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.src
{
    internal readonly struct Movement(Vector3 target, int feedRate = 320, MovementType movementType = MovementType.Feed)
    {
        /// <summary>
        /// The target X,Y,Z coordinate for this movement.
        /// </summary>
        public readonly Vector3 Target { get; } = target;
        
        /// <summary>
        /// The movement type.
        /// </summary>
        public readonly MovementType MovementType { get; } = movementType;

        /// <summary>
        /// The feed rate for this movemnt.
        /// </summary>
        public readonly int FeedRate { get; } = feedRate;

        public static Movement PositioningMovement(float x, float y, float z)
        {
            return new Movement(new Vector3(x, y, z), 0, MovementType.Rapid);
        }

        public static Movement Home => new Movement(new Vector3(48, 0, 8), 0, MovementType.Rapid);
    }
}
