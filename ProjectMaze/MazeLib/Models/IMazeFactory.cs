using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Models
{
    public interface IMazeFactory
    {
        IMaze Maze2d { get; }
        IMaze3d Maze3d { get; }
    }
}
