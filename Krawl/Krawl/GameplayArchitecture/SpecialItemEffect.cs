using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
    class SpecialItemEffect
    {
        public float BonusMoveSpeed;
        public float BonusArmor;
        public float BonusDamage;
        public int GoldCost;

        public SpecialItemEffect()
        {
            if (RandomR.Rand.Next(10) <= 1) // 20% chance to add a special effect
                BonusArmor = RandomR.Rand.Next(GameManager.Round) + 3;
            if (RandomR.Rand.Next(10) <= 1) // 20% chance to add a special effect
                BonusMoveSpeed = RandomR.Rand.Next(GameManager.Round) + 3;
            if (RandomR.Rand.Next(10) <= 1) // 20% chance to add a special effect
                BonusDamage = RandomR.Rand.Next(GameManager.Round) + 3;
        }

    }
}
