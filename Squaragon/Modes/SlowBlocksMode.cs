using Cog;
using Cog.Modules.Content;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    class SlowBlocksMode : Mode
    {
        public SlowBlocksMode(MainScene scene)
            : base(scene)
        {
            Difficulty = 10;
            EnterUpdate();
        }

        private void EnterUpdate()
        {
            Action<float> spawnEnemy = null;
            TimedEvent ev = null;
            float roll = Engine.RandomFloat();
            Vector2 direction;
            if(roll < 0.25f) direction = Vector2.Up;
            else if (roll < 0.5f) direction = Vector2.Right;
            else if (roll < 0.75f) direction = Vector2.Down;
            else direction = Vector2.Left;

            spawnEnemy = offset =>
            {
                const float spawnDistance = 32f;
                EnemyGenericBlock enemy;
                if (direction == Vector2.Up) //down -> up
                {
                    enemy = Scene.CreateObject<EnemyGenericBlock>(new Vector2((Engine.RandomFloat()-0.5f) * Engine.Resolution.X, -Engine.Resolution.Y / 2f - spawnDistance));
                    enemy.Velocity = new Vector2(0f, 64f * Mathf.Pow(1.05f, Difficulty));
                }
                else if(direction == Vector2.Right) //left -> right
                {
                    enemy = Scene.CreateObject<EnemyGenericBlock>(new Vector2(-Engine.Resolution.X - spawnDistance, (Engine.RandomFloat() - 0.5f)*Engine.Resolution.Y));
                    enemy.Velocity = new Vector2(64f * Mathf.Pow(1.05f, Difficulty), 0f);
                }
                else if (direction == Vector2.Down) //up -> down
                {
                    enemy = Scene.CreateObject<EnemyGenericBlock>(new Vector2((Engine.RandomFloat() - 0.5f) * Engine.Resolution.X, +Engine.Resolution.Y / 2f + spawnDistance));
                    enemy.Velocity = new Vector2(0f, -64f * Mathf.Pow(1.05f, Difficulty));
                }
                else      //right -> left
                {
                    enemy = Scene.CreateObject<EnemyGenericBlock>(new Vector2(Engine.Resolution.X + spawnDistance, (Engine.RandomFloat() - 0.5f) * Engine.Resolution.Y));
                    enemy.Velocity = new Vector2(-64f * Mathf.Pow(1.05f, Difficulty), 0f);
                }

                enemy.Start(new Vector2(32f, 16f), enemy.Velocity, 0f);

                ev = Engine.InvokeTimed(0.7f * Mathf.Pow(0.88f, Difficulty) - offset, spawnEnemy);
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
