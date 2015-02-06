using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Objects
{
    public struct Line
    {
        public Vector2 FirstPoint,
            SecondPoint;
        public float Length
        {
            get
            {
                return ((FirstPoint - SecondPoint).Length);
            }
        }
        public Angle Angle
        {
            get
            {
                return (FirstPoint - SecondPoint).Angle;
            }
        }

        public Vector2 Middle
        {
            get
            {
                return (FirstPoint + SecondPoint) / 2;
            }
        }
    }
}
