using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
	public enum ItemType { Weapon, Armor, Boots, Enemies };
	class ShopItem : GameObject
	{
		private ItemType itemType;
		private int attackIncrease;
		private int armorIncrease;
		private int movementSpeedIncrease;
		private int numEnemies;
		private const double SUB_STAT_CHANCE = 0.2;
		private Random r = new Random();

		public ItemType Type { get { return itemType; } }
		public int AttackIncrease { get { return attackIncrease; } }
		public int ArmorIncrease { get { return armorIncrease; } }
		public int SpeedIncrease { get { return movementSpeedIncrease; } }
		public int NumEnemies { get { return numEnemies; } }


		public ShopItem(int round)
		{
			int randoType = r.Next(1, 4);

			//need to scale with round
			int mainStat = r.Next(round, round * 2);

			int subStat1 = 0;
			int subStat2 = 0;
			//need to scale with round
			if (r.NextDouble() <= SUB_STAT_CHANCE) subStat1 = r.Next(round / 2, round);
			if (r.NextDouble() <= SUB_STAT_CHANCE) subStat2 = r.Next(round / 2, round);

			switch (randoType)
			{
				case 1:
					itemType = ItemType.Weapon;
					attackIncrease = mainStat;
					armorIncrease = subStat1;
					movementSpeedIncrease = subStat2;
					break;
				case 2:
					itemType = ItemType.Boots;
					movementSpeedIncrease = mainStat;
					attackIncrease = subStat1;
					armorIncrease = subStat2;
					break;
				case 3:
					itemType = ItemType.Armor;
					armorIncrease = mainStat;
					attackIncrease = subStat1;
					movementSpeedIncrease = subStat2;
					break;

				case 4:
					numEnemies = r.Next(round + 1);
					break;
			}
		}
		public ShopItem(ItemType type, int attack, int armor, int speed, int enemies)
		{
			itemType = type;
			attackIncrease = attack;
			armorIncrease = armor;
			movementSpeedIncrease = speed;
			numEnemies = 0;
			if (itemType == ItemType.Enemies)
			{
				numEnemies = enemies;
				attackIncrease = 0;
				armorIncrease = 0;
				movementSpeedIncrease = 0;
			}
		}

		public string getMessage()
		{
			string message = "";
			switch (itemType)
			{
				case ItemType.Weapon:
					message += "This weapon ";
					break;

				case ItemType.Armor:
					message += "This piece of armor ";
					break;

				case ItemType.Boots:
					message += "These pair of boots ";
					break;
			}
			message += "gives your player:\n";
			if (attackIncrease != 0) message += "+" + attackIncrease + " Attack\n";
			if (armorIncrease != 0) message += "+" + armorIncrease + " Armor\n";
			if (movementSpeedIncrease != 0) message += "+" + movementSpeedIncrease + " Speed\n";

			if (itemType == ItemType.Enemies) message = "Give your opponent " + numEnemies + " enemies.";
			return message;
		}

		/// <summary>
		/// Uses the item (uses the stats/ adds more enemies to opponent)
		/// </summary>
		/// <param name="player">player that bought the item</param>
		public void Use(Player player) {
			if (itemType == ItemType.Enemies)
			{
				if (player.PlayerNum == 0)
				{
					GameManager.numEnemiesStartP1 += NumEnemies;
				}
				else
				{
					GameManager.numEnemiesStartP1 += NumEnemies;
				}
			}
			else {
				player.Damage += attackIncrease;
				player.MovementSpeed += movementSpeedIncrease;
			}
		}

		public void RemoveItem(Player player) {
			if (itemType != ItemType.Enemies) {
				player.Damage -= attackIncrease;
				player.MovementSpeed -= movementSpeedIncrease;
			}
		}
	}
}
