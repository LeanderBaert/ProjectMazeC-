using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Models
{
    public static class MazeProperties
    {
        public static readonly Point MazeStartEdge = new Point(30, 30);

        public static readonly int gridUnitSize = 30;

        public static readonly int MazeGridLength = 13;
        public static readonly int MazeGridWidth = 13;
        public static readonly int WallThickness = 3;

    }
}
