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
            if ((attackPoint - WorldCoord).Length < minDistance)
            {
                Remove();
            }
            base.PhysicsUpdate(ev);
        }
    }
}
