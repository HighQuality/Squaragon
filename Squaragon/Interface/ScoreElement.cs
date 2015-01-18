using Cog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog;
using Cog.Modules.Renderer;

namespace Squaragon.Interface
{
    class ScoreElement : InterfaceElement
    {
        public BitmapFont Font;

        public ScoreElement(InterfaceElement parent, Vector2 location)
            : base(parent, location)
        {
            Font = (BitmapFont)Engine.ResourceHost.GetContainer("main").Load("Fonts/Alpha Quadrant.fnt");
        }

        public override void OnDraw(IRenderTarget target, Vector2 drawPosition)
        {
            Font.DrawString(target, string.Format("Score: {0}", Program.Scene.Score), Font.RenderSize, new Color(64, 64, 64), drawPosition + new Vector2(0f, 4f), HAlign.Center, VAlign.Center);
            Font.DrawString(target, string.Format("Score: {0}", Program.Scene.Score), Font.RenderSize + 4f, Color.White, drawPosition + new Vector2(0f, 0f), HAlign.Center, VAlign.Center);
            
            base.OnDraw(target, drawPosition);
        }
    }
}
