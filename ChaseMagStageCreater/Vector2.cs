using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaseMagStageCreater
{
    public struct Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public static readonly Vector2 zero = new Vector2(0.0f, 0.0f);
        public static readonly Vector2 one = new Vector2(1.0f, 1.0f);

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
