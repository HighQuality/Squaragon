using Cog;
using Cog.Interface;
using Cog.Modules.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Interface
{
    class DifficultyElement : InterfaceElement
    {
        BitmapFont font;
        public DifficultyElement(InterfaceElement parent, Vector2 location)
            :base(parent, location)
        {
            font = (BitmapFont)Engine.ResourceHost.GetContainer("main").Load("Fonts/Alpha Quadrant.fnt");
        }

        public override void OnDraw(IRenderTarget target, Vector2 drawPosition)
        {
            font.DrawString(target, string.Format("Difficulty: {0}", Math.Round(Program.Scene.CurrentMode.Difficulty * 10) / 10), 
                20, Color.Red, drawPosition, HAlign.Left, VAlign.Center);
        }
    }
}
