using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan.Map
{
    public class MapPath
    {
        public MapTileType[,] pathTable { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public TilePosition LeftTunnelPosition { get; set; }
        public TilePosition RightTunnelPosition { get; set; }

        public MapPath(List<Tile> tilesList, int rows, int columns)
        {
            pathTable = new MapTileType[columns + 1, rows + 1];
            Rows = rows;
            Columns = columns;

            foreach (var tile in tilesList)
            {
                //int x = (int)((tile.Position.X -160) / 16);
                //int y = (int)((tile.Position.Y + 32) / 16);
                if (tile.Selected == 1)
                    pathTable[tile.ColumnNumber, tile.RowNumber] = MapTileType.MapBarrier;
                else if (tile.Selected == 0)
                    pathTable[tile.ColumnNumber, tile.RowNumber] = MapTileType.Dot;
                else if (tile.Selected == 9)
                    pathTable[tile.ColumnNumber, tile.RowNumber] = MapTileType.SuperDot;
                else if (tile.Selected == 8)
                {
                    pathTable[tile.ColumnNumber, tile.RowNumber] = MapTileType.TunnelLeft;
                    LeftTunnelPosition = new TilePosition() { PosX = tile.ColumnNumber, PosY = tile.RowNumber };
                }
                else if (tile.Selected == 7)
                {
                    pathTable[tile.ColumnNumber, tile.RowNumber] = MapTileType.TunnelRight;
                    RightTunnelPosition = new TilePosition() { PosX = tile.ColumnNumber, PosY = tile.RowNumber };
                }
            }
        }

        public bool InMap(int column, int row)
        {
            return (row >= 0 && row < Rows &&
                column >= 0 && column < Columns);
        }

        public bool IsOpenLocation(int column, int row)
        {
            return InMap(column, row) && pathTable[column, row] != MapTileType.MapBarrier;
        }

        public bool IsDotActive(int column, int row)
        {
            return InMap(column, row) && pathTable[column, row] == MapTileType.Dot;
        }

        public bool IsSuperDotActive(int column, int row)
        {
            return InMap(column, row) && pathTable[column, row] == MapTileType.SuperDot;
        }
        public bool IsLeftTunnel(int column, int row)
        {
            return InMap(column, row) && pathTable[column, row] == MapTileType.TunnelLeft;
        }

        public bool IsRightTunnel(int column, int row)
        {
            return InMap(column, row) && pathTable[column, row] == MapTileType.TunnelRight;
        }

        public void DeactivateDot(int column, int row)
        {
            pathTable[column, row] = MapTileType.MapEmpty;
        }

        public static TilePosition ConvertToMapLocation(float posX, float posY)
        {
            return new TilePosition() { PosX = (int)((posX - 160) / 16), PosY = (int)(posY / 16) };
        }


    }
}
