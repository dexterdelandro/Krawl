using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Krawl.GameplayArchitecture
{
    public enum ItemTypes { Enemy, Weapon, Armor, Boots}

    class Item : GameObject
    {
        public ItemTypes ItemType;
        public string Name;
        public Weapon TheWeapon;
        public Armor TheArmor;
        public Boots TheBoots;
        public int MoreEnemies;
        public int GoldCost;

        public void Spawn()
        {
            SpriteRend.DrawLayer = 3;
            Scale = new Vector2(5);

            BoxCollider boxCollider = AddComponent<BoxCollider>();
            boxCollider.Blocks = false;
        }

        public void PickupItem(Player player)
        {
            if (player.Gold >= GoldCost)
            {
                player.Gold -= GoldCost;
                switch (ItemType)
                {
                    case ItemTypes.Weapon:
                        player.WeaponHolding = TheWeapon;
                        break;
                    case ItemTypes.Armor:
                        player.ArmorWearing = TheArmor;
                        break;
                    case ItemTypes.Boots:
                        player.BootsWearing = TheBoots;
                        break;
                    case ItemTypes.Enemy:
                        if (player.PlayerNum == Players.Player1)
                            GameManager.numEnemiesStartP2 += MoreEnemies;
                        else
                            GameManager.numEnemiesStartP1 += MoreEnemies;
                        break;
                }
				Destroy();

			}
		}

        public static Item CreateRandomItem()
        {
            Item item = new Item();
            SpriteRenderer spriteRend = item.AddComponent<SpriteRenderer>();
            switch (RandomR.Rand.Next(4))
            {
                // Weapon
                case 0:
                    item.TheWeapon = Weapon.WeaponPrefabs((Weapons)RandomR.Rand.Next(sizeof(Weapons)));
                    item.Name = item.TheWeapon.Name;
                    spriteRend.Sprite = Game1.LoadedImages[item.TheWeapon.SpriteName];
                    item.ItemType = ItemTypes.Weapon;
                    item.GoldCost = item.TheWeapon.GoldCost;
                    item.Spawn();
                    return item;
                // Armor
                case 1:
                    item.TheArmor = Armor.CreateRandomArmor();
                    spriteRend.Sprite = Game1.LoadedImages[item.TheArmor.SpriteName];
                    item.ItemType = ItemTypes.Armor;
                    item.GoldCost = item.TheArmor.GoldCost;
                    item.Spawn();
                    return item;
                // Boots
                case 2:
                    item.TheBoots = Boots.CreateRandomBoots();
                    spriteRend.Sprite = Game1.LoadedImages[item.TheBoots.SpriteName];
                    item.ItemType = ItemTypes.Boots;
                    item.GoldCost = item.TheBoots.GoldCost;
                    item.Spawn();
                    return item;
                // Enemies
                case 3:
                    item.MoreEnemies = RandomR.Rand.Next(6) + 1;
                    spriteRend.Sprite = Game1.LoadedImages["01" + (item.MoreEnemies < 3 ? "3" : item.MoreEnemies < 5 ? "4" : "5")];
                    item.ItemType = ItemTypes.Enemy;
                    item.GoldCost = (int)((RandomR.Rand.Next((int)item.MoreEnemies * 4) * 5) + GameManager.Round * item.MoreEnemies * 2) * 3;
                    item.Spawn();
                    return item;
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Enum.GetName(typeof(ItemTypes), ItemType));
            sb.AppendLine(GoldCost + " gold");
            switch (ItemType)
            {
                case ItemTypes.Armor:
                    sb.AppendLine(TheArmor.ArmorAmt + " armor");
                    if (TheArmor.BonusArmor > 0)
                    sb.AppendLine(TheArmor.BonusArmor + " bonus armor");
                    if (TheArmor.BonusDamage > 0)
                    sb.AppendLine(TheArmor.BonusDamage + " bonus damage");
                    if (TheArmor.BonusMoveSpeed > 0)
                    sb.AppendLine(TheArmor.BonusMoveSpeed + " bonus move speed");
                    break;
                case ItemTypes.Boots:;
                    sb.AppendLine(TheBoots.MoveSpeed + " move speed");
                    if (TheBoots.BonusArmor > 0)
                        sb.AppendLine(TheBoots.BonusArmor + " bonus armor");
                    if (TheBoots.BonusDamage > 0)
                        sb.AppendLine(TheBoots.BonusDamage + " bonus damage");
                    if (TheBoots.BonusMoveSpeed > 0)
                        sb.AppendLine(TheBoots.BonusMoveSpeed + " bonus move speed");
                    break;
                case ItemTypes.Enemy:
                    sb.AppendLine(MoreEnemies + " enemies");
                    break;
                case ItemTypes.Weapon:
                    sb.AppendLine(TheWeapon.Name);
                    sb.AppendLine(TheWeapon.Damage + " damage");
                    sb.AppendLine(TheWeapon.AttackSpeed + " attack speed");
                    sb.AppendLine(TheWeapon.LifeTime + " range");
                    if (TheWeapon.BonusArmor > 0)
                        sb.AppendLine(TheWeapon.BonusArmor + " bonus armor");
                    if (TheWeapon.BonusDamage > 0)
                        sb.AppendLine(TheWeapon.BonusDamage + " bonus damage");
                    if (TheWeapon.BonusMoveSpeed > 0)
                        sb.AppendLine(TheWeapon.BonusMoveSpeed + " bonus move speed");
                    break;
            }
            sb.AppendLine("");
            return sb.ToString();
        }
    }
}
