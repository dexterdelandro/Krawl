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
    class Animator : SpriteRenderer
    {
        public Animation[] Animations;
        public Animation CurrentAnimation;

        float animationFrameTimeLeft;
        int currentAnimationSpriteID;

        public override void Start()
        {

        }

        public override void Update()
        {
            if (CurrentAnimation != null)
            {
                animationFrameTimeLeft -= Time.DeltaTime;

                if (animationFrameTimeLeft <= 0)
                {
                    currentAnimationSpriteID = ++currentAnimationSpriteID % CurrentAnimation.Sprites.Length;
                    animationFrameTimeLeft = CurrentAnimation.Speed / 1.5f;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (CurrentAnimation == null)
                return;
            //throw new Exception(ConnectedGameObject.Name + " -> Active animation is null!");
            rec = new Rectangle((int)ConnectedGameObject.Position.X, (int)ConnectedGameObject.Position.Y, (int)(CurrentAnimation.Sprites[0].Width * ConnectedGameObject.Scale.X), (int)(CurrentAnimation.Sprites[0].Height * ConnectedGameObject.Scale.Y));
            try
            {
                batch.Draw(CurrentAnimation.Sprites[currentAnimationSpriteID], rec, ColorOverlay);
            }
            catch { }
        }
    }
}
