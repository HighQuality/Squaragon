using Cog;
using Cog.Modules.Content;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Modes
{
    class StandardMode : Mode
    {

        public StandardMode(MainScene scene)
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
                    Scene.CreateObject<Enemy>(new Vector2(Engine.RandomFloat() * Engine.Resolution.X - Engine.Resolution.X / 2f, -Engine.Resolution.Y / 2f));
                }
                else if (roll <= .5f)
                {
                    // Bottom
                    var enemy = Scene.CreateObject<Enemy>(new Vector2(Engine.RandomFloat() * Engine.Resolution.X - Engine.Resolution.X / 2f, Engine.Resolution.Y / 2f));
                    enemy.Velocity = new Vector2(Engine.RandomFloat() * 256f, -128f - 256f * Engine.RandomFloat());
                }
                else if (roll <= .75f)
                {
                    // Left Side
                    var enemy = Scene.CreateObject<Enemy>(new Vector2(-Engine.Resolution.X / 2f, Engine.RandomFloat() * Engine.Resolution.Y - Engine.Resolution.Y / 2f));
                    enemy.Velocity = new Vector2(128f, -96f);
                }
                else
                {
                    // Right Side
                    var enemy = Scene.CreateObject<Enemy>(new Vector2(Engine.Resolution.X / 2f, Engine.RandomFloat() * Engine.Resolution.Y - Engine.Resolution.Y / 2f));
                    enemy.Velocity = new Vector2(-128f, -96f);
                }
                ev = Engine.InvokeTimed(1f / Difficulty - offset, spawnEnemy);
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
