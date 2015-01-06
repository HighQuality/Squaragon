using Cog;
using Cog.Modules.EventHost;
using Cog.Scenes;
using Squaragon.Interface;
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
        public float Score { get; set; }
        public float Multiplier { get; set; }

        public MainScene()
            : base("Game")
        {
            CurrentMode = new StandardMode(this);

            Multiplier = 1f;

            CreateObject<Player>(new Vector2(0f, 0f));

            BackgroundColor = new Color(236, 240, 241);

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);

            CreateObject<Star>(new Vector2((Engine.RandomFloat() - 0.5f) * Engine.Resolution.X * 0.9f, (Engine.RandomFloat() - 0.5f) * Engine.Resolution.Y * 0.9f));

            Engine.InvokeTimed(1f, AddScore);
            
            new ScoreElement(Interface, new Vector2(Engine.Resolution.X / 2f, 32f));
        }

        private void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (CurrentMode != null)
                CurrentMode.TriggerUpdate(ev.DeltaTime);
        }

        private void AddScore(float offset)
        {
            Score += Multiplier;
            Engine.InvokeTimed(1f - offset, AddScore);
        }
    }
}
