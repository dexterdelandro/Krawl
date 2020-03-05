using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Krawl.GameArchitecture
{
    class Rigidbody : Component
    {
        public float MaxVelocityMagnitude = 15;
        public Vector2 Velocity; // Units per sec
        public float FrictionContant = .5f; // 0-1 : 1 = max friction and 0 = no friction

        public override void Start()
        {
            
        }

        public override void Update()
        {
            VeloctiyPositionUpdate();
        }

        public void VeloctiyPositionUpdate()
        {
            Velocity.X -= FrictionContant * Velocity.X;
            Velocity.Y -= FrictionContant * Velocity.Y;

            Velocity.X = Math.Max(Math.Min(Velocity.X, MaxVelocityMagnitude), -MaxVelocityMagnitude);
            Velocity.Y = Math.Max(Math.Min(Velocity.Y, MaxVelocityMagnitude), -MaxVelocityMagnitude);

            //

            Vector2 pos = ConnectedGameObject.Position;
            pos.X += (Velocity.X);
            pos.Y += (Velocity.Y);
            ConnectedGameObject.Position = pos;
        }
    }
}
