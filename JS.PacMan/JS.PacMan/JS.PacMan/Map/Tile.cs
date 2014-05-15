using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan.Map
{
    public class Tile
    {
        private Rectangle sourceRect;
        private Rectangle destRect;
        const int tileSize = 16;
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Vector2 Position;
        public int Selected = 0;
        public Texture2D TileTexture { get; set; }

        public Tile(Vector2 position, int selected)
        {
            this.Position = position;
            this.Selected = selected;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sourceRect = new Rectangle(Selected * tileSize, 0, tileSize, tileSize);
            //destRect = new Rectangle((int)Position.X, (int)Position.Y, tileSize, tileSize);
            //spriteBatch.Draw(Game1.StripTexture, destRect, sourceRect, Color.White);
            if (TileTexture != null)
                spriteBatch.Draw(TileTexture, Position, Color.White);

        }
    }
}
