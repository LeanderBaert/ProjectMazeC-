using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace MazeLib.Models
{
    public interface IMaze3d 
    {
        int MazeLength { get; }
        int MazeWidth { get; }
        ModelVisual3D MazeVisual3d { get;  }
        List<Cuboid> MazeWalls3d { get; }
    }
}
