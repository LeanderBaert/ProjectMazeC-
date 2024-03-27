using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Maze3d.Models
{
    public interface IBall
    {
        int Pitch { get; set; }
        int Roll { get; set; }
        int BallSize { get; set; }
        double X { get; set; }
        double Z { get; set; }
        double Y { get; set; }
        Point3D Position { get; set; }
        double SpeedX { get; set; }
        double SpeedZ { get; set; }
        double AccelerationX { get; set; }
        double AccelerationZ { get; set; }
        double Speed { get; }
        double BounceEnergyLosse { get; set; }
        double RollEnergyLosse { get; set; }
        ModelVisual3D BallModelVisual3d { get; }
    }
}
