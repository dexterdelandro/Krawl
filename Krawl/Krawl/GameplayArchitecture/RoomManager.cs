using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
    public enum DoorState { Open, Closed }

    public static class RoomManager
    {
        public static Room P1Room;
        public static Room P2Room;
        public static Room ShopRoom;

        public static void LoadRoomFiles()
        {
            string[] files = Directory.GetFiles("content\\RoomTileSet");
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Remove(0, "Content\\".Length).Replace(".xnb", "");
                Texture2D texture = Game1.use.Content.Load<Texture2D>(fileName);
                Game1.LoadedImages.Add(fileName.Remove(0, "RoomTileSet\\".Length), texture);
            }
        }

        public static void CreateRooms()
        {
            P1Room = new Room(RoomPrefabType.Player, new Vector2(12, (int)(Game1.use.GraphicsDevice.Viewport.Height * .2)));
            P2Room = new Room(RoomPrefabType.Player, new Vector2(Game1.use.GraphicsDevice.Viewport.Width * .5f + 12, (int)(Game1.use.GraphicsDevice.Viewport.Height * .2)));

            ShopRoom = new Room(RoomPrefabType.Shop, new Vector2((((Game1.use.GraphicsDevice.Viewport.Width / 32f) - 30f) * 32f) / 2f, 0));

            // Do shop room later
        }

        public static void ChangeDoorState(Players p, DoorState doorState)
        {
            Room playerRoom;
            if (p == Players.Player1)
                playerRoom = P1Room;
            else
                playerRoom = P2Room;

            playerRoom.FloorRoomTiles[9, 0].GetComponent<BoxCollider>().Blocks = doorState == DoorState.Closed;
            playerRoom.FloorRoomTiles[9, 0].GetComponent<SpriteRenderer>().Sprite = Game1.LoadedImages[doorState == DoorState.Closed ? "tile006" : "tile219"];
        }
    }
}
