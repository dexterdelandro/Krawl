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
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite;
        public Color ColorOverlay = Color.White;
        public int DrawLayer = 0; // 0 being drawn first and then 1, 2, 3... and so on.
        // Backgrounds should be drawn on the lower layers so they dont overlap other things like the player
        protected Rectangle rec;

        public Rectangle Rec { get => rec; }

        public override void Start()
        {

        }

        public override void Update()
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (Sprite == null)
                throw new Exception("No sprite attached to \"" + this + "\"!");
            rec = new Rectangle((int)ConnectedGameObject.Position.X, (int)ConnectedGameObject.Position.Y, (int)(Sprite.Width * ConnectedGameObject.Scale.X), (int)(Sprite.Height * ConnectedGameObject.Scale.Y));
            batch.Draw(Sprite, rec, ColorOverlay);
        }
    }
}
