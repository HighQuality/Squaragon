using Cog;
using Cog.Modules.EventHost;
using Cog.Scenes;
using Squaragon.Modes;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon
{
    class MainScene : Scene
    {
        public Mode CurrentMode;
        private Star star;

        public MainScene()
            : base("Game")
        {
            CurrentMode = new StandardMode(this);

            CreateObject<Player>(new Vector2(0f, 0f));

            BackgroundColor = new Color(236, 240, 241);

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);

            CreateObject<Star>(new Vector2((Engine.RandomFloat() - 0.5f) * Engine.Resolution.X, (Engine.RandomFloat() - 0.5f) * Engine.Resolution.Y));
        }

        private void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (CurrentMode != null)
                CurrentMode.TriggerUpdate(ev.DeltaTime);
        }
    }
}
