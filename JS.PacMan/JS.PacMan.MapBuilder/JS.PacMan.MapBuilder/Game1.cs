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
using System.IO;

namespace JS.PacMan.MapBuilder
{

    public enum GameState
    {
        MapEditor,
        Collision,
        DotsEditor
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int TILE_SIZE = 16;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState currentKeyBoardState;
        private KeyboardState previousKeyboardState;
        private Grid grid;
        private Cursor cursor;
        private TileStrip tileStrip;
        private Point mapPosition = new Point();
        private List<Tile> tileList;
        private List<Tile> alreadyCreatedTileList;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private Texture2D screenShotTexture;
       

        public static SpriteFont HudFont;
        public static Texture2D TileStripTexture;
        public GameState GameState = GameState.MapEditor;
        public Dictionary<Vector2, int> mapImagesDict = new Dictionary<Vector2, int>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 576;
            graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            grid = new Grid(Game1.TILE_SIZE, 28, 31, new Vector2(160, 48));
            cursor = new Cursor(Vector2.Zero, Color.Red, 1.5f);
            tileStrip = new TileStrip();
            tileList = new List<Tile>();
            alreadyCreatedTileList = new List<Tile>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            HudFont = Content.Load<SpriteFont>("Fonts\\HUDFont");
            cursor.LoadContent(Content);
            tileStrip.LoadContent(Content);
            alreadyCreatedTileList = new MapBuilder().GetTilesListFromFile();
        }


        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            currentKeyBoardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if (Utils.CheckKeyboard(currentKeyBoardState, previousKeyboardState, Keys.Escape))
                this.Exit();

            if(Utils.CheckKeyboard(currentKeyBoardState, previousKeyboardState, Keys.F1))
            {
                GameState = GameState.MapEditor;
                TileStripTexture = Content.Load<Texture2D>("Sprites\\tileStrip4");
                tileStrip.TileCount = (TileStripTexture.Width / Game1.TILE_SIZE);
                cursor.color = Color.Red;
            }

            if (Utils.CheckKeyboard(currentKeyBoardState, previousKeyboardState, Keys.F2))
            {
                GameState = GameState.Collision;
                TileStripTexture = Content.Load<Texture2D>("Sprites\\collisionstrip");
                tileStrip.TileCount = (TileStripTexture.Width / Game1.TILE_SIZE);
                cursor.color = Color.Yellow;
            }

            if (GameState == GameState.MapEditor)
            {
                if (Utils.CheckKeyboard(currentKeyBoardState, previousKeyboardState, Keys.S))
                {
                    
                    Draw(new GameTime());
                    int[] backBuffer = new int[graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight];
                    GraphicsDevice.PrepareScreenShot();
                    GraphicsDevice.SaveScreenshot("scrreenshot.png");
                    // GraphicsDevice.GetBackBufferData<int>(backBuffer);                    

                    try
                    {
                        using (screenShotTexture = new Texture2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false, SurfaceFormat.Color))
                        {
                            screenShotTexture.SetData(backBuffer);
                            Stream stream = File.OpenWrite("screenshot.png");
                            screenShotTexture.SaveAsPng(stream, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                            stream.Dispose();
                            StreamWriter mapWriter = new StreamWriter("map.txt");
                            foreach (var item in mapImagesDict)
                            {
                                string line = String.Format("{0},{1}:{2}", item.Key.X, item.Key.Y, item.Value);
                                mapWriter.WriteLine(line);
                            }
                            mapWriter.Close();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (cursor.position.X > 160 && cursor.position.X < 609 &&
                cursor.position.Y > 47 && cursor.position.Y < 529)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed &&
                    previousMouseState.LeftButton == ButtonState.Released)
                {
                    AddTile();
                }
                if (currentMouseState.RightButton == ButtonState.Pressed &&
                    previousMouseState.RightButton == ButtonState.Released)
                {
                    DeleteTile(new Vector2(cursor.position.X - Game1.TILE_SIZE, cursor.position.Y));
                }
            }

            grid.Update(gameTime);
            cursor.Update(gameTime);
            tileStrip.Update(gameTime);

            previousKeyboardState = currentKeyBoardState;
            previousMouseState = currentMouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if(GameState==GameState.Collision || GameState== GameState.DotsEditor)
            {
                foreach (Tile t in alreadyCreatedTileList)
                {
                    t.Draw(spriteBatch);
                }
            }

            grid.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            tileStrip.Draw(spriteBatch);
            foreach (Tile t in tileList)
            {
                t.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            string status = (GameState == GameState.MapEditor ? "Map editor" : (GameState == GameState.Collision) ? "Collision map" : "Dots editor");
            spriteBatch.DrawString(HudFont, status, new Vector2(160, 0), Color.White);
            mapPosition = Utils.WorldToMap(new Vector2(cursor.position.X - 176, cursor.position.Y));
            spriteBatch.DrawString(HudFont, "Map X: " + mapPosition.X.ToString() + " Y: " + mapPosition.Y.ToString(),
                new Vector2(410, 550), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool AlreadyExist(Tile tile)
        {
            foreach (Tile t in tileList)
            {
                if (t.Position == tile.Position)
                    return true;
            }
            return false;
        }

        private void AddTile()
        {
            Vector2 tilePos = new Vector2(cursor.position.X - Game1.TILE_SIZE, cursor.position.Y);
            Tile t = new Tile(tilePos, tileStrip.selected);
            if (!mapImagesDict.ContainsKey(tilePos))
                mapImagesDict.Add(tilePos, t.Selected);
            if (!AlreadyExist(t))
                tileList.Add(t);
            t = null;
        }

        private void DeleteTile(Vector2 position)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                if (tileList[i].Position == position)
                {
                    mapImagesDict.Remove(position);
                    tileList.RemoveAt(i);
                }
            }
        }
    }
}
