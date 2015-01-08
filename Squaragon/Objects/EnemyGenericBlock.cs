using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Objects
{
    class EnemyGenericBlock : PhysicsObject, IPlayerCollision
    {
        public EnemyGenericBlock() 
            : base(Vector2.Zero, new Color(155, 89, 182))
        {

        }

        public void Start(Vector2 size, Vector2 velocity, float gravity)
        {
            SetSize(size);
            Velocity = velocity;
            Gravity = gravity;
            LocalRotation = velocity.Angle;
        }

        public void OnCollisionWithPlayer(Player player)
        {
            player.Remove();
        }
    }
}
