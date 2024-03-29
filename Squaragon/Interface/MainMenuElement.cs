﻿using Cog.Interface;
using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog.Modules.Renderer;
using Cog.Modules.EventHost;
using Squaragon.Modes;
using Squaragon.Objects;

namespace Squaragon.Interface
{
    class MainMenuElement : InterfaceElement
    {
        BitmapFont font;
        EventListener<ButtonDownEvent> ev;
        public MainMenuElement(InterfaceElement parent, Vector2 location)
            :base(parent, location)
        {
            ev = RegisterEvent<ButtonDownEvent>(0, ButtonDown);
            Program.Scene.Lost = false;
            font = (BitmapFont)Engine.ResourceHost.GetContainer("main").Load("Fonts/Alpha Quadrant.fnt");
        }

        public void ButtonDown(ButtonDownEvent ev)
        {
            if (ev.Button == Mouse.Button.Left || ev.Button == Mouse.Button.Right || ev.Button == Mouse.Button.Middle)
            {
                Program.Scene.CurrentMode = new StandardMode(Program.Scene);
                Program.Scene.Player = Program.Scene.CreateObject<Player>(new Vector2(0f, 0f));
                Remove();
                this.ev.Cancel();
            }
        }
        public override void OnDraw(IRenderTarget target, Vector2 drawPosition)
        {
            font.DrawString(target, "SQUARAGON",
                70, Color.Black, drawPosition, HAlign.Center, VAlign.Center);
            font.DrawString(target, "Press any mouse button to Start!",
                30, Color.Red, drawPosition + new Vector2(0, 100), HAlign.Center, VAlign.Center);
        }
    }
}
