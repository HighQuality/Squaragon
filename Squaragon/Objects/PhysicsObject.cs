using Cog;
using Cog.Modules.Content;
using Cog.Modules.EventHost;
using Cog.Modules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Objects
{
    [Resource(ContainerName = "main", Filename = "Textures/pixel.png", Key = "pixel")]
    class PhysicsObject : GameObject
    {
        public float Radius { get; private set; }
        public Vector2 Velocity;
        public float Gravity = 200f;
        public double CreationTime = Engine.TimeStamp;
        public float Age { get { return (float)(Engine.TimeStamp - CreationTime); } }
        const float outlineThickness = 2f;
        public SpriteComponent Sprite,
            Outline;

        public PhysicsObject(Vector2 size, Color color)
        {
            var pixel = Resources.GetTexture("pixel");

            Outline = SpriteComponent.RegisterOn(this, pixel);
            Outline.Origin = new Vector2(0.5f, 0.5f);
            Outline.Color = Color.White; //new Color(44, 62, 80);

            Sprite = SpriteComponent.RegisterOn(this, pixel);
            Sprite.Origin = new Vector2(0.5f, 0.5f);
            Sprite.Color = color; // new Color(52, 152, 219);

            SetSize(size);
            LocalScale = Vector2.Zero;

            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
        }

        protected void SetSize(Vector2 size)
        {
            Outline.Scale = size + new Vector2(outlineThickness * 2f, outlineThickness * 2f);
            Sprite.Scale = size;

            this.Size = size;
            Radius = (Size * 0.5f).Length;
        }

        protected virtual void PhysicsUpdate(PhysicsUpdateEvent ev)
        {
            Velocity.Y += Gravity * ev.DeltaTime;
            LocalScale = new Vector2(1f, 1f) * Mathf.Min(Age, 1f);

            LocalCoord += Velocity * ev.DeltaTime;
        }

        public bool CheckCollisionWith(PhysicsObject other)
        {
            //TODO: Move collisioncheck to Line class.
            Vector2 x1, x2,
                y1, y2,
                
                ox1, ox2,
                oy1, oy2;

            var pos = WorldCoord;
            var oPos = other.WorldCoord;
            var rot = WorldRotation;
            var oRot = other.WorldRotation;

            x1 = pos + new Vector2(-Size.X * .5f, -Size.Y * .5f).Rotate(rot);
            x2 = pos + new Vector2(Size.X * .5f, -Size.Y * .5f).Rotate(rot);
            y1 = pos + new Vector2(-Size.X * .5f, Size.Y * .5f).Rotate(rot);
            y2 = pos + new Vector2(Size.X * .5f, Size.Y * .5f).Rotate(rot);

            ox1 = oPos + new Vector2(-other.Size.X * .5f, -other.Size.Y * .5f).Rotate(oRot);
            ox2 = oPos + new Vector2(other.Size.X * .5f, -other.Size.Y * .5f).Rotate(oRot);
            oy1 = oPos + new Vector2(-other.Size.X * .5f, other.Size.Y * .5f).Rotate(oRot);
            oy2 = oPos + new Vector2(other.Size.X * .5f, other.Size.Y * .5f).Rotate(oRot);

            Line[] myLines = new Line[4] {
                new Line { FirstPoint = x1, SecondPoint = x2 },
                new Line { FirstPoint = y1, SecondPoint = y2 },
                new Line { FirstPoint = x2, SecondPoint = y2 },
                new Line { FirstPoint = x1, SecondPoint = y1 }
            };
            Line[] otherLines = new Line[4] {
                new Line { FirstPoint = ox1, SecondPoint = ox2 },
                new Line { FirstPoint = oy1, SecondPoint = oy2 },
                new Line { FirstPoint = ox2, SecondPoint = oy2 },
                new Line { FirstPoint = ox1, SecondPoint = oy1 }
            };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (LineCollision(myLines[i].FirstPoint, myLines[i].SecondPoint, otherLines[j].FirstPoint, otherLines[j].SecondPoint))
                        return true;
            return false;
        }

        public bool LineCollision(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}
