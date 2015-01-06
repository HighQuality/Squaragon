using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog;

namespace Squaragon.Objects
{
    class EnemyBlock : PhysicsObject, IPlayerCollision
    {
        public EnemyBlock()
            : base(new Vector2(Engine.RandomFloat() * 100 + 50, Engine.RandomFloat() * 100 + 50), new Color(155, 89, 182))
        {
            Gravity = 0;
        }

        protected override void PhysicsUpdate(Cog.Modules.EventHost.PhysicsUpdateEvent ev)
        {
            if (WorldCoord.Length >= 1280f)
            {
                Remove();
            }

            base.PhysicsUpdate(ev);
        }

        public void OnCollisionWithPlayer(Player player)
        {
            player.Remove();
        }
    }
}
