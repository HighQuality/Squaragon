using Cog;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    class AdventureMode : Mode
    {
        Line[] CollisionLines = new Line[] {new Line
        {
            FirstPoint = new Vector2(100f, 100f), SecondPoint = new Vector2(200f, 300f),
        } };
        public AdventureMode(MainScene scene)
            :base(scene)
        {
            foreach(Line line in CollisionLines)
            {
                var temp = Scene.CreateObject<CollisionLine>(new Vector2(0f, 0f));
                temp.Start(line);
            }
        }

        private void EnterUpdate()
        {

        }
    }
}
