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
                Scene.CreateObject<Enemy>(new Vector2(Engine.RandomFloat() * Engine.Resolution.X - Engine.Resolution.X / 2f, -Engine.Resolution.Y / 2f));
                ev = Engine.InvokeTimed(1f - offset, spawnEnemy);
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
