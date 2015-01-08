using Cog;
using Cog.Modules.Content;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    class FlyingBlocksMode : Mode
    {
        float baseSpawnTime = 1.5f;
        public FlyingBlocksMode(MainScene scene)
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
                float roll = Engine.RandomFloat();

                if (roll <= .25f)
                {
                    // Top
                    var enemy = Scene.CreateObject<EnemyBlock>(new Vector2(Engine.RandomFloat() * Scene.PlayableArea.Width + Scene.PlayableArea.Left, -Engine.Resolution.Y - 100));
                    enemy.Velocity = new Vector2(0f, 128 + 10 * Difficulty);
                }
                else if (roll <= .5f)
                {
                    // Bottom
                    var enemy = Scene.CreateObject<EnemyBlock>(new Vector2(Engine.RandomFloat() * Scene.PlayableArea.Width + Scene.PlayableArea.Left, Engine.Resolution.Y + 100));
                    enemy.Velocity = new Vector2(0f, -128 - 10 * Difficulty);
                }
                else if (roll <= .75f)
                {
                    // Left Side
                    var enemy = Scene.CreateObject<EnemyBlock>(new Vector2(-Engine.Resolution.X - 100, Engine.RandomFloat() * Scene.PlayableArea.Height + Scene.PlayableArea.Top));
                    enemy.Velocity = new Vector2(128 + 10 * Difficulty, 0f);
                }
                else
                {
                    // Right Side
                    var enemy = Scene.CreateObject<EnemyBlock>(new Vector2(Engine.Resolution.X + 100, Engine.RandomFloat() * Scene.PlayableArea.Height + Scene.PlayableArea.Top));
                    enemy.Velocity = new Vector2(-128 - 10 * Difficulty, 0f);
                }
                ev = Engine.InvokeTimed(baseSpawnTime * (float)Math.Pow(0.95f, Difficulty) - offset, spawnEnemy);
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
