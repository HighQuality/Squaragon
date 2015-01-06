using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog;
using Cog.Modules.Content;
using Cog.Modules.Resources;

namespace Squaragon.Objects
{
    [Resource(ContainerName = "main", Filename = "Textures/pixel.png", Key = "pixel")]
    class Arrow : GameObject
    {
        const float thickness = 2f;
        private Node right, left;

        public Arrow()
        {
            var pixel = Resources.GetTexture("pixel");
            var middle = SpriteComponent.RegisterOn(this, pixel);
            middle.Origin = new Vector2(1f, 0.5f);
            middle.Color = Color.Black;

            left = Scene.CreateLocalObject<Node>(this, new Vector2(0f, 0f));
            var leftLine = SpriteComponent.RegisterOn(left, pixel);
            leftLine.Origin = new Vector2(1f, 0.5f);
            leftLine.Color = Color.Black;
            leftLine.Scale = new Vector2(0.5f, 1);
            left.LocalRotation = Angle.FromDegree(35f);

            right = Scene.CreateLocalObject<Node>(this, new Vector2(0f, 0f));
            var rightLine = SpriteComponent.RegisterOn(right, pixel);
            rightLine.Origin = new Vector2(1f, 0.5f);
            rightLine.Color = Color.Black;
            rightLine.Scale = new Vector2(0.5f, 1);
            right.LocalRotation = Angle.FromDegree(-35f);
        }

        public void Update(Vector2 deltaPosition)
        {
            WorldRotation = deltaPosition.Angle;
            LocalScale = new Vector2(deltaPosition.Length * 100, thickness);
            left.LocalRotation = Angle.FromDegree(-45 + 25 * (deltaPosition.Length * 2));
            right.LocalRotation = Angle.FromDegree(45 - 25 * (deltaPosition.Length * 2));

        }
    }
}
