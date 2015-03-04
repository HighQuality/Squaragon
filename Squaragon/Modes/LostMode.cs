using Cog;
using Cog.Modules.Content;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog.Modules.EventHost;
using Squaragon.Objects;
using Cog.Interface;
using Squaragon.Interface;

namespace Squaragon.Modes
{
    class LostMode : Mode
    {
        public LostMode(MainScene scene)
            : base(scene)
        {
            new LostElement(Program.Scene.Interface, new Vector2(Engine.Resolution.X / 2f, Engine.Resolution.Y / 2f));
        }
    }
}
