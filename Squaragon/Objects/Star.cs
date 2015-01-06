using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Modules.Resources;

namespace Squaragon.Objects
{
    [Resource(ContainerName = "main", Filename = "Textures/happystar.png", Key = "star")]
    class Star : GameObject, IPlayerCollision
    {
        public float Radius { get; set; }
        private float offset;

        public Star()
        {
            Radius = 10f;

            WorldCoord = new Vector2((int)WorldCoord.X, (int)WorldCoord.Y);
            var star = Resources.GetTexture("star");
            var main = SpriteComponent.RegisterOn(this, star);
            main.Color = Color.Yellow;
            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);

            OnRemoved += Star_OnRemoved;

            offset = Engine.RandomFloat()*200f;
        }

        void Star_OnRemoved()
        {
            Scene.CreateObject<Star>(new Vector2((Engine.RandomFloat() - 0.5f) * Engine.Resolution.X, (Engine.RandomFloat() - 0.5f) * Engine.Resolution.Y));
        }

        void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            LocalRotation = Angle.FromDegree((Mathf.Sin((float)Engine.TimeStamp * 4f + offset)) * 45f);
            LocalScale = Vector2.One * (.75f + (Mathf.Sin((float)Engine.TimeStamp * 2f + offset) + 1f) / 2f * .25f);
        }

        public void OnCollisionWithPlayer(Player player)
        {
            Remove();
        }
    }
}
