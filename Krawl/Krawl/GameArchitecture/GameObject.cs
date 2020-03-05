using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Krawl.GameArchitecture
{
    public class GameObject
    {

        #region Variables
        /// <summary>
        /// Collection of all active GameObjects
        /// </summary>
        public static List<GameObject> AllGameObjects = new List<GameObject>();

        private SpriteRenderer spriteRenderer;
        private List<Component> attachedComponents = new List<Component>();

        private Vector2 position = Vector2.Zero;
        private float rotation = 0f;
        private Vector2 scale = Vector2.One;
        private string name;
        #endregion

        #region Properties
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 PositionCenter
        {
            get => new Vector2(position.X + ((SpriteRend.Sprite.Width * scale.X) / 2f), position.Y + ((SpriteRend.Sprite.Height * scale.Y) / 2f));
            set => position = new Vector2(value.X - ((SpriteRend.Sprite.Width * scale.X) / 2f), value.Y - ((SpriteRend.Sprite.Height * scale.Y) / 2f));
        }
        public float Rotation { get => rotation; set => rotation = value; }
        public Vector2 Scale { get => scale; set => scale = value; }
        public SpriteRenderer SpriteRend { get => spriteRenderer; set => spriteRenderer = value; }
        public string Name { get => name; set => name = value; }
        #endregion

        public GameObject()
        {
            AllGameObjects.Add(this);
            Start();
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

            foreach (Component comp in attachedComponents)
                comp.Update();
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (SpriteRend != null)
            {
                SpriteRend.Draw(batch);
            }
        }

        /// <summary>
        /// Get component by type.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (Component comp in attachedComponents)
                if (comp is T)
                    return comp as T;
            return null;
        }

        /// <summary>
        /// Add a Component to this Gameobject.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <returns></returns>
        public T AddComponent<T>() where T : Component, new()
        {
            return AddComponent(new T());
        }

        /// <summary>
        /// Add a Component to this Gameobject.
        /// </summary>
        /// <typeparam name="T">Type of Component.</typeparam>
        /// <param name="component">The Component being added.</param>
        public T AddComponent<T>(T component) where T : Component
        {
            if (component is SpriteRenderer)
                SpriteRend = component as SpriteRenderer;
            attachedComponents.Add(component);

            component.ConnectedGameObject = this;
            component.Start();

            return component;
        }

        /// <summary>
        /// Gets all components, that are attached to GameObjects, of a type.
        /// </summary>
        /// <typeparam name="T">The type of Component.</typeparam>
        /// <returns></returns>
        public static List<T> GetAllComponentsOfType<T>() where T : Component
        {
            List <T> listOfComp = new List<T>();
            foreach(GameObject go in AllGameObjects)
                foreach(Component co in go.attachedComponents)
                    if (co is T)
                        listOfComp.Add(co as T);
            return listOfComp;
        }

        /// <summary>
        /// Draws all GameObjects that have SpriteRenderer's attached.
        /// </summary>
        /// <param name="batch">The active SpriteBatch.</param>
        public static void DrawAll(SpriteBatch batch)
        {
            // We create a sorted list here:
            // We are sorting by the draw layer, if something is ordered futher back (closer to zero) 
            // then we will draw that first, and draw objects in order of their draw layer
            BetterSortedList<int, GameObject> gameObjectsToDrawSorted = new BetterSortedList<int, GameObject>();
            foreach (GameObject go in AllGameObjects)
            {
                // Making sure we only want to order (and draw) stuff that can be drawn (Having SpriteRenderer)
                if (go.SpriteRend != null)
                    gameObjectsToDrawSorted.Add(go.SpriteRend.DrawLayer, go);
            }

            gameObjectsToDrawSorted.Sort();

            foreach (Tuple<int, GameObject> goPair in gameObjectsToDrawSorted)
            {
                goPair.Item2.SpriteRend.Draw(batch);
            }
        }

        /// <summary>
        /// Destorys This GameObject.
        /// </summary>
        public void Destroy()
        {
            AllGameObjects.Remove(this);
        }

        /// <summary>
        /// Update all active GameObjects.
        /// </summary>
        /// <param name="gt">The game time.
        /// Use ElapsedGameTime.TotalSeconds to get delta time
        /// </param>
        public static void UpdateAll()
        {
            for (int i = 0; i < AllGameObjects.Count; i++)
            {
                GameObject go = AllGameObjects[i];
                go.Update();
            }
        }
    }
}
