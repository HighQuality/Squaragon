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
        public SpriteComponent Sprite;

        public Vector2 Velocity;

        public float Gravity = 400f;

        public PhysicsObject(Vector2 size, Color color)
        {
            const float outlineThickness = 1f;
            var pixel = Resources.GetTexture("pixel");

            Sprite = SpriteComponent.RegisterOn(this, pixel);
            Sprite.Scale = size + new Vector2(outlineThickness * 2f, outlineThickness * 2f);
            Sprite.Origin = new Vector2(0.5f, 0.5f);
            Sprite.Color = new Color(44, 62, 80);

            Sprite = SpriteComponent.RegisterOn(this, pixel);
            Sprite.Scale = size;
            Sprite.Origin = new Vector2(0.5f, 0.5f);
            Sprite.Color = color; // new Color(52, 152, 219);

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
        }

        protected virtual void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            Velocity.Y += Gravity * ev.DeltaTime;

            LocalCoord += Velocity * ev.DeltaTime;
        }
    }
}
