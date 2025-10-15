using System.Drawing;
using System.Numerics;

namespace OnsrudOps.src
{
    abstract class Operation(Part parentPart, Tool tool, PointF position)
    {
        protected Part _parentPart = parentPart;
        protected List<Movement> _movements = [];
        protected float _lastZ;
        protected int _lastFeed;

        /// <summary>
        /// The tool used for this operation.
        /// </summary>
        public Tool Tool { get; set; } = tool;

        /// <summary>
        /// The tool movements.
        /// </summary>
        public List<Movement> Movements => _movements;

        /// <summary>
        /// The position of the operation on the part
        /// </summary>
        public PointF Position { get; set; } = position;

        /// <summary>
        /// Operation Width
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Operation Height
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Operation Depth
        /// </summary>
        public float Depth { get; set; }

        protected void Move(float x, float y, float z, int feed, MovementType type)
        {
            Vector3 v = new(x, y, z);
            _movements.Add(new Movement(v, feed, type));
            _lastZ = z;
            _lastFeed = feed;
        }

        protected void Move(float x, float y, float z, int feed)
        {
            Vector3 v = new(x, y, z);
            _movements.Add(new Movement(v, feed));
            _lastZ = z;
            _lastFeed = feed;
        }

        protected void Move(float x, float y, float z)
        {
            Vector3 v = new(x, y, z);
            _movements.Add(new Movement(v, _lastFeed));
            _lastZ = z;
        }

        protected void Move(float x, float y)
        {
            Vector3 v = new(x, y, _lastZ);
            _movements.Add(new Movement(v, _lastFeed));
        }
    }
}
