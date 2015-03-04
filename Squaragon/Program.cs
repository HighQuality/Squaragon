using Cog;
using Cog.Modules.EventHost;
using Cog.Modules.Renderer;
using Cog.SfmlAudio;
using Cog.SfmlRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squaragon
{
    class Program
    {
        public static MainScene Scene;

        static void Main(string[] args)
        {
            Engine.Initialize<SfmlRenderer, SfmlAudioModule>();
            Engine.DesiredResolution = new Vector2(720f, 720f);
            
            Engine.EventHost.RegisterEvent<InitializeEvent>(0, Initialize);

            Engine.StartGame("Squaragon", WindowStyle.Default);
        }

        static void Initialize(InitializeEvent ev)
        {
            Engine.ResourceHost.LoadDictionary("main", "Resources");

            Scene = Engine.SceneHost.CreateGlobal<MainScene>();
            Scene.Initialize(MainScene.GameModes.Arcade);
            Engine.SceneHost.Push(Scene);
        }
    }
}
