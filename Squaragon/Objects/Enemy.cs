﻿using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog.Modules.EventHost;

namespace Squaragon.Objects
{
    class Enemy : PhysicsObject
    {
        private float rotationSpeed;

        public Enemy()
            : base(new Vector2(24f, 24f), new Color(155, 89, 182))
        {
            rotationSpeed = Engine.RandomFloat() >= 0.5f ? 200f : -200f;
        }

        protected override void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            LocalRotation += Angle.FromDegree(rotationSpeed * ev.DeltaTime);

            if (WorldCoord.Length >= 720f)
            {
                Remove();
            }

            base.PhysicsUpdate(ev);
        }
    }
}
