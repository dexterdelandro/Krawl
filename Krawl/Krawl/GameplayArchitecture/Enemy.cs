using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Krawl.GameplayArchitecture
{
    public enum EnemyState { Attacking = 0, Idle = 1, Moving = 2 }
    public enum MovementType //The different types of movement that enemies can have.
    {
        Demon,
        Goblin,
        Zombie
    }
    class Enemy : LivingEntity
    {
        //Fields
        private LivingEntity target; //The target of the enemy
        private MovementType type; //The type of the enemy.
        private EnemyState currentState;
        private int warpTimer;
        private Vector2 warpLocation;

        private float attackTimeLeft;

        //Properties
        /// <summary>
        /// Allows changing the target of the enemy.
        /// </summary>
        public LivingEntity Target
        {
            get { return target;  }
            set { target = value; }
        }
        public EnemyState CurrentState
        {
            get => currentState; set
            {
                // if it has changed (Fix Needed)
                //if (currentState != value)
                {
                    Animator animator = GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.CurrentAnimation = animator.Animations[(int)CurrentState];
                        currentState = value;
                    }
                }
            }
        }

        //Constructors
        /// <summary>
        /// Creates an enemy with no target.
        /// </summary>
        /// <param name="t">The type of the enemy.</param>
        public Enemy(MovementType t) : this(null, t)
        { }
        /// <summary>
        /// Creates an enemy with a target.
        /// </summary>
        /// <param name="target">The target of the enemy.</param>
        /// <param name="t">The type of enemy.</param>
        public Enemy(LivingEntity target, MovementType t)
        {
            this.target = target;
            type = t;
        }

        //Methods
        public override void Update()
        {
            //if (GameManager.State != States.Round)
            //    return;

            if (attackTimeLeft > 0)
                attackTimeLeft -= Time.DeltaTime;
            else
            {
                if (currentState == EnemyState.Attacking)
                    Target.DealDamage(Damage);
                attackTimeLeft = 1 / AttackSpeed;
            }

            // If close enough to target then start dealing damage
            if (Vector2.Distance(PositionCenter, Target.PositionCenter) < 50)
            {
                //Attacking
                CurrentState = EnemyState.Attacking;
            }
            else
            {
                //Moving
                CurrentState = EnemyState.Moving;
                MoveAI();
            }
            base.Update();
        }
        /// <summary>
        /// Deduct health from the enemy. Destroy the enemy if it's health is less than zero, or give it knockback otherwise.
        /// </summary>
        /// <param name="damage">The damage the enemy will take.</param>
        /// <param name="kbDir">The direction of the knockback.</param>
        public void TakeDamage(int damage, FacingDirection kbDir)
        {

			if (!invincible)//Ensures the enemy doesn't get melted when attacked.
            {
				HP -= damage;
                if (HP <= 0)
				{
                    Destroy();
				}

				switch (kbDir)
                {
                    case FacingDirection.Up:
                        Position = new Vector2(Position.X, Position.Y - 30);
                        break;
                    case FacingDirection.Left:
                        Position = new Vector2(Position.X - 30, Position.Y);
                        break;
                    case FacingDirection.Right:
                        Position = new Vector2(Position.X + 30, Position.Y);
                        break;
                    case FacingDirection.Down:
                        Position = new Vector2(Position.X, Position.Y + 30);
                        break;
                }
                invincible = true;
            }
        }
        /// <summary>
        /// This is called every tick and is the AI that controls the enemy's movement
        /// </summary>
        public virtual void MoveAI()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            switch (type)
            {
                case MovementType.Demon: //Constantly runs at the player.
                    if (Position.Y < target.Position.Y)
                    {
                        rigidbody.Velocity.Y += (MovementSpeed * Time.DeltaTime * 4);
                    }
                    if (Position.Y > target.Position.Y)
                    {
                        rigidbody.Velocity.Y -= (MovementSpeed * Time.DeltaTime * 4);
                    }
                    if (Position.X < target.Position.X)
                    {
                        rigidbody.Velocity.X += (MovementSpeed * Time.DeltaTime * 4);
                    }
                    if (Position.X > target.Position.X)
                    {
                        rigidbody.Velocity.X -= (MovementSpeed * Time.DeltaTime * 4);
                    }
                        Position = new Vector2(Position.X - (MovementSpeed * Time.DeltaTime * 1), Position.Y);
                    break;
                case MovementType.Goblin: //Goblins will warp to where the player was 3 seconds ago every 5 seconds.
                    warpTimer++;
                    if(warpTimer == 120)
                    {
                        warpLocation = target.Position;
                    }
                    if(warpTimer == 300)
                    {
                        Position = warpLocation;
                        warpTimer = 0;
                    }
                    break;
                case MovementType.Zombie:
                    //Will be implemented later - John
                    break;
            }
        }
        public static void DealDamage()
        {
            
        }
        public static Enemy CreateEnemyGameObjectFromBlueprint(string enemyName, Vector2 spawnPostion, LivingEntity target)
        {
            return CreateEnemyGameObjectFromBlueprint(enemyName, spawnPostion, target, MovementType.Demon);
        }
        public static Enemy CreateEnemyGameObjectFromBlueprint(string enemyName, Vector2 spawnPostion, LivingEntity target, MovementType move)
        {
            EnemyBlueprint loadedBlueprint = Game1.LoadedEnemyBlueprints[enemyName.ToLower()];

            Enemy enemy = new Enemy(target, move);
            enemy.Scale = new Vector2(1.0f, 1.0f);
            enemy.Position = spawnPostion;
            enemy.AddComponent<BoxCollider>();
            Rigidbody rigid = enemy.AddComponent<Rigidbody>();
            rigid.FrictionContant = 0.2f;
            Animator animator = enemy.AddComponent<Animator>();
            animator.DrawLayer = 1;
            // only 3 animations (move, idle, attack)
            animator.Animations = new Animation[3];
            for (int i = 0; i < animator.Animations.Length; i++)
            {
                Animation animation = loadedBlueprint.Animations[i];
                animator.Animations[i] = new Animation();
                animator.Animations[i].Sprites = new Texture2D[animation.Sprites.Length];
                animator.Animations[i].Sprites = animation.Sprites;

                animator.Animations[i].Speed = loadedBlueprint.Animations[i].Speed;
            }
            animator.CurrentAnimation = animator.Animations[0];

            animator.Sprite = animator.CurrentAnimation.Sprites[0];

			if (enemy.Name == "Grockel" || enemy.Name == "Zombie")
			{
				enemy.Gold = 23;
			}
			else {
				enemy.Gold = 20;
			}
		
            enemy.Name = loadedBlueprint.Name;
            enemy.Damage = loadedBlueprint.Damage;
            enemy.AttackSpeed = loadedBlueprint.AttackSpeed;
            enemy.MovementSpeed = loadedBlueprint.MoveSpeed;
            enemy.MaxHP = loadedBlueprint.Health;

            enemy.HP = enemy.MaxHP;
            enemy.invincible = false;

            enemy.CurrentState = EnemyState.Moving;

            return enemy;
        }
    }
}