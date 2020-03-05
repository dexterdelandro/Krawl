using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
    public enum Weapons
    {
        WeakSword,
        AverageSword,
        StrongSword,
        WeakBow,
        AverageBow,
        StrongBow
    }

    class Weapon : SpecialItemEffect
    {
        public string Name;
        public float LifeTime;
        public float Damage;
        public float ProjectSpeed;
        public float AttackSpeed;
        public Vector2 ProjectileSize;
        public string SpriteName;

        private float timeTillNextAttack;

        public void Update()
        {
            if (timeTillNextAttack > 0)
                timeTillNextAttack -= Time.DeltaTime * AttackSpeed;

        }

        public void Attack(LivingEntity wielder, Vector2 directionOfAttack)
        {
            if (timeTillNextAttack > 0)
                return;
            timeTillNextAttack = 1;

            bool isGood = wielder is Player;
            Vector2 scale;
            if (directionOfAttack.X == 0)
                scale = ProjectileSize;
            else
                scale = new Vector2(ProjectileSize.Y, ProjectileSize.X);
            float totalDamage = Damage;
            if (wielder is Player)
            {
                Player p = (Player)wielder;
                if (p.BootsWearing != null)
                    totalDamage += p.BootsWearing.BonusDamage;
                if (p.ArmorWearing != null)
                    totalDamage += p.ArmorWearing.BonusDamage;
                if (p.WeaponHolding != null)
                    totalDamage += p.WeaponHolding.BonusDamage;
            }

            Projectile.CreateProjectile(wielder.PositionCenter + (directionOfAttack * 5), directionOfAttack * ProjectSpeed, scale / 5f, totalDamage, LifeTime, isGood);
        }

        public static Weapon WeaponPrefabs(Weapons prefab)
        {
            Weapon weapon = new Weapon();
            weapon.Name = Enum.GetName(typeof(Weapons), prefab);

            switch (prefab)
            {
                case Weapons.WeakSword:
                    weapon.LifeTime = .1f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 3;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(9, 3);
                    weapon.SpriteName = "003";
                    weapon.GoldCost = 50;
                    break;
                case Weapons.AverageSword:
                    weapon.LifeTime = .1f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 6;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(9, 3);
                    weapon.SpriteName = "001";
                    weapon.GoldCost = 200;
                    break;
                case Weapons.StrongSword:
                    weapon.LifeTime = .1f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 12;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(9, 3);
                    weapon.SpriteName = "002";
                    weapon.GoldCost = 400;
                    break;
                case Weapons.WeakBow:
                    weapon.LifeTime = 3f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 2;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(2, 6);
                    weapon.SpriteName = "017";
                    weapon.GoldCost = 100;
                    break;
                case Weapons.AverageBow:
                    weapon.LifeTime = 3f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 4;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(2, 6);
                    weapon.SpriteName = "018";
                    weapon.GoldCost = 230;
                    break;
                case Weapons.StrongBow:
                    weapon.LifeTime = 3f;
                    weapon.ProjectSpeed = 6;
                    weapon.Damage = 8;
                    weapon.AttackSpeed = 3;
                    weapon.ProjectileSize = new Vector2(2, 6);
                    weapon.SpriteName = "019";
                    weapon.GoldCost = 500;
                    break;
            }
            weapon.Damage += weapon.Damage + GameManager.Round * 2.5f;
            weapon.GoldCost += (RandomR.Rand.Next(GameManager.Round) + 5) * 20;
            return weapon;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            sb.AppendLine(Damage + " damage");
            sb.AppendLine(AttackSpeed + " attack speed");
            sb.AppendLine(LifeTime + " range");
            if (BonusArmor > 0)
                sb.AppendLine(BonusArmor + " bonus armor");
            if (BonusDamage > 0)
                sb.AppendLine(BonusDamage + " bonus damage");
            if (BonusMoveSpeed > 0)
                sb.AppendLine(BonusMoveSpeed + " bonus move speed");
            return sb.ToString();
        }
    }
}
