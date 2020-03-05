using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Krawl.GameplayArchitecture
{
	public enum Players { Player1, Player2 }
    class Player : LivingEntity
    {
        //Fields
        public HealthBar TheHealthBar;
        public Weapon WeaponHolding;
        public Boots BootsWearing;
        public Armor ArmorWearing;
        private Players playerNum;
        private FacingDirection direction;
        private int attackTimer = 6;
        private int invincibleTimer = 91;
        int animTest = 0; // This is an interger used in the Update method and the AnimationUpdate method. 
                          // This integer basically represents frames in the animation updater
                          // *NOTE*
                          // Player animation can now be done in the Animator class rather then having a huge else-if block -Josh
        Rigidbody rigidbody;
        private const float COMBO_TIME = 1.5f;
        private float timeSinceLastKill = 0;
        public int combo = 1;
        public Item ItemOver;

        /// <summary>
        /// Returns 0 for player 1, 1 for player 2. I know, I know. This is so that the functionality doesn't break.
        /// </summary>
        public Players PlayerNum
        {
            get { return playerNum; }
        }

        //Constructors
        public Player(Players playerNum)
        {
            this.playerNum = playerNum;
            MaxHP = 100;
            HP = MaxHP;

            Scale *= 1.5f;
            AddComponent<SpriteRenderer>();
            SpriteRend.DrawLayer = 4;

            if (playerNum == Players.Player1)
            {
                //player 1
                SpriteRend.Sprite = Game1.LoadedImages["knight1_idle_1"];
            }
            else {
                //player 2
                SpriteRend.Sprite = Game1.LoadedImages["knight2_idle_1"];
            }

            BoxCollider boxCollider = AddComponent<BoxCollider>();
            boxCollider.OnCollisionEnter += BoxCollider_OnCollisionEnter;
            boxCollider.OnCollisionExit += BoxCollider_OnCollisionExit;

            rigidbody = AddComponent<Rigidbody>();
            rigidbody.FrictionContant = 0.2f;

            MovementSpeed /= 8;

            TheHealthBar = new HealthBar(this, playerNum);

            // Starting Weapon
            WeaponHolding = Weapon.WeaponPrefabs(Weapons.WeakSword);
        }

        private void BoxCollider_OnCollisionExit(object otherCollider)
        {
            if (((BoxCollider)otherCollider).ConnectedGameObject is Item)
            {
                ItemOver = null;
            }
        }

        private void BoxCollider_OnCollisionEnter(object otherCollider)
        {
            if (((BoxCollider)otherCollider).ConnectedGameObject is Item)
            {
                ItemOver = ((BoxCollider)otherCollider).ConnectedGameObject as Item;
            }
        }

        public override void Update()
        {
            //Console.WriteLine(Position.X + " : " + Position.Y + " : ");

            if (ItemOver != null)
            {
                if (Input.GetKeyDown(Keys.E) && playerNum == Players.Player1)
                    ItemOver.PickupItem(GameManager.p1);
                if (Input.GetKeyDown(Keys.NumPad0) && playerNum == Players.Player2)
                    ItemOver.PickupItem(GameManager.p2);
            }

            MovementUpdate();
            AttackUpdate();
            animTest += 1;
            AnimationUpdate();
            base.Update();
        }

        private void MovementUpdate()
        {
            if (invincibleTimer < 91)
            {
                invincibleTimer++;
            }
            else
            {
                invincible = false;
            }
            if (playerNum == Players.Player1)
            {
                float adjustedMoveSpeed = MovementSpeed;
                if (BootsWearing != null)
                    adjustedMoveSpeed += BootsWearing.MoveSpeed + BootsWearing.BonusMoveSpeed;
                if (ArmorWearing != null)
                    adjustedMoveSpeed += ArmorWearing.BonusMoveSpeed;
                if (WeaponHolding != null)
                    adjustedMoveSpeed += WeaponHolding.BonusMoveSpeed;
                //FOR PLAYER 1
                if (Input.GetKey(Keys.W))
                {
                    rigidbody.Velocity.Y -= MovementSpeed;
                    direction = FacingDirection.Up;
                }
                if (Input.GetKey(Keys.S))
                {
                    rigidbody.Velocity.Y += MovementSpeed;
                    direction = FacingDirection.Down;
                }
                if (Input.GetKey(Keys.D))
                {
                    rigidbody.Velocity.X += MovementSpeed;
                    direction = FacingDirection.Right;
                }
                if (Input.GetKey(Keys.A))
                {
                    rigidbody.Velocity.X -= MovementSpeed;
                    direction = FacingDirection.Left;
                }
            }
            else {
                float adjustedMoveSpeed = MovementSpeed;
                if (BootsWearing != null)
                    adjustedMoveSpeed += BootsWearing.MoveSpeed;

                //FOR PLAYER 2
                if (Input.GetKey(Keys.Up))
                {
                    rigidbody.Velocity.Y -= MovementSpeed;
                    direction = FacingDirection.Up;
                }
                if (Input.GetKey(Keys.Down))
                {
                    rigidbody.Velocity.Y += MovementSpeed;
                    direction = FacingDirection.Down;
                }
                if (Input.GetKey(Keys.Right))
                {
                    rigidbody.Velocity.X += MovementSpeed;
                    direction = FacingDirection.Right;
                }
                if (Input.GetKey(Keys.Left))
                {
                    rigidbody.Velocity.X -= MovementSpeed;
                    direction = FacingDirection.Left;
                }
            }
            if (GameManager.State == States.Round)
            {
                timeSinceLastKill += Time.DeltaTime;
            }
        }

        private void AnimationUpdate()
        {
            if (playerNum == Players.Player1)
            {
                //FOR PLAYER1
                if (Input.GetKey(Keys.W) == true || Input.GetKey(Keys.S) == true || Input.GetKey(Keys.D) == true || Input.GetKey(Keys.A) == true)
                { //If the player is moving, run the sprites representing "Running"
                    if (animTest % 20 <= 4)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_run_1"];
                    }
                    else if (animTest % 20 > 4 && animTest % 20 <= 9)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_run_2"];
                    }
                    else if (animTest % 20 > 9 && animTest % 20 <= 14)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_run_3"];
                    }
                    else if (animTest % 20 > 14 && animTest % 20 <= 19)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_run_4"];
                    }
                }
                else
                { //If the player is not moving, run the "Idle" sprites

                    if (animTest % 20 <= 4)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_idle_1"];
                    }
                    else if (animTest % 20 > 4 && animTest % 20 <= 9)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_idle_2"];
                    }
                    else if (animTest % 20 > 9 && animTest % 20 <= 14)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_idle_3"];
                    }
                    else if (animTest % 20 > 14 && animTest % 20 <= 19)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight1_idle_4"];
                    }
                }
            }
            else {
                //FOR PLAYER 2
                if (Input.GetKey(Keys.Up) == true || Input.GetKey(Keys.Down) == true || Input.GetKey(Keys.Right) == true || Input.GetKey(Keys.Left) == true)
                { //If the player is moving, run the sprites representing "Running"
                    if (animTest % 20 <= 4)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_run_1"];
                    }
                    else if (animTest % 20 > 4 && animTest % 20 <= 9)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_run_2"];
                    }
                    else if (animTest % 20 > 9 && animTest % 20 <= 14)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_run_3"];
                    }
                    else if (animTest % 20 > 14 && animTest % 20 <= 19)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_run_4"];
                    }
                }
                else
                { //If the player is not moving, run the "Idle" sprites

                    if (animTest % 20 <= 4)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_idle_1"];
                    }
                    else if (animTest % 20 > 4 && animTest % 20 <= 9)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_idle_2"];
                    }
                    else if (animTest % 20 > 9 && animTest % 20 <= 14)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_idle_3"];
                    }
                    else if (animTest % 20 > 14 && animTest % 20 <= 19)
                    {
                        SpriteRend.Sprite = Game1.LoadedImages["knight2_idle_4"];
                    }
                }
            }
        }

        /// <summary>
        /// Runs the numbers on whether or not a player attacks.
        /// </summary>
        private void AttackUpdate()
        {
            WeaponHolding.Update();

            if (playerNum == Players.Player1)
            {
                if (Input.GetKey(Keys.E)) 
                {
                    Vector2 directionVector2 = Projectile.DirectionToVector2(direction) * 2;
                    WeaponHolding.Attack(this, directionVector2);
                }
            }
            else
            {
                //FOR PLAYER 2
                if (Input.GetKey(Keys.NumPad0))
                {
                    Vector2 directionVector2 = Projectile.DirectionToVector2(direction) * 2;
                    WeaponHolding.Attack(this, directionVector2);
                }
            }
        }

        public void ComboManager()
        {
            if(timeSinceLastKill < COMBO_TIME)
            {
                combo++;
            }
            else
            {
                combo = 1;
            }
        }
    }
}