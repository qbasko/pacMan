using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JS.PacMan.Map;
using IndependentResolutionRendering;

namespace JS.PacMan
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StartScreen startScreen;
        EndScreen endScreen;
        MySprite mySprite;
        List<Tile> tilesList;
        public static int SuperDotTime;
        public static bool reloadContent;
        public static Texture2D StripTexture;
        public static SpriteFont HudFont;
        bool open;

        public static Game1 PacmanGame;
        public static GameStateEnum GameState;


        public static MapPath MapPath;
        //public static Map.Map Map;
        public const int TILE_SIZE = 16;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PacmanGame = this;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(800, 800);
            reloadContent = false;
        }

        protected override void Initialize()
        {
            Items.Initialize();
            GameState = GameStateEnum.Intro;
            base.Initialize();
            MapBuilder mapBuilder = new MapBuilder();
            tilesList = mapBuilder.GetTilesListFromFile();
            foreach (var item in tilesList)
            {
                if (item.Selected == 1)
                    item.TileTexture = Content.Load<Texture2D>("map\\whiteBox2");
                else if (item.Selected == 0)
                    item.TileTexture = Content.Load<Texture2D>("map\\dot");
                else if (item.Selected == 9)
                    item.TileTexture = Content.Load<Texture2D>("map\\superDot");
            }

            startScreen = new StartScreen(this);
            startScreen.SetData("Welcome to PACMAN!", GameState);

            endScreen = new EndScreen(this);
            endScreen.SetData("Score: " + Items.Pacman.eatenDotsPoints, GameState);

            MapPath = new MapPath(tilesList, mapBuilder.NumberOfRows, mapBuilder.NumberOfColumns);
        }

        protected override void LoadContent()
        {
            //mySprite = new MySprite(Content.Load<Texture2D>("pacman_open"), Vector2.Zero,
            //   new Vector2(64f, 64f), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //mySprite.Velocity = new Vector2(3, 1);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (Obj o in Items.ObjList)
            {
                o.LoadContent(Content);
            }

            StripTexture = Content.Load<Texture2D>("map\\tilestripWhite");
            HudFont = Content.Load<SpriteFont>("Fonts\\Font1");
        }

        protected override void UnloadContent()
        {
            //mySprite.Texture.Dispose();
            //spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GameState == GameStateEnum.Game)
            {
                foreach (Obj o in Items.ObjList)
                {
                    o.Update(gameTime);
                }
            }

            if (reloadContent)
            {
                foreach (var item in Items.ObjList)
                {
                    item.LoadContent(Content);
                }
                reloadContent = false;
            }

            if (GameState == GameStateEnum.Intro)
                startScreen.Update(gameTime);

            else if (GameState == GameStateEnum.End)
                endScreen.Update(gameTime);


            //if (SuperDotTime > 0)
            //    if (Items.ObjList.FirstOrDefault(o => o is Ghost && ((Ghost)o).isEatable) != null)
            //    {
            //        foreach (var item in Items.ObjList)
            //        {
            //            item.LoadContent(Content);
            //        }
            //        SuperDotTime--;
            //    }
            //    else if

            base.Update(gameTime);
        }

        private void Move()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Up) || gamepadState.IsButtonDown(Buttons.DPadUp))
                if (mySprite.Position.Y - 1 > 0)
                    mySprite.Position.Y -= 1;
                else
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
            if (keyboardState.IsKeyDown(Keys.Down) || gamepadState.IsButtonDown(Buttons.DPadDown))
                if (mySprite.Position.Y + 1 + mySprite.Size.Y <= graphics.PreferredBackBufferHeight)
                    mySprite.Position.Y += 1;
                else
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
            if (keyboardState.IsKeyDown(Keys.Left) || gamepadState.IsButtonDown(Buttons.DPadLeft))
                if (mySprite.Position.X - 1 > 0)
                    mySprite.Position.X -= 1;
                else
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
            if (keyboardState.IsKeyDown(Keys.Right) || gamepadState.IsButtonDown(Buttons.DPadRight))
                if (mySprite.Position.X + 1 + mySprite.Size.X <= graphics.PreferredBackBufferWidth)
                    mySprite.Position.X += 1;
                else
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            Resolution.BeginDraw();

            if (GameState == GameStateEnum.Game)
            {
                GraphicsDevice.Clear(Color.Black);

                //spriteBatch.Begin();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

                foreach (Obj o in Items.ObjList)
                {
                    o.Draw(spriteBatch);
                }
                spriteBatch.End();

                //spriteBatch.Begin();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
                foreach (Tile t in tilesList)
                {
                    if (MapPath.pathTable[t.ColumnNumber, t.RowNumber] != MapTileType.MapEmpty)
                        t.Draw(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
                spriteBatch.DrawString(HudFont, "SCORE: " + Items.Pacman.eatenDotsPoints.ToString(), new Vector2(160, 750), Color.White);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
                spriteBatch.DrawString(HudFont, "LIVES: ", new Vector2(400, 750), Color.White);
                spriteBatch.End();

                int livesBarPosition = 550;

                for (int i = 0; i < Items.Pacman.numberOfLifes; i++)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
                    spriteBatch.Draw(Content.Load<Texture2D>("Sprites\\pacman1"), new Rectangle(livesBarPosition, 750, 30, 30), Color.White);
                    spriteBatch.End();
                    livesBarPosition += 40;
                }

            }

            else if (GameState == GameStateEnum.Intro)            
                startScreen.Draw(gameTime);

            else if (GameState == GameStateEnum.End)
            {
                endScreen.SetData("Score: " + Items.Pacman.eatenDotsPoints, GameState);
                endScreen.Draw(gameTime);
            }
           
            base.Draw(gameTime);
        }
    }
}
