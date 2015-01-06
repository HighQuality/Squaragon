using Cog;
using Cog.Scenes;
using Squaragon.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon
{
    class MainScene : Scene
    {
        public MainScene()
            : base("Game")
        {
            CreateObject<Player>(new Vector2(0f, 0f));
            SpawnEnemy(0f);

            BackgroundColor = new Color(236, 240, 241);
        }

        private void SpawnEnemy(float offset)
        {
            CreateObject<Enemy>(new Vector2(Engine.RandomFloat() * Engine.Resolution.X - Engine.Resolution.X / 2f, -Engine.Resolution.Y / 2f));

            Engine.InvokeTimed(1f - offset, SpawnEnemy);
        }
    }
}
