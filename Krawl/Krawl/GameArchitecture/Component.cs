using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Krawl.GameArchitecture
{
    public abstract class Component
    {
        public GameObject ConnectedGameObject;

        public abstract void Start();
        public abstract void Update();

    }
}
