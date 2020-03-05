using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/* Currently not in use. Intended for an arc for attacking.
namespace Krawl.GameArchitecture
{
    
    class Circle
    {
        private Vector2 center;
        private double radius;

        public Circle(Vector2 c, double r)
        {
            center = c;
            radius = r;
        }

        public bool checkArc(int degrees) 
        {
            Vector2 edge;
            switch(degrees)
            {
                case 0:
                    for(int a = 0; a < 90; a++)
                    {
                        edge.X = center.X + (float)(radius * Math.Cos(a));
                        edge.Y = center.Y + (float)(radius * Math.Sin(a));
                    }
                    break;
                case 90:
                    for (int a = 90; a < 180; a++)
                    {
                        edge.X = center.X + (float)(radius * Math.Cos(a));
                        edge.Y = center.Y + (float)(radius * Math.Sin(a));
                    }
                    break;
                case 180:
                    for (int a = 180; a < 270; a++)
                    {
                        edge.X = center.X + (float)(radius * Math.Cos(a));
                        edge.Y = center.Y + (float)(radius * Math.Sin(a));
                    }
                    break;
                case 270:
                    for (int a = 270; a < 360; a++)
                    {
                        edge.X = center.X + (float)(radius * Math.Cos(a));
                        edge.Y = center.Y + (float)(radius * Math.Sin(a));
                    }
                    break;
            }
        }
    }
}
*/