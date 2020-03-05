using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Krawl.GameplayArchitecture
{
	static class Shop
	{

        public static List<Item> ItemsInShop = new List<Item>();

        public static void LoadUpShop()
        {
            while (0 < ItemsInShop.Count)
            {
                ItemsInShop[0].Destroy();
                ItemsInShop.RemoveAt(0);
            }
            for (int i = 0; i < 7; i++)
            {
                Item item = Item.CreateRandomItem();
                item.Position = new Vector2(288 + (i * 100), 70);
                ItemsInShop.Add(item);
            }
        }

	}
}
