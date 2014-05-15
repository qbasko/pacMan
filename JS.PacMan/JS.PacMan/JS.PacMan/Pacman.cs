using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JS.PacMan.Map;

namespace JS.PacMan
{
    class Pacman : Obj
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private Animation pacmanAnimation;
        private SoundEffect walkingSound;
        private SoundEffectInstance walkingSoundInstance;
        private SoundEffect deathSound;
        public SoundEffectInstance deathSoundInstance;
        private SoundEffect superDotSound;
        private SoundEffectInstance superDotSoundInstance;
        private SoundEffect ghostEatedSound;
        public SoundEffectInstance ghostEatedSoundInstance;



        private bool walking;
        public const int TILE_SIZE = 16;
        public const int pacmanHeight = 28;
        public const int pacmanWidth = 37;
        public int eatenDotsPoints;
        public int numberOfLifes;
        int updateCounter;


        public Pacman(Vector2 pos, string textureName)
        {
            TextureName = textureName;
            Position = pos;
            isAlive = true;
            Speed = 2.0f;
            pacmanAnimation = new Animation();
            eatenDotsPoints = 0;
            numberOfLifes = 3;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            pacmanAnimation.Initialize(Texture, Position, pacmanWidth, pacmanHeight, 7, 25, Color.White, Scale, true, 0);
            Center = new Vector2(pacmanAnimation.FrameWidth / 2, pacmanAnimation.FrameHeigh / 2);
            walkingSound = content.Load<SoundEffect>("Sounds\\pacman_chomp");
            walkingSoundInstance = walkingSound.CreateInstance();
            walkingSoundInstance.IsLooped = false;
            walkingSoundInstance.Volume = 0.1f;

            deathSound = content.Load<SoundEffect>("Sounds\\pacman_death");
            deathSoundInstance = deathSound.CreateInstance();
            deathSoundInstance.IsLooped = false;
            deathSoundInstance.Volume = 0.1f;

            superDotSound = content.Load<SoundEffect>("Sounds\\pacman_extrapac");
            superDotSoundInstance = superDotSound.CreateInstance();
            superDotSoundInstance.IsLooped = false;
            superDotSoundInstance.Volume = 0.1f;

            ghostEatedSound = content.Load<SoundEffect>("Sounds\\pacman_eatfruit");
            ghostEatedSoundInstance = ghostEatedSound.CreateInstance();
            ghostEatedSoundInstance.IsLooped = false;
            ghostEatedSoundInstance.Volume = 0.1f;
        }

        public override void Update(GameTime gameTime)
        {
            updateCounter++;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Game1.PacmanGame.Exit();
            }

            if (currentKeyboardState.IsKeyDown(Keys.A))
                isAlive = true;

            if (!isAlive && numberOfLifes == 0)
            {
                Game1.GameState = GameStateEnum.End;
                return;
            }
            else if (updateCounter % 100 == 0) isAlive = true;

            walking = false;

            if (Game1.SuperDotTime > 1)
            {
                Game1.SuperDotTime--;
                superDotSoundInstance.Play();
            }
            else if (Game1.SuperDotTime == 1)
            {
                Game1.SuperDotTime = 0;
                Items.SetGhostAsNotEatable();
                Game1.reloadContent = true;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                TilePosition tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - 16);
                if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY - 1))
                {
                    if (Game1.MapPath.IsDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 10;
                    }
                    else if (Game1.MapPath.IsSuperDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 100;
                        Items.SetGhostsAsEatable();
                        Game1.SuperDotTime += 400;
                        //superDotSoundInstance.Play();
                    }
                    CheckCollisionWithGhosts(tilePos);
                    CheckTunnelEntrance(tilePos);

                    walking = true;
                    Rotation = (int)RotationEnum.North;
                    Position.Y -= Speed;
                    Position.X = SnapToX(Position);
                    //walkingSoundInstance.Play();
                }

                //Point mapLoc = Utils.WorldToMap(new Vector2(Position.X - 160, Position.Y - Game1.TILE_SIZE));
                //if (Game1.Map.IsOpenLocation(mapLoc.X, mapLoc.Y - 1) && Game1.Map.IsOpenLocation(mapLoc.X -1, mapLoc.Y - 1)
                //    && Game1.Map.IsOpenLocation(mapLoc.X - 2, mapLoc.Y - 1))
                //{
                //    walking = true;
                //    Rotation = (int)RotationEnum.North;
                //    Position.Y -= Speed;
                //Position.X = SnapToX(Position);
                //    //walkingSoundInstance.Play();
                //}
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                //Point mapLoc = Utils.WorldToMap(new Vector2(Position.X - 160, Position.Y));
                //if (Game1.Map.IsOpenLocation(mapLoc.X, mapLoc.Y))
                //{
                TilePosition tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - TILE_SIZE);
                if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY + 1))
                {
                    if (Game1.MapPath.IsDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 10;
                    }
                    else if (Game1.MapPath.IsSuperDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 100;
                        Items.SetGhostsAsEatable();
                        Game1.SuperDotTime += 400;
                        //superDotSoundInstance.Play();
                    }
                    CheckCollisionWithGhosts(tilePos);
                    CheckTunnelEntrance(tilePos);

                    walking = true;
                    Rotation = (int)RotationEnum.South;
                    Position.Y += Speed;
                    Position.X = SnapToX(Position);
                    //    //walkingSoundInstance.Play();
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                //Point mapLoc = Utils.WorldToMap(new Vector2(Position.X - 165 - Game1.TILE_SIZE, Position.Y));
                //if (Game1.Map.IsOpenLocation(mapLoc.X - 1, mapLoc.Y) && Game1.Map.IsOpenLocation(mapLoc.X - 1, mapLoc.Y -1) &&
                //    Game1.Map.IsOpenLocation(mapLoc.X - 1, mapLoc.Y -2)
                //    && Game1.Map.IsOpenLocation(mapLoc.X - 1, mapLoc.Y +1)&&
                //    Game1.Map.IsOpenLocation(mapLoc.X - 1, mapLoc.Y -2))

                TilePosition tilePos = MapPath.ConvertToMapLocation(Position.X - 16, Position.Y - TILE_SIZE);
                if (Game1.MapPath.IsOpenLocation(tilePos.PosX - 1, tilePos.PosY))
                {
                    if (Game1.MapPath.IsDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 10;
                    }
                    else if (Game1.MapPath.IsSuperDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 100;
                        Items.SetGhostsAsEatable();
                        Game1.SuperDotTime += 400;
                        //superDotSoundInstance.Play();
                    }
                    CheckCollisionWithGhosts(tilePos);
                    CheckTunnelEntrance(tilePos);

                    walking = true;
                    Rotation = (int)RotationEnum.West;
                    Position.X -= Speed;
                    Position.Y = SnapToY(Position);
                    //    //walkingSoundInstance.Play();
                }
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                //Point mapLoc = Utils.WorldToMap(new Vector2(Position.X - 160, Position.Y));
                //if (Game1.Map.IsOpenLocation(mapLoc.X, mapLoc.Y) && Game1.Map.IsOpenLocation(mapLoc.X, mapLoc.Y-1) &&
                //    Game1.Map.IsOpenLocation(mapLoc.X, mapLoc.Y-2))
                TilePosition tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - TILE_SIZE);
                if (Game1.MapPath.IsOpenLocation(tilePos.PosX + 1, tilePos.PosY))
                {
                    if (Game1.MapPath.IsDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 10;
                    }
                    else if (Game1.MapPath.IsSuperDotActive(tilePos.PosX, tilePos.PosY))
                    {
                        Game1.MapPath.DeactivateDot(tilePos.PosX, tilePos.PosY);
                        eatenDotsPoints += 100;
                        Items.SetGhostsAsEatable();
                        Game1.SuperDotTime += 400;
                        // superDotSoundInstance.Play();
                    }

                    CheckCollisionWithGhosts(tilePos);
                    CheckTunnelEntrance(tilePos);

                    walking = true;
                    Rotation = (int)RotationEnum.East;
                    Position.X += Speed;
                    Position.Y = SnapToY(Position);
                    //     //walkingSoundInstance.Play();
                }
            }

            pacmanAnimation.Position = Position;
            pacmanAnimation.Rotation = Rotation;
            pacmanAnimation.Update(gameTime);
            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                pacmanAnimation.Draw(spriteBatch, Center, walking);
            }
        }

        public float SnapToX(Vector2 pos)
        {
            pos -= new Vector2(pos.X % TILE_SIZE, pos.Y % TILE_SIZE);
            return pos.X + 4;
        }

        public float SnapToY(Vector2 pos)
        {
            pos -= new Vector2(pos.X % TILE_SIZE, pos.Y % TILE_SIZE);
            return pos.Y + 3;
        }

        private void CheckCollisionWithGhosts(TilePosition position)
        {
            if (isAlive)
                for (int i = 0; i < Items.ObjList.Count; i++)
                {
                    if (Items.ObjList[i] is Ghost)
                    {
                        Ghost ghost = (Ghost)Items.ObjList[i];
                        TilePosition ghostPosition = MapPath.ConvertToMapLocation(ghost.Position.X, ghost.Position.Y);

                        if (ghost.isAlive && (
                            ghostPosition.PosX == position.PosX ||
                            ghostPosition.PosX + 1 == position.PosX ||
                             ghostPosition.PosX + 2 == position.PosX))
                        {
                            if (ghostPosition.PosY == position.PosY ||
                                ghostPosition.PosY - 1 == position.PosY ||
                                ghostPosition.PosY + 1 == position.PosY)
                            {
                                if (ghost.isEatable)
                                {
                                    ghost.isAlive = false;
                                    eatenDotsPoints += 200;
                                    ghostEatedSoundInstance.Play();
                                    break;
                                }
                                else
                                {
                                    Items.Pacman.isAlive = false;
                                    Items.Pacman.numberOfLifes--;
                                    Items.Pacman.Position = new Vector2(480, 565);
                                    deathSoundInstance.Play();
                                    break;
                                }
                            }
                        }
                    }
                }
        }

        //to do
        private void CheckTunnelEntrance(TilePosition tilePos)
        {
            if (Rotation == (int)RotationEnum.West)
            {
                if (Game1.MapPath.IsLeftTunnel(tilePos.PosX, tilePos.PosY) ||
                    Game1.MapPath.IsLeftTunnel(tilePos.PosX, tilePos.PosY + 1) ||
                    Game1.MapPath.IsLeftTunnel(tilePos.PosX, tilePos.PosY - 1))
                {
                    Vector2 newPosition = new Vector2(Game1.MapPath.RightTunnelPosition.PosX * 16 + 160,
                        (Game1.MapPath.RightTunnelPosition.PosY + 1) * 16);

                    Position = newPosition;
                }
            }
            else if (Rotation == (int)RotationEnum.East)
            {
                if (Game1.MapPath.IsRightTunnel(tilePos.PosX, tilePos.PosY) ||
                    Game1.MapPath.IsRightTunnel(tilePos.PosX, tilePos.PosY + 1) ||
                    Game1.MapPath.IsRightTunnel(tilePos.PosX, tilePos.PosY - 1))
                {
                    Vector2 newPosition = new Vector2(Game1.MapPath.LeftTunnelPosition.PosX * 16 + 160,
                        (Game1.MapPath.LeftTunnelPosition.PosY + 1) * 16);

                    Position = newPosition;
                }
            }
        }
    }
}
