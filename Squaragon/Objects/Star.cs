using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Modules.Resources;
using Microsoft.SqlServer.Server;

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
            Scene.CreateObject<Star>(Program.Scene.RandomizePlayablePosition());
        }

        void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            var player = Scene.EnumerateObjects<Player>().OrderBy(o => (o.WorldCoord - WorldCoord).Length).FirstOrDefault();
            if (player != null)
            {
                if ((player.WorldCoord - WorldCoord).Length < 64f)
                {
                    LocalCoord += (player.WorldCoord - WorldCoord).Unit * 200f * ev.DeltaTime;
                }
            }
            
            LocalRotation = Angle.FromDegree((Mathf.Sin((float)Engine.TimeStamp * 4f + offset)) * 45f);
            LocalScale = Vector2.One * (.75f + (Mathf.Sin((float)Engine.TimeStamp * 2f + offset) + 1f) / 2f * .25f);
        }

        public void OnCollisionWithPlayer(Player player)
        {
            Program.Scene.Multiplier++;
            Remove();
        }
    }
}
