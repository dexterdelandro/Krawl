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
    class Projectile : GameObject
    {
        public Rigidbody TheRigidbody;
        public float Damage;
        public float LifeTime;
        public bool IsGood; // This means if its good it will only harm enemies if its bad it will only harm players

        //private FacingDirection kbDirection;
        private float lifeTimeLeft;

        private Projectile(float damage, /*FacingDirection d,*/ float lifeTime)
        {
            Damage = damage;
            //kbDirection = d;
            LifeTime = lifeTime;
            lifeTimeLeft = LifeTime;
        }

        public override void Update()
        {
            lifeTimeLeft -= Time.DeltaTime;
            if (lifeTimeLeft <= 0)
                Destroy();

            base.Update();
        }

        private void LaunchProjectile()
        {
            TheRigidbody = GetComponent<Rigidbody>();
            GetComponent<BoxCollider>().OnCollisionEnter += Projectile_OnCollisionEnter;
        }

        private void Projectile_OnCollisionEnter(object otherCollider)
        {
            GameObject hitGameObject = ((BoxCollider)otherCollider).ConnectedGameObject;
            if ((hitGameObject is Player && !IsGood) || (hitGameObject is Enemy && IsGood))
            {
                LivingEntity entity = hitGameObject as LivingEntity;
                if (!entity.invincible)
                {
                    entity.HP -= Damage;
                    if (entity.HP <= 0)
                    {
                        Player targetHolder = (Player)((Enemy)entity).Target;
                        targetHolder.ComboManager();
                        targetHolder.Gold = targetHolder.Gold + (entity.Gold * targetHolder.combo);
                        if (!GameManager.EnemiesP1.Remove((Enemy)entity)) GameManager.EnemiesP2.Remove((Enemy)entity);
                        entity.Destroy();

                    }
                    else
                    {
                        //switch (kbDirection)
                        //{
                        //    case FacingDirection.Up:
                        //        Position = new Vector2(Position.X, Position.Y - 30);
                        //        break;
                        //    case FacingDirection.Left:
                        //        Position = new Vector2(Position.X - 30, Position.Y);
                        //        break;
                        //    case FacingDirection.Right:
                        //        Position = new Vector2(Position.X + 30, Position.Y);
                        //        break;
                        //    case FacingDirection.Down:
                        //        Position = new Vector2(Position.X, Position.Y + 30);
                        //        break;
                        //}
                    }
                }

                Destroy();
            }
            else if (!(hitGameObject is LivingEntity))
                Destroy();
        }


        /// <summary>
        /// Spawn Projectile!
        /// </summary>
        /// <param name="spawnPos">Spawn position of projectile.</param>
        /// <param name="startVelocity">Starting velocity.</param>
        /// <param name="damage">Damage of projectile.</param>
        /// <returns>Returns the created projectile.</returns>
        public static Projectile CreateProjectile(Vector2 spawnPos, Vector2 startVelocity, Vector2 scaleOfProjectile, float damage/*, FacingDirection d*/, float lifeTime, bool isGood)
        {
            Projectile projectile = new Projectile(damage, /*d,*/ lifeTime);
            SpriteRenderer spriteRenderer = projectile.AddComponent<SpriteRenderer>();
            spriteRenderer.Sprite = Game1.LoadedImages["blankProjectile"];
            spriteRenderer.DrawLayer = 3;
            projectile.PositionCenter = spawnPos;
            projectile.Scale = scaleOfProjectile;
            projectile.IsGood = isGood;
            BoxCollider box = projectile.AddComponent<BoxCollider>();
            box.Blocks = false;
            Rigidbody rigidbody = projectile.AddComponent<Rigidbody>();
            rigidbody.Velocity = startVelocity;
            rigidbody.FrictionContant = 0;

            projectile.LaunchProjectile();
            return projectile;
        }

        public static Vector2 DirectionToVector2(FacingDirection direction)
        {
            switch (direction)
            {
                case FacingDirection.Up:
                    return new Vector2(0, -1);
                case FacingDirection.Down:
                    return new Vector2(0, 1);
                case FacingDirection.Left:
                    return new Vector2(-1, 0);
                case FacingDirection.Right:
                    return new Vector2(1, 0);
                default:
                    return Vector2.Zero;
            }
        }
    }
}
