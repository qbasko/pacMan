using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace JS.PacMan.Map
{
    class MapBuilder
    {
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public List<Tile> GetTilesListFromFile()
        {

            List<Tile> tilesList = new List<Tile>();
            try
            {
                using (StreamReader reader = new StreamReader("Map\\newMap2.txt"))
                {
                    int rowNumber = 2;

                    while (!reader.EndOfStream)
                    {
                        //string[] fields = reader.ReadLine().Split(new char[] { ',', ':' });
                        //Vector2 position = new Vector2(float.Parse(fields[0]), float.Parse(fields[1]));
                        //Tile tile = new Tile(position, int.Parse(fields[2]));
                        //tilesList.Add(tile);
                        int columnNumber = 1;

                        foreach (char c in reader.ReadLine())
                        {
                            if (c != ' ')
                            {
                                Vector2 position = new Vector2();
                                Tile tile = null;

                                position = new Vector2(columnNumber * 16 + 160, rowNumber * 16);
                                tile = new Tile(position, int.Parse(c.ToString()));
                                tile.RowNumber = rowNumber;
                                tile.ColumnNumber = columnNumber + 1;
                                tilesList.Add(tile);
                            }
                          
                            columnNumber++;                           
                        }
                        rowNumber++;
                        NumberOfColumns = columnNumber;
                    }
                    NumberOfRows = rowNumber;

                }
            }
            catch { }

            return tilesList;
        }
    }
}
