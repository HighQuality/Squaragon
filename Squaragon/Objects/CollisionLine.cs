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
    class CollisionLine : GameObject, IPlayerCollision
    {
        public float Radius
        {
            get
            {
                return Line.Length;
            }
        }
        public Line Line;
        
        public CollisionLine()
        {

        }

        public void Start(Line line)
        {
            Line = line;
            var pixel = Resources.GetTexture("pixel");
            SpriteComponent sprite = SpriteComponent.RegisterOn(this, pixel);
            sprite.Origin = new Vector2(0.5f, 0.5f);
            LocalRotation = Line.Angle;
            sprite.Scale = new Vector2(Line.Length, 3f);
            sprite.Color = Color.Black;
            RegisterEvent<PhysicsUpdateEvent>(0, PhysicsUpdate);
            WorldCoord = line.Middle;
        }

        protected void PhysicsUpdate(PhysicsUpdateEvent ev)
        {

        }

        public bool CheckCollisionWith(PhysicsObject other)
        {
            Vector2 ox1, ox2,
                oy1, oy2;

            var pos = WorldCoord;
            var oPos = other.WorldCoord;
            var rot = WorldRotation;
            var oRot = other.WorldRotation;

            ox1 = oPos + new Vector2(-other.Size.X * .5f, -other.Size.Y * .5f).Rotate(oRot);
            ox2 = oPos + new Vector2(other.Size.X * .5f, -other.Size.Y * .5f).Rotate(oRot);
            oy1 = oPos + new Vector2(-other.Size.X * .5f, other.Size.Y * .5f).Rotate(oRot);
            oy2 = oPos + new Vector2(other.Size.X * .5f, other.Size.Y * .5f).Rotate(oRot);

            Line[] otherLines = new Line[4] {
                new Line { FirstPoint = ox1, SecondPoint = ox2 },
                new Line { FirstPoint = oy1, SecondPoint = oy2 },
                new Line { FirstPoint = ox2, SecondPoint = oy2 },
                new Line { FirstPoint = ox1, SecondPoint = oy1 }
            };

            for (int j = 0; j < 4; j++)
                if (LineCollision(Line.FirstPoint, Line.SecondPoint, otherLines[j].FirstPoint, otherLines[j].SecondPoint))
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

        public void OnCollisionWithPlayer(Player player)
        {
            player.Velocity *= -1;
        }
    }
}
