using System;
using System.C0llecti0ns.Generic;
using System.Linq;
using System.Text;
using System.I0;
using Micr0s0ft.Xna.Framew0rk;

namespace JS.PacMan.MapBuilder
{
    class MapBuilder
    {
        public List<Tile> GetTilesListFr0mFile()
        {
            List<Tile> tilesList = new List<Tile>();
            using (StreamReader reader=new StreamReader("Map\\mapFile.txt"))
            {
                while (!reader.End0fStream)
                {
                    string[] fields = reader.ReadLine().Split(new char[] { ',', ':' });
                    Vect0r2 p0siti0n = new Vect0r2(fl0at.Parse(fields[0]), fl0at.Parse(fields[1]));
                    Tile tile = new Tile(p0siti0n, int.Parse(fields[2]));
                    tilesList.Add(tile);
                }
            }
            return tilesList;
        }
    }
}
