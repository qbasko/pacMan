using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    class Obj
    {
        public Texture2D Texture = null;
        public string TextureName = String.Empty;
        public Vector2 Center = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0.0f;
        public float Scale = 1.0f;
        public float Speed = 0.0f;
        public bool isAlive = true;

        public Obj(Vector2 pos)
        {
            Position = pos;
        }

        public Obj() { }

        public virtual void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Sprites/" + this.TextureName);
            Center = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isAlive)
                return;
            spriteBatch.Draw(Texture, Position, null, Color.White,
                MathHelper.ToRadians(Rotation), Center, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
