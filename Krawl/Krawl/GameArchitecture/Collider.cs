using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Krawl.GameplayArchitecture;

namespace Krawl.GameArchitecture
{
    public delegate void CollisionEnterExit(object otherCollider);

    abstract class Collider : Component
    {
        public bool Blocks = true;

        public event CollisionEnterExit OnCollisionEnter;
        public event CollisionEnterExit OnCollisionExit;

        public List<Collider> CurrentlyCollidingWith = new List<Collider>();

        private void OnCollEnter(Collider other)
        {
            OnCollisionEnter?.Invoke(other);
        }

        private void OnCollExit(Collider other)
        {
            OnCollisionExit?.Invoke(other);
        }

        private void Collide(Collider other)
        {
            CollideBlocking(other);

            if (!CurrentlyCollidingWith.Contains(other))
            {
                CurrentlyCollidingWith.Add(other);
                OnCollEnter(other);

            }
        }

        public void CollideBlocking(Collider other)
        {
            if (!Blocks || !other.Blocks)
                return;
            if (ConnectedGameObject is Player && other.ConnectedGameObject is Player)
                return;

            Rigidbody rigidbody = ConnectedGameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                float differenceX = ConnectedGameObject.PositionCenter.X - other.ConnectedGameObject.PositionCenter.X;
                float differenceY = ConnectedGameObject.PositionCenter.Y - other.ConnectedGameObject.PositionCenter.Y;

                bool isVertical = Math.Abs(differenceY) > Math.Abs(((other.ConnectedGameObject.SpriteRend.Sprite.Height * other.ConnectedGameObject.Scale.Y) / 2));

                float bouncePower = 2;

                if (!isVertical)
                {
                    //player moving left
                    if (Math.Floor(rigidbody.Velocity.X) < 0 && differenceX > 0)
                    {
                        rigidbody.Velocity.X = rigidbody.Velocity.X * -bouncePower;
                        //rigidbody.Velocity.X = Math.Max(0, rigidbody.Velocity.X);
                    }
                    //player moving right
                    else if (Math.Floor(rigidbody.Velocity.X) > 0 && differenceX < 0)
                    {
                        //rigidbody.Velocity.X = Math.Min(0, rigidbody.Velocity.X);
                        rigidbody.Velocity.X = rigidbody.Velocity.X * -bouncePower;
                    }
                }
                else
                {

                    //player moving down
                    if (Math.Floor(rigidbody.Velocity.Y) > 0 && differenceY < 0)
                    {
                        rigidbody.Velocity.Y = rigidbody.Velocity.Y * -bouncePower;
                        //rigidbody.Velocity.Y = Math.Min(0, rigidbody.Velocity.Y);
                    }
                    //player moving up
                    else if (Math.Floor(rigidbody.Velocity.Y) < 0 && differenceY > 0)
                    {
                        //rigidbody.Velocity.Y = Math.Max(0, rigidbody.Velocity.Y);
                        rigidbody.Velocity.Y = rigidbody.Velocity.Y * -bouncePower;
                    }


                }


                //Vector2 normal = new Vector2(-1, 0);

                ////player moving left
                //if (Math.Floor(rigidbody.Velocity.X) < 0 && differenceX > 0)
                //    normal = new Vector2(1, 0);
                ////player moving right
                //if (Math.Floor(rigidbody.Velocity.X) > 0 && differenceX < 0)
                //    normal = new Vector2(-1, 0);
                ////player moving up
                //if (Math.Floor(rigidbody.Velocity.Y) < 0 && differenceY > 0)
                //    normal = new Vector2(0, 1);
                ////player moving down
                //if (Math.Floor(rigidbody.Velocity.Y) > 0 && differenceY < 0)
                //    normal = new Vector2(0, -1);


                //Vector2 normalizedVector = new Vector2(differenceX, differenceY);
                //normalizedVector.Normalize();

                //Vector2 R = normalizedVector;

                //R = -2 * (Vector2.Dot(normalizedVector, normal)) * normalizedVector + rigidbody.Velocity;

                //rigidbody.Velocity = R * 2;


                //Vector2 normalizedVector = new Vector2(differenceX, differenceY);


                ////player moving left
                //if (Math.Floor(rigidbody.Velocity.X) < 0 && differenceX > 0)
                //{
                //    normalizedVector.X = -rigidbody.Velocity.X;
                //}
                ////player moving right
                //if (Math.Floor(rigidbody.Velocity.X) > 0 && differenceX < 0)
                //{
                //    normalizedVector.X = Math.Min(0, normalizedVector.X);
                //}

                ////player moving down
                //if (Math.Floor(rigidbody.Velocity.Y) > 0 && differenceY < 0)
                //{
                //    normalizedVector.Y = Math.Min(0, normalizedVector.Y);
                //}
                ////player moving up
                //if (Math.Floor(rigidbody.Velocity.Y) < 0 && differenceY > 0)
                //{
                //    normalizedVector.Y = Math.Max(0, normalizedVector.Y);
                //}

                //normalizedVector.Normalize();

                //rigidbody.Velocity = normalizedVector * 5;
            }
        }

        public override void Update()
        {
            List<Collider> collidingWithThisTick = new List<Collider>();

            List<Collider> colliders = GameObject.GetAllComponentsOfType<Collider>();
            for (int i = 0; i < colliders.Count; i++)
            {
                Collider co = colliders[i];

                if (co != this && IntersectsWith(co))
                {
                    Collide(co);
                    collidingWithThisTick.Add(co);
                }
            }

            for (int i = 0; i < CurrentlyCollidingWith.Count; i++)
            {
                if (!collidingWithThisTick.Contains(CurrentlyCollidingWith[i]))
                {
                    OnCollExit(CurrentlyCollidingWith[i]);
                    CurrentlyCollidingWith.RemoveAt(i--);
                }
            }
        }

        private bool IntersectsWith(Collider other)
        {
            GameObject otherGo = other.ConnectedGameObject;
            GameObject go = ConnectedGameObject;
            SpriteRenderer otherSpriteRenderer = otherGo.GetComponent<SpriteRenderer>();
            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();

            return spriteRenderer.Rec.Intersects(otherSpriteRenderer.Rec);
        }
    }
}
