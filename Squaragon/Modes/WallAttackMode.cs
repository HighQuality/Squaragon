using Squaragon.Objects;
using Cog;
using Cog.Modules.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    class WallAttackMode : Mode
    {
        float baseSpawnTime = 5.0f;
        public WallAttackMode(MainScene scene)
            : base(scene)
        {
            EnterUpdate();
        }


        private void EnterUpdate()
        {
            Action<float> spawnEnemy = null;
            TimedEvent ev = null;

            spawnEnemy = offset =>
            {
                Vector2 attackPoint = new Vector2((Engine.RandomFloat() - 0.5f) * Scene.PlayableArea.Width, 
                    (Engine.RandomFloat() - 0.5f) * Scene.PlayableArea.Height);
                float angle = Engine.RandomFloat() * 360;

                var first = Scene.CreateObject<EnemyAttackingWall>(attackPoint + Vector2.Up.Rotate(Angle.FromDegree(angle)) * 1500);
                var second = Scene.CreateObject<EnemyAttackingWall>(attackPoint + Vector2.Up.Rotate(Angle.FromDegree(angle - 180)) * 1500);
                first.Start(new Vector2(40, 2000), Vector2.Down.Rotate(Angle.FromDegree(angle)) * (200 + 10 * Difficulty), 0f,
                    attackPoint, 90 * (float)Math.Pow(0.95f, Difficulty));
                second.Start(new Vector2(40, 2000), Vector2.Down.Rotate(Angle.FromDegree(angle - 180)) * (200 + 10 * Difficulty), 0f, 
                    attackPoint, 90 * (float)Math.Pow(0.95f, Difficulty));
                ev = Engine.InvokeTimed(baseSpawnTime * (float)Math.Pow(0.91f, Difficulty) - offset, spawnEnemy);
            };

            spawnEnemy(0f);
            ChangeState((dt) =>
            {

            });

            onStateChanged = () =>
            {
                if (ev != null)
                    ev.Cancel();
            };
        }
    }
}
