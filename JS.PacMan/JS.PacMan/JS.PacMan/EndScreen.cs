using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    class EndScreen : DrawableGameComponent
    {
        string Score;
        string Exit;
        SpriteFont spriteFont;
        SpriteFont secondarySpriteFont;
        SpriteBatch spriteBatch;
        GameStateEnum currentGameState;

        public EndScreen(Game g) : base(g) 
        { 
            LoadContent(); 
        }

        protected override void LoadContent()
        {
            secondarySpriteFont = Game.Content.Load<SpriteFont>(@"Fonts\MenuFont1");
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\MenuFont2");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Did the player hit Enter?
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // If we're not in end game, move to play state
                Game1.PacmanGame.Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {            
            spriteBatch.Begin();            

            // Get size of string
            Vector2 TitleSize = spriteFont.MeasureString(Score);

            // Draw main text
            spriteBatch.DrawString(spriteFont, Score, new Vector2(Game.Window.ClientBounds.Width / 2
                    - TitleSize.X / 2,
                    Game.Window.ClientBounds.Height / 2),
                    Color.Gold);

            // Draw subtext
            spriteBatch.DrawString(secondarySpriteFont,
                Exit,
                new Vector2(Game.Window.ClientBounds.Width / 2
                    - secondarySpriteFont.MeasureString(
                        Exit).X / 2,
                    Game.Window.ClientBounds.Height / 2 +
                    TitleSize.Y + 10),
                    Color.Gold);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetData(string main, GameStateEnum currGameState)
        {
            Score = main;
            this.currentGameState = currGameState;

            switch (currentGameState)
            {
                case GameStateEnum.Intro:
                    Exit = "Press ENTER to exit";
                    break;
            }
        }
    }
}
