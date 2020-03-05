using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Krawl.GameArchitecture
{
    public static class Time
    {
        public static GameTime CurrentGameTime;

        public static float DeltaTime
        {
            get { return (float)CurrentGameTime.ElapsedGameTime.TotalSeconds; }
        }
    }
}
