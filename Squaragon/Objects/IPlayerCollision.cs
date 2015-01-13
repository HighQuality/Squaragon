using Cog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Squaragon.Objects
{
    interface IPlayerCollision
    {
        Vector2 WorldCoord { get; }
        float Radius { get; }
        void OnCollisionWithPlayer(Player player);
        bool CheckCollisionWith(PhysicsObject other);
    }
}
