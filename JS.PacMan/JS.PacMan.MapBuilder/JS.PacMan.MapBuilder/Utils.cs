using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan.MapBuilder
{
    class Utils
    {
        public static bool CheckKeyboard(KeyboardState current, KeyboardState previous, Keys key)
        {
            if (current.IsKeyDown(key) && previous.IsKeyUp(key))
                return true;
            return false;
        }

        public static Point WorldToMap(Vector2 position)
        {
            Point mapPosition = new Point();
            mapPosition.X = (int)position.X / (int)Game1.TILE_SIZE;
            mapPosition.Y = (int)position.Y / (int)Game1.TILE_SIZE;
            return mapPosition;
        }
    }
}
