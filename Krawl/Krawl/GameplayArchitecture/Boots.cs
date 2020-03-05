using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krawl.GameArchitecture;

namespace Krawl.GameplayArchitecture
{
    class Boots : SpecialItemEffect
    {
        public float MoveSpeed;
        public string SpriteName;

        public static Boots CreateRandomBoots()
        {
            Boots boots = new Boots();
            boots.MoveSpeed = RandomR.Rand.Next(GameManager.Round) + 3;
            boots.GoldCost = (int)((RandomR.Rand.Next((int)boots.MoveSpeed * 3) * 5) + GameManager.Round * boots.MoveSpeed);
            boots.SpriteName = "0" + (RandomR.Rand.Next(4) + 9).ToString("D2");
            return boots;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Boots");
            sb.AppendLine(MoveSpeed + " move speed");
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
