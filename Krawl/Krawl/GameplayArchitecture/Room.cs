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
    public enum RoomPrefabType { Player, Shop }

    public class Room
    {
        public RoomPrefabType RoomPrefab;
        public GameObject[,] FloorRoomTiles;

        public Vector2 StartPostion;

        public Room(RoomPrefabType roomPrefabType, Vector2 startPos)
        {
            RoomPrefab = roomPrefabType;
            StartPostion = startPos;

            GenerateRoom();
        }

        public void GenerateRoom()
        {

            switch (RoomPrefab)
            {
                case RoomPrefabType.Player:

                    int roomWidth = 18;
                    int roomHeight = 20;
                    FloorRoomTiles = new GameObject[roomWidth, roomHeight];
                    for (int x = 0; x < roomWidth; x++)
                    {
                        for (int y = 0; y < roomHeight; y++)
                        {
                            if ((x == 0 || y == 0 || (x == roomWidth - 1 || y == roomHeight - 1)))
                            {
                                GameObject tile = new GameObject();
                                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                                tile.Scale = new Vector2(2, 2);

                                if (y == 0)
                                    renderer.Sprite = Game1.LoadedImages["tile00" + (6 + (x % 3))];
                                else if (x == 0 && y == 1)
                                    renderer.Sprite = Game1.LoadedImages["tile113"];
                                else if (x == 0 && y == roomHeight - 1)
                                    renderer.Sprite = Game1.LoadedImages["tile167"];
                                else if (x == roomWidth - 1 && y == 1)
                                    renderer.Sprite = Game1.LoadedImages["tile115"];
                                else if (x == roomWidth - 1 && y == roomHeight - 1)
                                    renderer.Sprite = Game1.LoadedImages["tile169"];
                                else if (x == 0)
                                    renderer.Sprite = Game1.LoadedImages["tile140"];
                                else if (x == roomWidth - 1)
                                    renderer.Sprite = Game1.LoadedImages["tile142"];
                                else if (y == roomHeight - 1)
                                    renderer.Sprite = Game1.LoadedImages["tile168"];
                                tile.Position = StartPostion + new Vector2((x * tile.SpriteRend.Sprite.Width * tile.Scale.X), (y * tile.SpriteRend.Sprite.Height * tile.Scale.Y));
                                renderer.ColorOverlay = Color.White;
                                renderer.DrawLayer = 1;
                                tile.AddComponent<BoxCollider>();

                                FloorRoomTiles[x, y] = tile;
                            }
                            else
                            {
                                GameObject tile = new GameObject();
                                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                                tile.Scale = new Vector2(2, 2);
                                if (y == 1)
                                    renderer.Sprite = Game1.LoadedImages["tile114"];
                                else if (Game1.Rand.Next(2) == 0)
                                    renderer.Sprite = Game1.LoadedImages["tile141"];
                                else
                                    renderer.Sprite = Game1.LoadedImages["tile" + (194 + Game1.Rand.Next(3))];
                                tile.Position = StartPostion + new Vector2((x * tile.SpriteRend.Sprite.Width * tile.Scale.X), (y * tile.SpriteRend.Sprite.Height * tile.Scale.Y));
                                renderer.ColorOverlay = Color.White;
                                renderer.DrawLayer = 0;

                                FloorRoomTiles[x, y] = tile;
                            }
                        }
                    }

                    break;
                case RoomPrefabType.Shop:
                    roomWidth = 30;
                    roomHeight = 5;
                    FloorRoomTiles = new GameObject[roomWidth, roomHeight];
                    for (int x = 0; x < roomWidth; x++)
                    {
                        for (int y = 0; y < roomHeight; y++)
                        {
                            if ((x == 0 || y == 0) || x == roomWidth - 1)
                            {
                                GameObject tile = new GameObject();
                                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                                tile.Scale = new Vector2(2, 2);

                                if (y == 0)
                                    renderer.Sprite = Game1.LoadedImages["tile08" + (4 + (x % 3))];
                                else
                                    renderer.Sprite = Game1.LoadedImages["tile00" + (3 + (x % 3))];

                                tile.Position = StartPostion + new Vector2((x * tile.SpriteRend.Sprite.Width * tile.Scale.X), (y * tile.SpriteRend.Sprite.Height * tile.Scale.Y));
                                renderer.ColorOverlay = Color.White;
                                renderer.DrawLayer = 1;
                                tile.AddComponent<BoxCollider>();

                                FloorRoomTiles[x, y] = tile;
                            }
                            else
                            {
                                GameObject tile = new GameObject();
                                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
                                tile.Scale = new Vector2(2, 2);
                                renderer.Sprite = Game1.LoadedImages["tile060"];
                                tile.Position = StartPostion + new Vector2((x * tile.SpriteRend.Sprite.Width * tile.Scale.X), (y * tile.SpriteRend.Sprite.Height * tile.Scale.Y));
                                renderer.ColorOverlay = Color.White;
                                renderer.DrawLayer = 0;

                                FloorRoomTiles[x, y] = tile;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
