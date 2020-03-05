using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Krawl.GameArchitecture
{
    public class Animation
    {
        public Texture2D[] Sprites;
        public float Speed;

        public Animation() { }

        public Animation(Texture2D[] sprites, float speed)
        {
            Sprites = sprites;
            Speed = speed;
        }
    }
}
