using HelixToolkit.Wpf;
using MazeLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Vector3D = System.Numerics.Vector3;

namespace Maze3d.Models
{
    public class Ball : IBall
    { 
        public int Pitch { get; set; } = 0 ;
        public int Roll { get; set; } = 0;
        public int BallSize { get; set; } = 10;

        public double X { get; set; }
        public double Z { get; set; }
        public double Y { get; set; }
        public Point3D Position { 
            get { return new Point3D(X, Y, Z); } 
            set { 
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            } 
        }

        public double SpeedX { get; set; } = 0;
        public double SpeedZ { get; set; } = 0;
        public double AccelerationX { get; set; } = 0;
        public double AccelerationZ { get; set; } = 0;
        public double Speed => Math.Sqrt(Math.Pow(SpeedX, 2) + Math.Pow(SpeedZ, 2));
        private double MaxSpeed = 2;

        public double BounceEnergyLosse { get; set; } = 0.75;
        public double RollEnergyLosse { get; set; } = 0.97;

        public ModelVisual3D BallModelVisual3d => CreateBallModel();
        public Ball(Point3D spawnLocation) {
            X = (int)spawnLocation.X;
            Z = (int)spawnLocation.Z;
            Y = (int)spawnLocation.Y;
        }
        private ModelVisual3D CreateBallModel()
        {
            //--------Bron!!!--------
            //Hot examples url-->> https://csharp.hotexamples.com/examples/HelixToolkit.Wpf/MeshBuilder/AddSphere/php-meshbuilder-addsphere-method-examples.html
            Point3D location = new Point3D { X = X, Z = Z , Y = Y};

            GeometryModel3D model = new GeometryModel3D();
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(location, BallSize, 100, 50);
            model.Geometry = meshBuilder.ToMesh();
            model.Material = Materials.Blue;

            return new ModelVisual3D {Content = model};
        } 
    }
}
