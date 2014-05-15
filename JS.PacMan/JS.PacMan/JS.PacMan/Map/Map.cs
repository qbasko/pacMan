using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JS.PacMan.PathData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace JS.PacMan.Map
{
    public class Map
    {
        private int numberColumns;
        private int numberRows;
        private int currentMap = 0;
        private List<MapData> maps;
        private MapTileType[,] mapTiles;

        public void LoadContent(ContentManager content)
        {
            maps = new List<MapData>();
            maps.Add(content.Load<MapData>("map\\Map1"));
            SetMapData();
        }

        private void SetMapData()
        {
            numberColumns = maps[currentMap].NumberColumns;
            numberRows = maps[currentMap].NumberRows;
            mapTiles = new MapTileType[numberColumns, numberRows];
            int x = 0;
            int y = 0;
            for (int i = 0; i < maps[currentMap].Barriers.Count; i++)
            {
                x = (maps[currentMap].Barriers[i].X - 160) / Game1.TILE_SIZE;
                y = maps[currentMap].Barriers[i].Y / Game1.TILE_SIZE;
                mapTiles[x, y] = MapTileType.MapBarrier;
            }
        }

        public bool InMap(Point point)
        {
            return (point.Y >= 0 && point.Y < numberRows &&
                point.X >= 0 && point.X < numberColumns);
        }

        public bool InMap(int column, int row)
        {
            return (row >= 0 && row < numberRows &&
                column >= 0 && column < numberColumns);
        }

        public bool IsOpenLocation(int column, int row)
        {
            return InMap(column, row) && mapTiles[column, row] != MapTileType.MapBarrier;
        }
    }
}
