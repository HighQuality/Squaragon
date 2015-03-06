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
        public enum GameModes
        {
            Arcade,
            Adventure
        };
        GlslShader gaussianBlurX, gaussianBlurY, saveColors;

        RenderTexture bloomRender, mainRender;
        public Mode CurrentMode;
        public float Score { get; set; }
        public float Multiplier { get; set; }
        private Texture pixel,
            grid;
        public Player Player;

        public bool Lost = false;

        public Rectangle PlayableArea = new Rectangle(new Vector2(540f, 540f) / 2f * -1f, new Vector2(540f, 540f));

        public MainScene()
            : base("Game")
        {

        }

        public void Initialize(GameModes mode)
        {
            #region SaveColors
            /*saveColors = Engine.Renderer.LoadGlslShader(
                            @"// Vertex Shader

            void main()
            {
             gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
             gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
             gl_FrontColor = gl_Color;
            }

            ",

            @"// Fragment Shader
            uniform sampler2D texture;

            void main()
            {
                gl_Color = texture2D(texture, gl_TexCoord[0].xy);
                if (gl_Color.r > 0.5 || gl_Color.g > 0.5 || gl_Color.g > 0.5)
                {
                    gl_FragColor = gl_Color;
                }
                else
                {
                    gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);
                }
            }
            ");
            #endregion

            #region GaussianBlurX
            gaussianBlurX = Engine.Renderer.LoadGlslShader(
                            @"// Vertex Shader
           
            void main()
            {
             gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
             gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
             gl_FrontColor = gl_Color;
            }

            ",

            @"// Fragment Shader
            uniform sampler2D texture;
            const float BLUR_SIZE = 0.001087;
            void main()
            {
                gl_Color = texture2D(texture, gl_TexCoord[0].xy);
                gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x - 4.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.05;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x - 3.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.09;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x - 2.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.12;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x - BLUR_SIZE, gl_TexCoord[0].y)) * 0.15;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y)) * 0.2;        
                gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x + 4.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.05;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x + 3.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.09;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x + 2.0 * BLUR_SIZE, gl_TexCoord[0].y)) * 0.12;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x + BLUR_SIZE, gl_TexCoord[0].y)) * 0.15;
            }
            ");
            #endregion

            #region GaussianBlurY
            gaussianBlurY = Engine.Renderer.LoadGlslShader(
                            @"// Vertex Shader
           
            void main()
            {
             gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
             gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
             gl_FrontColor = gl_Color;
            }

            ",

            @"// Fragment Shader
            uniform sampler2D texture;
            const float BLUR_SIZE = 0.001087;
            void main()
            {
                gl_Color = texture2D(texture, gl_TexCoord[0].xy);
                gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y - 4.0 * BLUR_SIZE)) * 0.05;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y - 3.0 * BLUR_SIZE)) * 0.09;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y - 2.0 * BLUR_SIZE)) * 0.12;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y - BLUR_SIZE)) * 0.15;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y)) * 0.2;        
                gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y + 4.0 * BLUR_SIZE)) * 0.05;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y + 3.0 * BLUR_SIZE)) * 0.09;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y + 2.0 * BLUR_SIZE)) * 0.12;
	            gl_Color += texture2D(texture, vec2(gl_TexCoord[0].x, gl_TexCoord[0].y + BLUR_SIZE)) * 0.15;
            }
            ");*/
            #endregion

            bloomRender = Engine.Renderer.CreateRenderTexture((int)Engine.Resolution.X / 4, (int)Engine.Resolution.Y / 4);
            switch (mode)
            {
                case GameModes.Arcade:
                    CurrentMode = new MenuMode(this);
                    break;

                case GameModes.Adventure:
                    PlayableArea = new Rectangle(new Vector2(-200f, -200f), new Vector2(2000f, 600f));
                    CurrentMode = new AdventureMode(this);
                    break;

                default:
                    throw new NotImplementedException();
            }
            
            Multiplier = 1f;


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
        }

        private void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            if (CurrentMode != null && !(CurrentMode is LostMode))
                CurrentMode.TriggerUpdate(ev.DeltaTime);
            if (Player != null)
            {
                Camera.WorldCoord += (Player.WorldCoord - Camera.WorldCoord) * Mathf.Min(1f, ev.DeltaTime * 3f);
                CurrentMode.Difficulty += 0.08f * ev.DeltaTime;
            }
            else
            {
                Camera.WorldCoord = new Vector2(0, 0);
            }
            if (Lost && !(CurrentMode is LostMode))
                CurrentMode = new LostMode(this);
        }

        private void Draw(DrawEvent ev)
        {
            const float outlineSize = 32f;

            ev.RenderTarget.DrawTexture(pixel, Engine.Resolution / 2f * -1f + Camera.WorldCoord, new Color(48, 48, 48), Engine.Resolution, Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));

            ev.RenderTarget.DrawTexture(grid, Engine.Resolution / -2f + Camera.WorldCoord, new Color(255, 255, 255, 255), Vector2.One, Vector2.Zero, 0f, new Rectangle(Vector2.One * 9999f + Engine.Resolution / -2f + Camera.WorldCoord, Engine.Resolution));

            ev.RenderTarget.DrawTexture(pixel, PlayableArea.TopLeft - new Vector2(outlineSize, outlineSize), new Color(236, 240, 241), PlayableArea.Size + new Vector2(outlineSize * 2f, outlineSize * 2f), Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));
            ev.RenderTarget.DrawTexture(pixel, PlayableArea.TopLeft, new Color(127, 140, 141), PlayableArea.Size, Vector2.Zero, 0f, new Rectangle(0f, 0f, 1f, 1f));

            ev.RenderTarget.DrawTexture(grid, PlayableArea.TopLeft, new Color(255, 255, 255, 255), Vector2.One, Vector2.Zero, 0f, new Rectangle(Vector2.One * 9999f + PlayableArea.TopLeft, PlayableArea.Size));
        
            //Apply bloom.
            //ev.RenderTarget.DrawTexture(ApplyBloom(ev.RenderTarget.))
        }

        private void AddScore(float offset)
        {
            if (!Lost)
                Score += Multiplier;
            Engine.InvokeTimed(1f - offset, AddScore);
        }

        public Vector2 RandomizePlayablePosition()
        {
            return PlayableArea.TopLeft + PlayableArea.Size * new Vector2(Engine.RandomFloat(), Engine.RandomFloat());
        }

        private Texture ApplyBloom(Texture texture)
        {
            bloomRender.Clear(new Color(0, 0, 0, 0)); //clears texture from earlier bloom passes
            #region SaveColors
            using (saveColors.Activate())
            {

                bloomRender.DrawTexture(texture, Vector2.Zero, Color.White, new Vector2(0.25f, 0.25f), Vector2.Zero, 0f,
                    new Rectangle(Vector2.One * 9999f + PlayableArea.TopLeft, PlayableArea.Size));
            }
            #endregion
            Texture gaussTexture = bloomRender.Texture;
            bloomRender.Clear(Color.Black);
            #region GaussianBlurX
            using (gaussianBlurX.Activate())
            {

                bloomRender.DrawTexture(gaussTexture, Vector2.Zero, Color.White, new Vector2(0.25f, 0.25f), Vector2.Zero, 0f,
                    new Rectangle(Vector2.One * 9999f + PlayableArea.TopLeft, PlayableArea.Size));
            }
            #endregion
            gaussTexture = bloomRender.Texture;
            bloomRender.Clear(Color.Black);
            #region GaussianBlurY
            using (gaussianBlurY.Activate())
            {

                bloomRender.DrawTexture(gaussTexture, Vector2.Zero, Color.White, new Vector2(0.25f, 0.25f), Vector2.Zero, 0f,
                    new Rectangle(Vector2.One * 9999f + PlayableArea.TopLeft, PlayableArea.Size));
            }
            #endregion
            return bloomRender.Texture;
        }
    }
}
