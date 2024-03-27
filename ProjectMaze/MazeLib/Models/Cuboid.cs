using MazeLib.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;

namespace MazeLib.Models
{
    public class Cuboid
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public Color Color { get; set; }
        public bool HasCollision { get; set; } = true;
        public Dictionary<CuboidSide, Line> CollisionLine => new Dictionary<CuboidSide, Line>()
        {
            {CuboidSide.Front, new Line() {X1 = X, Y1 = Z, X2 = X + Width, Y2 = Z}},                 //Front side
            {CuboidSide.Back, new Line() {X1 = X, Y1 = Z + Depth, X2 = X + Width, Y2 = Z + Depth}},  //Back side
            {CuboidSide.Left, new Line() {X1 = X, Y1 = Z , X2 = X , Y2 = Z + Depth}},               //Left side
            {CuboidSide.Right, new Line() {X1 = X + Width, Y1 = Z, X2 = X + Width, Y2 = Z + Depth}} //Right side
        };
        public GeometryModel3D GetWallGeometry => new GeometryModel3D
        {
            /*Bron -->
            Bardai
            url --> https://g.co/bard/share/5becbe507d9d
            */

            Geometry = new MeshGeometry3D
            {
                Positions = new Point3DCollection
                {
                        new Point3D(X, Y, Z),
                        new Point3D(X + Width, Y, Z),
                        new Point3D(X + Width, Y + Height, Z),
                        new Point3D(X, Y + Height, Z),
                        new Point3D(X, Y, Z + Depth),
                        new Point3D(X + Width, Y, Z + Depth),
                        new Point3D(X + Width, Y + Height, Z + Depth),
                        new Point3D(X, Y + Height, Z + Depth),
                },
                TriangleIndices = new Int32Collection
                {
                        // Front face
                        0, 2, 1,
                        0, 3, 2, 
                        // Back face
                        4, 5, 6,
                        4, 6, 7, 
                        // Left face
                        0, 1, 5,
                        0, 5, 4, 
                        // Right face
                        1, 2, 6,
                        1, 6, 5,
                        // Top face
                        2, 3, 7,
                        2, 7, 6, 
                        // Bottom face
                        3, 0, 4,
                        3, 4, 7
                }
            },
            Material = new DiffuseMaterial(new SolidColorBrush(Color)),
        };

        public Cuboid(int X, int Y, int Z, int Width, int Height, int Depth, Color Color)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
            this.Color = Color;
        }
    }
}
