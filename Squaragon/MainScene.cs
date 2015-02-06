using Cog;
using Cog.Modules.EventHost;
using Cog.Modules.Renderer;
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
        public float Score { get; set; }
        public float Multiplier { get; set; }
        private Texture pixel,
            grid;
        public Player Player;

        public Rectangle PlayableArea = new Rectangle(new Vector2(540f, 540f) / 2f * -1f, new Vector2(540f, 540f));

        public MainScene()
            : base("Game")
        {
            CurrentMode = new StandardMode(this);

            Multiplier = 1f;

            Player = CreateObject<Player>(new Vector2(0f, 0f));

            RegisterEvent<KeyDownEvent>((int)Keyboard.Key.R, 0, ev =>
                {
                    Player.Remove();
                    Player = CreateObject<Player>(new Vector2(0f, 0f));
                });

            pixel = (Texture)Engine.ResourceHost.GetContainer("main").Load("Textures/pixel.png");
            grid = (Texture)Engine.ResourceHost.GetContainer("main").Load("Textures/grid.png");
            RegisterEvent<DrawEvent>(int.MaxValue - 1, Draw);

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);

            CreateObject<Star>(RandomizePlayablePosition());

            Engine.InvokeTimed(1f, AddScore);

            new ScoreElement(Interface, new Vector2(Engine.Resolution.X / 2f, 64f));
            new DifficultyElement(Interface, new Vector2(20f, 20f));
        }

        private void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (CurrentMode != null)
                CurrentMode.TriggerUpdate(ev.DeltaTime);
            Camera.WorldCoord += (Player.WorldCoord - Camera.WorldCoord) * Mathf.Min(1f, ev.DeltaTime * 3f);
            CurrentMode.Difficulty += 0.08f * ev.DeltaTime;
        }

        private void Draw(DrawEvent ev)
        {
            const float outlineSize = 32f;
            ev.RenderTarget.DrawTexture(pixel, Engine.Resolution / 2f * -1f + Camera.WorldCoord, new Color(48, 48, 48), Engine.Resolution, Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));
            
            ev.RenderTarget.DrawTexture(grid, Engine.Resolution / -2f + Camera.WorldCoord, new Color(255, 255, 255, 255), Vector2.One, Vector2.Zero, 0f, new Rectangle(Vector2.One * 9999f + Engine.Resolution / -2f + Camera.WorldCoord, Engine.Resolution));

            ev.RenderTarget.DrawTexture(pixel, PlayableArea.TopLeft - new Vector2(outlineSize, outlineSize), new Color(236, 240, 241), PlayableArea.Size + new Vector2(outlineSize * 2f, outlineSize * 2f), Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));
            ev.RenderTarget.DrawTexture(pixel, PlayableArea.TopLeft, new Color(127, 140, 141), PlayableArea.Size, Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));
            
            ev.RenderTarget.DrawTexture(grid, PlayableArea.TopLeft, new Color(255, 255, 255, 255), Vector2.One, Vector2.Zero, 0f, new Rectangle(Vector2.One * 9999f + PlayableArea.TopLeft, PlayableArea.Size));

        }

        private void AddScore(float offset)
        {
            Score += Multiplier;
            Engine.InvokeTimed(1f - offset, AddScore);
        }

        public Vector2 RandomizePlayablePosition()
        {
            return PlayableArea.TopLeft + PlayableArea.Size * new Vector2(Engine.RandomFloat(), Engine.RandomFloat());
        }
    }
}
