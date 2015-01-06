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
            Font.DrawString(target, string.Format("Score: {0}", Program.Scene.Score), Font.RenderSize, Color.Black, drawPosition + Vector2.One, HAlign.Center, VAlign.Top);
            //Font.DrawString(target, string.Format("Score: 0"), 16f, Color.White, drawPosition, HAlign.Left, VAlign.Top);

            base.OnDraw(target, drawPosition);
        }
    }
}
