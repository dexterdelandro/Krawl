using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameplayArchitecture;

namespace Krawl.GameArchitecture
{
    class LivingEntity : GameObject
    {
        public float HP = 30; // Add heal properties
        public float MaxHP = 30;
        public float Damage = 20;
        public float Gold = 0;
        public float MovementSpeed = 5;
        public float AttackSpeed = 1;
        public bool invincible = false; //Used for enemies and players not being melted the moment they are touched.

        protected FacingDirection InvertDirection(FacingDirection d)
        {
            FacingDirection returned = d;
            switch (returned)
            {
                case FacingDirection.Up:
                    returned = FacingDirection.Down;
                    break;
                case FacingDirection.Down:
                    returned = FacingDirection.Up;
                    break;
                case FacingDirection.Left:
                    returned = FacingDirection.Right;
                    break;
                case FacingDirection.Right:
                    returned = FacingDirection.Left;
                    break;
            }
            return returned;
        }

        public void DealDamage(float damage)
        {
            if (this is Player)
            {
                float totalArmor = Damage;
                Player p = (Player)this;
                if (p.BootsWearing != null)
                    totalArmor += p.BootsWearing.BonusArmor;
                if (p.ArmorWearing != null)
                    totalArmor += p.ArmorWearing.BonusArmor + p.ArmorWearing.ArmorAmt;
                if (p.WeaponHolding != null)
                    totalArmor += p.WeaponHolding.BonusArmor;

                // Works 50% of the time
                if (RandomR.Rand.Next(4) == 0)
                    totalArmor = 0;

                HP -= Math.Max((damage - totalArmor), 0);
            }
            else
                HP -= damage;
            if (HP <= 0)
                Destroy();
        }
    }
}
