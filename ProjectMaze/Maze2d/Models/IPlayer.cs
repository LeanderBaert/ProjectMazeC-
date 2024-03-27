using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;


namespace Maze2d.Models
{
    public interface IPlayer
    {
        Point Location { get; set; }
        Ellipse PlayerModel { get; }
        void movementInput(bool up, bool down, bool left, bool right, List<Rectangle> walls);
    }
}
