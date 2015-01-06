using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Objects
{
    [Resource(ContainerName = "main", Filename = "Textures/pixel.png", Key = "pixel")]
    class PhysicsObject : GameObject
    {
        public float Radius { get; private set; }
        public Vector2 Velocity;
        public float Gravity = 200f;
        public double CreationTime = Engine.TimeStamp;
        public float Age { get { return (float)(Engine.TimeStamp - CreationTime); } }

        public PhysicsObject(Vector2 size, Color color)
        {
            const float outlineThickness = 1f;
            var pixel = Resources.GetTexture("pixel");

            var outline = SpriteComponent.RegisterOn(this, pixel);
            outline.Scale = size + new Vector2(outlineThickness * 2f, outlineThickness * 2f);
            outline.Origin = new Vector2(0.5f, 0.5f);
            outline.Color = new Color(44, 62, 80);

            var sprite = SpriteComponent.RegisterOn(this, pixel);
            sprite.Scale = size;
            sprite.Origin = new Vector2(0.5f, 0.5f);
            sprite.Color = color; // new Color(52, 152, 219);

            Radius = Mathf.Min(size.X, size.Y) / 2f;

            LocalScale = Vector2.Zero;

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
        }

        protected virtual void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            Velocity.Y += Gravity * ev.DeltaTime;
            LocalScale = new Vector2(1f, 1f) * Mathf.Min(Age, 1f);

            LocalCoord += Velocity * ev.DeltaTime;
        }
    }
}
