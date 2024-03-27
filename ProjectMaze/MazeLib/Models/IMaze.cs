using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;

namespace MazeLib.Models
{
    public interface IMaze
    {
        List<Rectangle> MazeWalls { get; set; }
        Point MazeStartEdge { get; set; }
        int MazeLength { get; }
        int MazeWidth { get; }
    }
}
