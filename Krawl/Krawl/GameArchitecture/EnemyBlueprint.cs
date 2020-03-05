using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krawl.GameArchitecture
{
    public class EnemyBlueprint
    {
        public string Name;
        public float Damage;
        public float AttackSpeed;
        public float MoveSpeed;
        public float Health;
        public List<Animation> Animations = new List<Animation>();

        public EnemyBlueprint(string name)
        {
            Name = name;
        }

        public EnemyBlueprint(string name, float dmg, float attspd, float mvspd, float hp)
        {
            Name = name;
            Damage = dmg;
            AttackSpeed = attspd;
            MoveSpeed = mvspd;
            Health = hp;
        }

    }
}
