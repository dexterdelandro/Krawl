using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Krawl.GameArchitecture;

namespace Krawl
{
    delegate void OnButtonClickDelegate(Button btn);

    public enum ButtonStatus {Away, Hovered, Clicked}
	class Button : GameObject
	{
        public event OnButtonClickDelegate OnButtonClick;

        private Texture2D baseImage;
		private Texture2D hoverImage;
		private Rectangle rect;
		private ButtonStatus status;

		/// <summary>
		/// returns the button's inner-rectangle object
		/// </summary>
		public Rectangle Rect { get { return rect; } }
		
		/// <summary>
		/// Gets or sets the state the button is in.
		/// </summary>
		public ButtonStatus IsPressed { get { return status; } set { status = value; } }

		/// <summary>
		/// creates a new button object with specified information. Sets button to Away status.
		/// </summary>
		/// <param name="baseImage">image of button when not hovered</param>
		/// <param name="hoverImage">image of button when it is hovered</param>
		/// <param name="x">x position of the button</param>
		/// <param name="y">y position of the button</param>
		/// <param name="width">width of the button</param>
		/// <param name="height">height of the button</param>
		public Button(Texture2D baseImage, Texture2D hoverImage, int x, int y, int width, int height)
		{
			status = ButtonStatus.Away;
			this.baseImage = baseImage;
			this.hoverImage = hoverImage;
			rect = new Rectangle(x, y, width, height);
		}

		public Button(Texture2D baseImage, int x, int y, int width, int height) {
			status = ButtonStatus.Away;
			this.baseImage = baseImage;
			this.hoverImage = baseImage;
			rect = new Rectangle(x, y, width, height);

            SpriteRenderer rend = AddComponent<SpriteRenderer>();
            rend.Sprite = baseImage;

        }

		/// <summary>
		/// Updates the status of the button: either away, hovered, or clicked
		/// </summary>
		private void UpdateButtonStatus() {
			if (status != ButtonStatus.Clicked)
			{
				MouseState state = Mouse.GetState();
				if (state.X >= rect.X && state.X <= rect.X + rect.Width &&
					state.Y >= rect.Y && state.Y <= rect.Y + rect.Height)
				{
					status = state.LeftButton == ButtonState.Pressed ? ButtonStatus.Clicked : ButtonStatus.Hovered;

                    if (state.LeftButton == ButtonState.Pressed)
                        OnButtonClick?.Invoke(this);
                }
				else
				{
					status = ButtonStatus.Away;
				}
			}
		}

		/// <summary>
		/// Public Update method that should be called every tick while the button is on the screen. Updates the status of the button.
		/// </summary>
        public override void Update()
        {
            UpdateButtonStatus();
        }

        /// <summary>
        /// Sets the button status to away. Call this when the Button appears on screen again. (not the first time)
        /// </summary>
        public void ResetButton() {
			status = ButtonStatus.Away;
		}

        /// <summary>
        /// Draws this button (changes when hovered) to the screen at it's rectangle.
        /// </summary>
        /// <param name="sb">SpriteBatch you are using to draw</param>
        public override void Draw(SpriteBatch sb)
        {
            switch (status)
            {
                case ButtonStatus.Away:
                    sb.Draw(baseImage, rect, Color.White);
                    break;

                case ButtonStatus.Clicked:
                case ButtonStatus.Hovered:
                    sb.Draw(hoverImage, rect, Color.White);
                    break;
            }
        }
    }
}
