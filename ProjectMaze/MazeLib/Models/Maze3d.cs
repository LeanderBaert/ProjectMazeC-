using MazeLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Color = System.Windows.Media.Color;
using Rectangle = System.Drawing.Rectangle;

namespace MazeLib.Models
{
    class Maze3d : Maze, IMaze3d
    {
        public ModelVisual3D MazeVisual3d { get; private set; } = new ModelVisual3D();
        public List<Cuboid> MazeWalls3d { get; private set; } = new List<Cuboid>();

        private readonly int WallHight = 15;
        private readonly int floorThickness = 3;
        private readonly int floor_Y = 0;


        public Maze3d(List<Node> Nodes, Point MazeStartEdge) : base(Nodes, MazeStartEdge)
        {
            MazeWalls3d = GenerateMazeCuboids();
            MazeVisual3d = MazeModelVisual3D();
        }

        public Maze3d(List<bool[]> HorizontalWalls, List<bool[]> VerticalWalls, int MazeGridLenght, int MazeGridWith, int gridUnitSize, Point MazeStartEdge) : base(HorizontalWalls, VerticalWalls, MazeGridLenght, MazeGridWith, gridUnitSize, MazeStartEdge)
        {
            MazeWalls3d = GenerateMazeCuboids();
            MazeVisual3d = MazeModelVisual3D();
        }

        private List<Cuboid> GenerateMazeCuboids()
        {
            List<Cuboid> MazeWalls3d = new List<Cuboid>();

            //Add all walls
            foreach (Rectangle wall in MazeWalls)
            {
                Color color;
                Random random = new Random();
                switch (random.Next(3))
                {
                    case 0: color = Colors.Bisque; break;
                    case 1: color = Colors.Gold; break;
                    case 2: color = Colors.GreenYellow; break;
                }

                MazeWalls3d.Add(new Cuboid(wall.X, floor_Y + floorThickness, wall.Y, wall.Width, WallHight, wall.Height, color));
            }

            //Add a floor
            int xCo = MazeStartEdge.X;
            int zCo = MazeStartEdge.Y;
            int mazeLenght = MazeLength;
            int mazeWidth = MazeWidth;

            MazeWalls3d.Add(new Cuboid(xCo, floor_Y, zCo, mazeWidth, floorThickness, mazeLenght, Colors.DarkSlateGray) {HasCollision = false});

            return MazeWalls3d;
        }
        private ModelVisual3D MazeModelVisual3D()
        {
            Model3DGroup ModelGroup = new Model3DGroup();

            foreach(Cuboid mazeWall in MazeWalls3d)
            {
                ModelGroup.Children.Add(mazeWall.GetWallGeometry);
            }

            return new ModelVisual3D
            {
                Content = ModelGroup
            };
        }
    }
}