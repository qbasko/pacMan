using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    class Animation
    {
        private float scale;
        private int elapsedTime;
        private int frameTime;
        private int currentFrame;
        private Color color;
        private Rectangle sourceRect = new Rectangle();

        public Rectangle DestinationRect = new Rectangle();
        public Texture2D SpriteStrip;
        public int FrameCount;
        public int FrameWidth;
        public int FrameHeigh;
        public bool IsActive;
        public bool IsLooping;
        public Vector2 Position;
        public float Rotation;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth,
            int frameHeigh, int frameCount, int frameTime, Color color, float scale, bool looping, int startFrame)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeigh = frameHeigh;
            this.FrameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;
            IsLooping = looping;
            Position = position;
            SpriteStrip = texture;
            elapsedTime = 0;
            currentFrame = startFrame;
            IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if(currentFrame==FrameCount)
                {
                    currentFrame = 0;
                    if (!IsLooping)
                        IsActive = false;
                }
                elapsedTime = 0;
            }   
                                         
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeigh);
            DestinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeigh * scale) / 2,
            (int)(FrameWidth * scale), (int)(FrameHeigh * scale));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 origin, bool walking)
        {           
            if(IsActive)
            {
                if(!walking)                
                    sourceRect = new Rectangle(0, 0, FrameWidth, FrameHeigh);                 
                
                spriteBatch.Draw(SpriteStrip, DestinationRect, sourceRect, 
                    color, MathHelper.ToRadians(Rotation), origin, SpriteEffects.None, 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
                spriteBatch.Draw(SpriteStrip, DestinationRect, sourceRect, color);
        }
    }
}
