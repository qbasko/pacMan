using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan.MapBuilder
{
    class Cursor
    {
   
        private Vector2 origin = Vector2.Zero;
        private Texture2D texture;
        private MouseState currentMouseState;
        private float scale;

        public Color color;
        public Vector2 position = Vector2.Zero;

        public Cursor(Vector2 position, Color color, float scale)
        {
            this.position = position;
            this.color = color;
            this.scale = scale;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites\\cursor");
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();
            position = new Vector2(currentMouseState.X, currentMouseState.Y);
            SnapToGrid();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.HudFont, "World X: " + (position.X - Game1.TILE_SIZE).ToString() +
                "Y: " + position.Y, new Vector2(160, 550), Color.White);
        }

        public virtual void SnapToGrid()
        {
            position -= new Vector2(position.X % Game1.TILE_SIZE, position.Y % Game1.TILE_SIZE);
        }
    }
}
