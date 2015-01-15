using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog.Modules.EventHost;

namespace Squaragon.Objects
{
    class EnemyAttackingWall : EnemyGenericBlock
    {
        Vector2 attackPoint;
        float minDistance;
        float velocityMultiplier;
        public EnemyAttackingWall()
            :base()
        {

        }

        public void Start(Vector2 size, Vector2 velocity, float gravity, Vector2 attackPoint, float minDistance)
        {
            Start(size, velocity, gravity);
            this.attackPoint = attackPoint;
            this.minDistance = minDistance;
        }

        protected override void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            float distanceToGoal = (attackPoint - WorldCoord).Length;
            if (distanceToGoal < minDistance)
            {
                Remove();
            }
            velocityMultiplier = Math.Min(1, distanceToGoal / 200);
            Vector2 tempVelocity = Velocity;
            Velocity *= velocityMultiplier;
            base.PhysicsUpdate(ev);
            Velocity = tempVelocity;
        }
    }
}
