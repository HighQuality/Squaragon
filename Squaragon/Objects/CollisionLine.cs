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
    class CollisionLine : GameObject, IPlayerCollision
    {
        public float Radius
        {
            get
            {
                return Line.Length;
            }
        }
        public Line Line;
        
        public CollisionLine()
        {

        }

        public void Start(Line line)
        {
            Line = line;
            var pixel = Resources.GetTexture("pixel");
            SpriteComponent sprite = SpriteComponent.RegisterOn(this, pixel);
            sprite.Origin = new Vector2(0.5f, 0.5f);
            LocalRotation = Line.Angle;
            sprite.Scale = new Vector2(Line.Length, 3f);
            sprite.Color = Color.Black;
            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
            WorldCoord = line.Middle;
        }

        protected void PhysicsUpdate(PhysicsUpdateEvent ev)
        {

        }

        public bool CheckCollisionWith(PhysicsObject other)
        {
            //TODO: COLLISION
            return false;
        }

        public void OnCollisionWithPlayer(Player player)
        {
            player.Velocity *= -1;
        }
    }
}
