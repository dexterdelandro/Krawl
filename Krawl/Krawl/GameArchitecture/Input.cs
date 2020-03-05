using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Krawl.GameArchitecture
{
    public static class Input
    {
        private static KeyboardState keyState;
        private static KeyboardState previousKeyState;

        public static void UpdateToNextKeyState(KeyboardState state)
        {
            previousKeyState = keyState;
            keyState = state;
        }

        public static bool GetKeyDown(Keys key)
        {
            return keyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        public static bool GetKeyUp(Keys key)
        {
            return !keyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key);
        }

        public static bool GetKey(Keys key)
        {
            return keyState.IsKeyDown(key);
        }
    }
}
