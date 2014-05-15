using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    class MySprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Velocity;
        private Vector2 screenSize;

        public MySprite(Texture2D texture, Vector2 position, Vector2 size,
             int screenWidth, int screenHeight)
        {
            Texture = texture;
            Position = position;
            Size = size;
            screenSize = new Vector2(screenWidth, screenHeight);
        }

        public bool Collides(MySprite otherSprite)
        {
            if (this.Position.X + this.Size.X > otherSprite.Position.X &&
                this.Position.X < otherSprite.Position.X + otherSprite.Position.X &&
                this.Position.Y + this.Size.Y > otherSprite.Position.Y &&
                this.Position.Y < otherSprite.Position.Y + otherSprite.Size.Y)
                return true;
            return false;
        }
    }
}
