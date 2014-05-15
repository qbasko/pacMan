using JS.PacMan.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{

    public enum GhostMoveDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    class Ghost : Obj
    {
        private int startFrame;
        public Animation GhostAnimation;
        public bool isEatable;
        public string Name;
        public int DisabledTime { get; set; }
        int updateCounter = 0;
        GhostMoveDirection moveDirection;

        public Ghost(Vector2 pos, string textureName, int startFrame, string ghostName)
        {
            TextureName = textureName;
            Position = pos;
            isAlive = true;
            this.startFrame = startFrame;
            GhostAnimation = new Animation();
            isEatable = false;
            Name = ghostName;
            Speed = 2.0f;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
            GhostAnimation.Initialize(Texture, Position, 35, 32, 7, 120, Color.White, 1f, true, startFrame);
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                // if (updateCounter % 5 == 0)
                MoveGhost();

                updateCounter++;

                GhostAnimation.Position = Position;
                GhostAnimation.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                GhostAnimation.Draw(spriteBatch);
            }
        }


        public void MoveGhost()
        {
            TilePosition tilePos = tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
            TilePosition pacmanPosition = MapPath.ConvertToMapLocation(Items.Pacman.Position.X, Items.Pacman.Position.Y);
            //int num = 0;

            //if (pacmanPosition.PosY < tilePos.PosY)
            //    num = 1;
            //if (pacmanPosition.PosY > tilePos.PosY)
            //    num = 2;        

            if (moveDirection != GhostMoveDirection.None)
            {
                //List<GhostMoveDirection> movesDirections = CheckAvailableDirection(tilePos);

                //if (moveDirection == GhostMoveDirection.Up && movesDirections.Contains(GhostMoveDirection.Down))
                //    movesDirections.Remove(GhostMoveDirection.Down);

                //else if (moveDirection == GhostMoveDirection.Down && movesDirections.Contains(GhostMoveDirection.Up))
                //    movesDirections.Remove(GhostMoveDirection.Up);

                //else if (moveDirection == GhostMoveDirection.Left && movesDirections.Contains(GhostMoveDirection.Right))
                //    movesDirections.Remove(GhostMoveDirection.Right);

                //else if (moveDirection == GhostMoveDirection.Right && movesDirections.Contains(GhostMoveDirection.Left))
                //    movesDirections.Remove(GhostMoveDirection.Left);                    

                //else if (movesDirections.Contains(moveDirection) && movesDirections.Count == 3)
                //    moveDirection = ChooseDirection(movesDirections);
                if (updateCounter % 50 == 0)
                {
                    List<GhostMoveDirection> movesDirections = MoveToPacmanPosition(Items.Pacman.Position);
                    if (moveDirection == GhostMoveDirection.Up && movesDirections.Contains(GhostMoveDirection.Down))
                        movesDirections.Remove(GhostMoveDirection.Down);

                    else if (moveDirection == GhostMoveDirection.Down && movesDirections.Contains(GhostMoveDirection.Up))
                        movesDirections.Remove(GhostMoveDirection.Up);

                    else if (moveDirection == GhostMoveDirection.Left && movesDirections.Contains(GhostMoveDirection.Right))
                        movesDirections.Remove(GhostMoveDirection.Right);

                    else if (moveDirection == GhostMoveDirection.Right && movesDirections.Contains(GhostMoveDirection.Left))
                        movesDirections.Remove(GhostMoveDirection.Left);

                    //else if (movesDirections.Contains(moveDirection) && movesDirections.Count == 2)
                    if (!movesDirections.Contains(moveDirection))
                        moveDirection = ChooseDirection(movesDirections);

                    updateCounter = 0;
                }

                MoveGhost(ref tilePos, (int)moveDirection);
            }
            // if (moveDirection == GhostMoveDirection.None)
            //  MoveToPacmanPosition(Items.Pacman.Position);

            while (moveDirection == GhostMoveDirection.None)
            {
                Random rd = new Random();
                int num = rd.Next(1, 5);
                MoveGhost(ref tilePos, num);
            }
        }

        private void MoveGhost(ref TilePosition tilePos, int moveNumber)
        {
            TilePosition pacmanPosition = MapPath.ConvertToMapLocation(Items.Pacman.Position.X, Items.Pacman.Position.Y);

            switch (moveNumber)
            {
                case 1:
                    tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
                    if (Game1.MapPath.IsOpenLocation(tilePos.PosX + 1, tilePos.PosY) &&
                        Game1.MapPath.IsOpenLocation(tilePos.PosX - 1, tilePos.PosY - 1))
                    {
                        Position.Y -= Speed;
                        moveDirection = GhostMoveDirection.Up;                       

                       //Position.X = SnapToX(Position);
                    }                        
                    else
                        moveDirection = GhostMoveDirection.None;
                    CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                    break;
                case 2:
                    tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
                    if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY + 3))
                    {
                        Position.Y += Speed;
                        moveDirection = GhostMoveDirection.Down;
                        CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                        //Position.X = SnapToX(Position);
                    }
                    else
                        moveDirection = GhostMoveDirection.None;
                    CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                    break;
                case 3:
                    tilePos = MapPath.ConvertToMapLocation(Position.X - 16, Position.Y - Game1.TILE_SIZE);
                    if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY) &&                       
                        Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY + 1))
                    {
                        Position.X -= Speed;
                        moveDirection = GhostMoveDirection.Left;
                 
                        //Position.Y = SnapToY(Position);
                    }
                    else
                        moveDirection = GhostMoveDirection.None;
                    CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                    break;
                case 4:
                    tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
                    if (Game1.MapPath.IsOpenLocation(tilePos.PosX + 3, tilePos.PosY) &&
                        Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY + 1))
                    {
                        Position.X += Speed;
                        moveDirection = GhostMoveDirection.Right;
                        CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                        //Position.Y = SnapToY(Position);
                    }
                    else
                        moveDirection = GhostMoveDirection.None;
                    CheckCollisionsWithPacMan(tilePos, pacmanPosition);
                    break;
                default:
                    moveDirection = GhostMoveDirection.None;
                    break;
            }
        }

        private void CheckCollisionsWithPacMan(TilePosition tilePos, TilePosition pacmanPosition)
        {
            if (isAlive && Items.Pacman.isAlive && (
                tilePos.PosX == pacmanPosition.PosX ||
                tilePos.PosX + 1 == pacmanPosition.PosX ||
                tilePos.PosX + 2 == pacmanPosition.PosX))
            {
                if (tilePos.PosY == pacmanPosition.PosY ||
                    tilePos.PosY + 1 == pacmanPosition.PosY ||
                    tilePos.PosY + 2 == pacmanPosition.PosY)
                {
                    if (isEatable)
                    {
                        isAlive = false;
                        Items.Pacman.eatenDotsPoints += 200;
                        Items.Pacman.ghostEatedSoundInstance.Play();
                    }
                    else
                    {
                        Items.Pacman.isAlive = false;
                        Items.Pacman.numberOfLifes--;
                        Items.Pacman.Position = new Vector2(480, 565);
                        Items.Pacman.deathSoundInstance.Play();
                    }
                }
            }
        }

        //to do
        public List<GhostMoveDirection> MoveToPacmanPosition(Vector2 pacmanPosition)
        {
            TilePosition pacmanMapPosition = MapPath.ConvertToMapLocation(pacmanPosition.X, pacmanPosition.Y);
            TilePosition currentGhostPosition = MapPath.ConvertToMapLocation(Position.X, Position.Y);
            List<GhostMoveDirection> availableMoves = new List<GhostMoveDirection>();
            if (pacmanMapPosition.PosY > currentGhostPosition.PosY)
                availableMoves.Add(GhostMoveDirection.Down);
            else if (pacmanMapPosition.PosY < currentGhostPosition.PosY)
                availableMoves.Add(GhostMoveDirection.Up);
            if (pacmanMapPosition.PosX < currentGhostPosition.PosX)
                availableMoves.Add(GhostMoveDirection.Left);
            else if (pacmanMapPosition.PosX > currentGhostPosition.PosX)
                availableMoves.Add(GhostMoveDirection.Right);

            return availableMoves;
        }

        public GhostMoveDirection ChooseDirection(List<GhostMoveDirection> movesDirections)
        {
            if (movesDirections == null || movesDirections.Count == 0)
                return GhostMoveDirection.None;

            Random rd = new Random();
            int number = rd.Next(movesDirections.Count());
            return movesDirections[number];
        }

        private List<GhostMoveDirection> CheckAvailableDirection(TilePosition ghostPosition)
        {
            List<GhostMoveDirection> moveDirections = new List<GhostMoveDirection>();

            TilePosition tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
            if (Game1.MapPath.IsOpenLocation(tilePos.PosX + 1, tilePos.PosY) &&
                Game1.MapPath.IsOpenLocation(tilePos.PosX - 1, tilePos.PosY))
            {
                moveDirections.Add(GhostMoveDirection.Up);
            }

            tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
            if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY + 3))
            {
                moveDirections.Add(GhostMoveDirection.Down);
            }

            tilePos = MapPath.ConvertToMapLocation(Position.X - 16, Position.Y - Game1.TILE_SIZE);
            if (Game1.MapPath.IsOpenLocation(tilePos.PosX, tilePos.PosY))
            {
                moveDirections.Add(GhostMoveDirection.Left);
            }

            tilePos = MapPath.ConvertToMapLocation(Position.X, Position.Y - Game1.TILE_SIZE);
            if (Game1.MapPath.IsOpenLocation(tilePos.PosX + 3, tilePos.PosY))
            {
                moveDirections.Add(GhostMoveDirection.Right);
            }

            return moveDirections;
        }

        public float SnapToX(Vector2 pos)
        {
            pos -= new Vector2(pos.X % Game1.TILE_SIZE, pos.Y % Game1.TILE_SIZE);
            return pos.X + 8;
        }

        public float SnapToY(Vector2 pos)
        {
            pos -= new Vector2(pos.X % Game1.TILE_SIZE, pos.Y % Game1.TILE_SIZE);
            return pos.Y + 8;
        }
    }
}
