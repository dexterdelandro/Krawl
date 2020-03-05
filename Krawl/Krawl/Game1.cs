using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Krawl.GameArchitecture;
using Krawl.GameplayArchitecture;
using System;


namespace Krawl
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Game1 : Game
    {
		float totalArmorP1;
		float totalDamageP1;
		float totalMoveSpeedP1;
		float totalArmorP2;
		float totalDamageP2;
		float totalMoveSpeedP2;

		private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D title;
        Button start;
        SpriteFont text;
        SpriteFont arial12;

        public static Game1 use;
        public static Random Rand = new Random();

        /// <summary>
        /// This will hold all the loaded Texture2Ds.
        /// These Images must be located in the 'Images\' folder
        /// </summary>
        public static Dictionary<string, Texture2D> LoadedImages = new Dictionary<string, Texture2D>();

        /// <summary>
        /// This is a lookup of all the loaded enemies by their name.
        /// </summary>
        public static Dictionary<string, EnemyBlueprint> LoadedEnemyBlueprints = new Dictionary<string, EnemyBlueprint>();


        public Game1()
        {
            use = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Makes window fullscreen. Looks nice, but takes an extra second or two to launch, so disable it to make your life easier when testing.
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadAllEnemyBlueprints();
            LoadAllImages();
            RoomManager.LoadRoomFiles();
            start = new Button(LoadedImages["start_button"], 500, 650, 200, 100);
            start.OnButtonClick += Start_OnButtonClick;
            text = Content.Load<SpriteFont>("Text");
            arial12 = Content.Load<SpriteFont>("arial12");
        }

        private void Start_OnButtonClick(Button btn)
        {
            GameManager.State = States.Round;
            btn.Destroy();
        }

        private void LoadAllImages()
        {
            string[] files = Directory.GetFiles("content\\Images");
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Remove(0, "Content\\".Length).Replace(".xnb", "");
                Texture2D texture = Content.Load<Texture2D>(fileName);
                LoadedImages.Add(fileName.Remove(0, "Images\\".Length), texture);
            }
        }

        /// <summary>
        /// Load all the Enemy blueprints into the game
        /// </summary>
        private void LoadAllEnemyBlueprints()
        {
            string[] enemyFolders = Directory.GetDirectories("content\\Enemies");
            for (int i = 0; i < enemyFolders.Length; i++)
            {
                // This is a local path from the content directory.
                string path = enemyFolders[i] + "\\Animator\\";
                // This is the entire path from the drive to the directory of enemies.
                string fullpath = Environment.CurrentDirectory + "\\" + path;

                // This is all the text from the 'data.txt' file that an enemy
                // being imported will hold.
                string enemyRawData = File.ReadAllText(fullpath + "data.txt");
                // This is all that data being broke up by there line breaks
                string[] enemyData = enemyRawData.Split('\n');

                // Here we are importing all the data from the 'data.txt'
                EnemyBlueprint enemyBlueprint = new EnemyBlueprint(enemyData[0].Split(':')[1].Trim());
                float.TryParse(enemyData[1].Split(':')[1].Trim(), out enemyBlueprint.Damage);
                float.TryParse(enemyData[2].Split(':')[1].Trim(), out enemyBlueprint.AttackSpeed);
                float.TryParse(enemyData[3].Split(':')[1].Trim(), out enemyBlueprint.MoveSpeed);
                float.TryParse(enemyData[4].Split(':')[1].Trim(), out enemyBlueprint.Health);

                // for loop for every animation folder
                string[] enemyInfoFolders = Directory.GetDirectories(path);
                for (int a = 0; a < enemyInfoFolders.Length; a++)
                {
                    // get all the files in the animation folder
                    string[] animationPNGFiles = Directory.GetFiles(enemyInfoFolders[a]);
                    Animation animation = new Animation();
                    // new array (minus 1 to account for the .txt file)
                    animation.Sprites = new Texture2D[animationPNGFiles.Length - 1];
                    for (int z = 0; z < animationPNGFiles.Length; z++)
                    {
                        // make sure that this file is an image
                        if (animationPNGFiles[z].Contains(".xnb"))
                        {
                            // format file path
                            string fileName = animationPNGFiles[z].Remove(0, "Content\\".Length).Replace(".xnb", "");
                            // load and save Texture2D
                            Texture2D sprite = Content.Load<Texture2D>(fileName);
                            animation.Sprites[z] = sprite;
                        }
                    }
                    // read the animation data file
                    string[] animationData = File.ReadAllText(enemyInfoFolders[a] + "\\AnimationData.txt").Split('\n');
                    // parse the animation speed from the file
                    float.TryParse(animationData[0].Split(':')[1].Trim(), out animation.Speed);

                    // add this animation to this enemy blueprint
                    enemyBlueprint.Animations.Add(animation);
                }

                LoadedEnemyBlueprints.Add(enemyBlueprint.Name.ToLower(), enemyBlueprint);
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void BeginRun()
        {
            // Create the rooms
            RoomManager.CreateRooms();

            GameManager.p1 = new Player(Players.Player1);
            GameManager.p1.Position = new Vector2(300f, 450f);

            GameManager.p2 = new Player(Players.Player2);
            GameManager.p2.Position = new Vector2(800f, 450f);

            /*GameObject shopkeeper = new GameObject();
            shopkeeper.Position = new Vector2(550f, 60f);
            SpriteRenderer renderer = shopkeeper.AddComponent<SpriteRenderer>();
            renderer.Sprite = Game1.LoadedImages["shopkeeper"];*/
        }

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update Time
            Time.CurrentGameTime = gameTime;

            // Update the keystate
            Input.UpdateToNextKeyState(Keyboard.GetState());

            GameManager.UpdateState();
            // Updates all active gameobjects
            GameObject.UpdateAll();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin();
            switch (GameManager.State)
            {
                case States.Menu:
                    spriteBatch.Draw(LoadedImages["krawl_title"], new Rectangle(0, 0, 1200, 800), Color.White);
                    start.Draw(spriteBatch);
                    break;

				default:
					// This will draw every gameobject that has a SpriteRenderer attached.
					GameObject.DrawAll(spriteBatch);
                    spriteBatch.DrawString(text, "Gold: " + GameManager.p1.Gold, new Vector2(275, 30), Color.Yellow);
                    spriteBatch.DrawString(text, "Gold: " + GameManager.p2.Gold, new Vector2(825, 30), Color.Yellow);
                    spriteBatch.DrawString(text, "Round: " + GameManager.Round, new Vector2(550, 30), Color.White);


					spriteBatch.DrawString(arial12, GameManager.p1Stats, new Vector2(10, 85), Color.Cyan);
					spriteBatch.DrawString(arial12, GameManager.p2Stats, new Vector2(1075, 85), Color.Cyan);

					if (GameManager.SecondsLeftInShop > 0)
                        spriteBatch.DrawString(text, "Time left in shop: " + (int)GameManager.SecondsLeftInShop, new Vector2(500, 50), Color.HotPink);
                    if (GameManager.p1.ItemOver != null)
                    {
                        spriteBatch.DrawString(arial12, GameManager.p1.ItemOver.ToString(), new Vector2(GameManager.p1.ItemOver.Position.X - 10, GameManager.p1.ItemOver.Position.Y + 50), Color.White);
                    }
                    if (GameManager.p2.ItemOver != null)
                    {
                        spriteBatch.DrawString(arial12, GameManager.p2.ItemOver.ToString(), new Vector2(GameManager.p2.ItemOver.Position.X - 10, GameManager.p2.ItemOver.Position.Y + 50), Color.White);
                    }
                    if (GameManager.State == States.GameOver)
                    {
                        if(GameManager.p1.HP > 0)
                            spriteBatch.DrawString(text, "Player 1 wins!", new Vector2(500, 600), Color.HotPink);
                        else
                            spriteBatch.DrawString(text, "Player 2 wins!", new Vector2(500, 600), Color.HotPink);
                    }

                    if (GameManager.P1ShopOpen)
                    {
                        spriteBatch.DrawString(arial12, GameManager.p1.WeaponHolding.ToString(), new Vector2(50, 650), Color.LightGreen);
                        if (GameManager.p1.ArmorWearing != null)
                            spriteBatch.DrawString(arial12, GameManager.p1.ArmorWearing.ToString(), new Vector2(200, 650), Color.LightGreen);
                        if (GameManager.p1.BootsWearing != null)
                            spriteBatch.DrawString(arial12, GameManager.p1.BootsWearing.ToString(), new Vector2(350, 650), Color.LightGreen);
                    }
                    if (GameManager.P2ShopOpen)
                    {
                        spriteBatch.DrawString(arial12, GameManager.p2.WeaponHolding.ToString(), new Vector2(50 + 600, 650), Color.LightGreen);
                        if (GameManager.p2.ArmorWearing != null)
                            spriteBatch.DrawString(arial12, GameManager.p2.ArmorWearing.ToString(), new Vector2(200 + 600, 650), Color.LightGreen);
                        if (GameManager.p2.BootsWearing != null)
                            spriteBatch.DrawString(arial12, GameManager.p2.BootsWearing.ToString(), new Vector2(350 + 600, 650), Color.LightGreen);
                    }

                    GameManager.totalArmorP1 = 0;
                    GameManager.totalArmorP2 = 0;
                    GameManager.totalDamageP1 = 0;
                    GameManager.totalDamageP2 = 0;
                    GameManager.totalMoveSpeedP1 = GameManager.p1.MovementSpeed;
                    GameManager.totalMoveSpeedP2 = GameManager.p2.MovementSpeed;

                    if (GameManager.p1.BootsWearing != null) GameManager.totalArmorP1 += GameManager.p1.BootsWearing.BonusArmor;
                    if (GameManager.p1.ArmorWearing != null) GameManager.totalArmorP1 += GameManager.p1.ArmorWearing.BonusArmor + GameManager.p1.ArmorWearing.ArmorAmt;
                    if (GameManager.p1.WeaponHolding != null) GameManager.totalArmorP1 += GameManager.p1.WeaponHolding.BonusArmor;

                    if (GameManager.p2.ArmorWearing != null) GameManager.totalArmorP2 += GameManager.p2.ArmorWearing.BonusArmor + GameManager.p2.ArmorWearing.ArmorAmt;
                    if (GameManager.p2.BootsWearing != null) GameManager.totalArmorP2 += GameManager.p2.BootsWearing.BonusArmor;
                    if (GameManager.p2.WeaponHolding != null) GameManager.totalArmorP2 += GameManager.p2.WeaponHolding.BonusArmor;

                    if (GameManager.p1.ArmorWearing != null) GameManager.totalDamageP1 += GameManager.p1.ArmorWearing.BonusDamage;
                    if (GameManager.p1.BootsWearing != null) GameManager.totalDamageP1 += GameManager.p1.BootsWearing.BonusDamage;
                    if (GameManager.p1.WeaponHolding != null) GameManager.totalDamageP1 += GameManager.p1.WeaponHolding.BonusDamage + GameManager.p1.WeaponHolding.Damage;

                    if (GameManager.p2.ArmorWearing != null) GameManager.totalDamageP2 += GameManager.p2.ArmorWearing.BonusDamage;
                    if (GameManager.p2.BootsWearing != null) GameManager.totalDamageP2 += GameManager.p2.BootsWearing.BonusDamage;
                    if (GameManager.p2.WeaponHolding != null) GameManager.totalDamageP2 += GameManager.p2.WeaponHolding.BonusDamage + GameManager.p2.WeaponHolding.Damage; ;

                    if (GameManager.p1.ArmorWearing != null) GameManager.totalMoveSpeedP1 += GameManager.p1.ArmorWearing.BonusMoveSpeed;
                    if (GameManager.p1.BootsWearing != null) GameManager.totalMoveSpeedP1 += GameManager.p1.BootsWearing.BonusMoveSpeed + GameManager.p1.BootsWearing.MoveSpeed;
                    if (GameManager.p1.WeaponHolding != null) GameManager.totalMoveSpeedP1 += GameManager.p1.WeaponHolding.BonusMoveSpeed;

                    if (GameManager.p2.WeaponHolding != null) GameManager.totalMoveSpeedP2 += GameManager.p2.WeaponHolding.BonusMoveSpeed;
                    if (GameManager.p2.ArmorWearing != null) GameManager.totalMoveSpeedP2 += GameManager.p2.ArmorWearing.BonusMoveSpeed;
                    if (GameManager.p2.BootsWearing != null) GameManager.totalMoveSpeedP2 += GameManager.p2.BootsWearing.BonusMoveSpeed + GameManager.p2.BootsWearing.MoveSpeed;

                    GameManager.p1Stats = "Damage: " + GameManager.totalDamageP1 + "\n" + "Armor: " + GameManager.totalArmorP1 + "\n" + "MoveSpeed: " + GameManager.totalMoveSpeedP1;
                    GameManager.p2Stats = "Damage: " + GameManager.totalDamageP2 + "\n" + "Armor: " + GameManager.totalArmorP2 + "\n" + "MoveSpeed: " + GameManager.totalMoveSpeedP2;

                    break;
			}
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
