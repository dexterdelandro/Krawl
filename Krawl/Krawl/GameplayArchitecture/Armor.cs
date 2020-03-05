using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
    class Armor : SpecialItemEffect
    {
        public float ArmorAmt;
        public string SpriteName;

        public static Armor CreateRandomArmor()
        {
            Armor armor = new Armor();
            armor.ArmorAmt = RandomR.Rand.Next(GameManager.Round) + 3;
            armor.GoldCost = (int)((RandomR.Rand.Next((int)armor.ArmorAmt * 3) * 5) + GameManager.Round * armor.ArmorAmt);
            armor.SpriteName = "00" + (RandomR.Rand.Next(4) + 5);
            return armor;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Armor");
            sb.AppendLine(ArmorAmt + " armor");
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
