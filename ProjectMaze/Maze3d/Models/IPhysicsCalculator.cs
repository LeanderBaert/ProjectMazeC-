using MazeLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze3d.Models
{
    public interface IPhysicsCalculator
    {
        IMaze3d Maze { get; set; }
    }
}
