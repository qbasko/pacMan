using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JS.PacMan.PathData
{
    public class MapData
    {
        public int NumberRows;
        public int NumberColumns;
        public List<Point> Barriers;

        public MapData()
        {

        }

        public MapData(int columns, int rows, List<Point> barriers)
        {
            NumberColumns = columns;
            NumberRows = rows;
            Barriers = barriers;
        }
    }
}
