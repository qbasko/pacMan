using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
   public class Utils
    {
        public static Point WorldToMap(Vector2 position)
        {
            Point mapPosition = new Point();
            mapPosition.X = (int)position.X / (int)Game1.TILE_SIZE;
            mapPosition.Y = (int)position.Y / (int)Game1.TILE_SIZE;
            return mapPosition;
        }
    }
}
