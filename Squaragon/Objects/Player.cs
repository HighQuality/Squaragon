﻿using Cog;
using Cog.Modules.Content;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cog.Modules.EventHost;

namespace Squaragon.Objects
{
    class Player : PhysicsObject
    {
        private float rotationSpeed;
        
        public Player()
            : base(new Vector2(24f, 24f), new Color(52, 152, 219))
        {
            RegisterEvent<ButtonDownEvent>(0, ButtonDown);
        }

        protected override void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            LocalRotation += Angle.FromDegree(rotationSpeed * ev.DeltaTime);

            #region Bounce
            if (WorldCoord.X < -Engine.Resolution.X / 2)
            {
                WorldCoord = new Vector2(-Engine.Resolution.X / 2, WorldCoord.Y);
                Velocity.X = Math.Abs(-Velocity.X);
            }
            if (WorldCoord.Y < -Engine.Resolution.Y / 2)
            {
                WorldCoord = new Vector2(WorldCoord.X, -Engine.Resolution.Y / 2);
                Velocity.Y = Math.Abs(-Velocity.Y);
            }
            if (WorldCoord.X > Engine.Resolution.X / 2)
            {
                WorldCoord = new Vector2(Engine.Resolution.X / 2, WorldCoord.Y);
                Velocity.X = -Math.Abs(Velocity.X);
            }
            if (WorldCoord.Y > Engine.Resolution.Y / 2)
            {
                WorldCoord = new Vector2(WorldCoord.X, Engine.Resolution.Y / 2);
                Velocity.Y = -Math.Abs(Velocity.Y);
            } 
            #endregion

            foreach (var enemy in Scene.EnumerateObjects<Enemy>())
            {
                var distanceTo = (enemy.WorldCoord - WorldCoord).Length - Radius - enemy.Radius;
                if (distanceTo <= 0f)
                {
                    Remove();
                }
            }

            base.PhysicsUpdate(ev);
        }

        private void ButtonDown(ButtonDownEvent ev)
        {
            if (ev.Button == Mouse.Button.Left)
            {
                Vector2 initialPosition = ev.Position;
                var arrow = Scene.CreateObject<Arrow>(this, Vector2.Zero);
                const float lengthDivision = 400f;
                var listener = RegisterEvent<BeginDrawEvent>(0, e =>
                {
                    Vector2 deltaPosition = (Mouse.Location - initialPosition) * new Vector2(-1f, -1f);
                    float length = deltaPosition.Length;
                    deltaPosition.Length = (length / lengthDivision / (length / lengthDivision + 0.5f));

                    arrow.Update(deltaPosition);
                });

                ev.ButtonUpCallback = () =>
                {
                    listener.Cancel();
                    arrow.Remove();
                    Vector2 deltaPosition = (Mouse.Location - initialPosition) * new Vector2(-1f, -1f);
                    if (deltaPosition.Length >= 20f)
                    {
                        float length = deltaPosition.Length;

                        deltaPosition.Length = (length / lengthDivision / (length / lengthDivision + 0.5f));
                        Velocity = deltaPosition * 750f;
                        rotationSpeed = deltaPosition.X * 360f;
                    }
                    else
                        Velocity *= .5f;
                };

                ev.Intercept = true;
            }
        }
    }
}
