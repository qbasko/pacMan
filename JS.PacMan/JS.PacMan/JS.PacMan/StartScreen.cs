using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JS.PacMan
{
    class StartScreen: DrawableGameComponent
    {
        string textToDraw;
        string secondaryTextToDraw;
        SpriteFont spriteFont;
        SpriteFont secondarySpriteFont;
        SpriteBatch spriteBatch;
        GameStateEnum currentGameState;

        public StartScreen(Game g) : base(g) 
        { 
            LoadContent(); 
        }

        protected override void LoadContent()
        {
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\MenuFont1");
            secondarySpriteFont = Game.Content.Load<SpriteFont>(@"Fonts\MenuFont2");

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Did the player hit Enter?
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // If we're not in end game, move to play state
                if (currentGameState == GameStateEnum.Intro)
                    Game1.GameState = GameStateEnum.Game;         
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Get size of string
            Vector2 TitleSize = spriteFont.MeasureString(textToDraw);

            // Draw main text
            spriteBatch.DrawString(spriteFont, textToDraw, new Vector2(Game.Window.ClientBounds.Width / 2
                    - TitleSize.X / 2,
                    Game.Window.ClientBounds.Height / 2),
                    Color.Gold);

            // Draw subtext
            spriteBatch.DrawString(secondarySpriteFont,
                secondaryTextToDraw,
                new Vector2(Game.Window.ClientBounds.Width / 2
                    - secondarySpriteFont.MeasureString(
                        secondaryTextToDraw).X / 2,
                    Game.Window.ClientBounds.Height / 2 +
                    TitleSize.Y + 10),
                    Color.Gold);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetData(string main, GameStateEnum currGameState)
        {
            textToDraw = main;
            this.currentGameState = currGameState;

            switch (currentGameState)
            {
                case GameStateEnum.Intro:
                    secondaryTextToDraw = "Press ENTER to begin";
                    break;
                case GameStateEnum.End:
                    secondaryTextToDraw = "Press ENTER to quit";
                    break;
            }
        }
    }
}
