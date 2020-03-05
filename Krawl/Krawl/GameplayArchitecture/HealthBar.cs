using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Krawl.GameArchitecture;
using Krawl.GameplayArchitecture;
using System;


namespace Krawl.GameplayArchitecture
{
    class HealthBar : GameObject
    {
        int health;
        public GameObject MovingBar;
        public GameObject SlowMovingBar;
        public GameObject BackgroundBar;
        Player player;
        public HealthBar(Player player, Players whichPlayer)
        {
            this.player = player;
            health = (int)player.HP;

            BackgroundBar = new GameObject();
            BackgroundBar.Position = new Vector2((whichPlayer == Players.Player2 ? 932 : 10), 10);
            SpriteRenderer renderer = BackgroundBar.AddComponent<SpriteRenderer>();
            renderer.Sprite = Game1.LoadedImages["health_bar_decoration" + (whichPlayer == Players.Player2 ? "_blue" : "" )];
            BackgroundBar.Scale = new Vector2(4);
            renderer.DrawLayer = 6;

            MovingBar = new GameObject();
            MovingBar.Position = new Vector2((whichPlayer == Players.Player2 ? 987 : 66), 10);
            renderer = MovingBar.AddComponent<SpriteRenderer>();
            renderer.Sprite = Game1.LoadedImages["health_bar" + (whichPlayer == Players.Player2 ? "_blue" : "")];
            MovingBar.Scale = new Vector2(4);
            renderer.DrawLayer = 8;

            SlowMovingBar = new GameObject();
            SlowMovingBar.Position = new Vector2((whichPlayer == Players.Player2 ? 987 : 66), 10);
            renderer = SlowMovingBar.AddComponent<SpriteRenderer>();
            renderer.Sprite = Game1.LoadedImages["health_bar_o"];
            SlowMovingBar.Scale = new Vector2(4);
            renderer.DrawLayer = 7;
        }

        public override void Update()
        {
            MovingBar.Scale = Vector2.Lerp(MovingBar.Scale, new Vector2((player.HP / player.MaxHP) * 4, MovingBar.Scale.Y), Time.DeltaTime * 30);
            SlowMovingBar.Scale = Vector2.Lerp(SlowMovingBar.Scale, new Vector2((player.HP / player.MaxHP) * 4, SlowMovingBar.Scale.Y), Time.DeltaTime * 3);
            base.Update();
        }

        
    }
}
