using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;

namespace MazeLib.Models
{
    public class Maze : IMaze
    {
        public Point MazeStartEdge { get; set; }
        private int gridUnitSize = MazeProperties.gridUnitSize;

        private int MazeGridLenght = MazeProperties.MazeGridLength   - 1;
        private int MazeGridWith  = MazeProperties.MazeGridWidth  - 1;

        private readonly int WallThickness = MazeProperties.WallThickness;

        public int MazeLength => MazeProperties.MazeGridLength   * MazeProperties.gridUnitSize;
        public int MazeWidth => MazeProperties.MazeGridWidth  * MazeProperties.gridUnitSize;


        public List<Rectangle> MazeWalls { get; set; } = new List<Rectangle>();

        public Maze(List<Node> Nodes, Point MazeStartEdge) 
        {
            this.MazeStartEdge = MazeStartEdge;
            MazeWalls = GraphToWalls(Nodes);
        }

        public Maze(List<bool[]> HorizontalWalls, List<bool[]> VerticalWalls, int MazeGridLenght, int MazeGridWith, int gridUnitSize, Point MazeStartEdge)
        {
            this.MazeStartEdge = MazeStartEdge;

            MazeWalls = BoolsToHorizontalWalls(HorizontalWalls).Concat(BoolsToVerticalWalls(VerticalWalls)).ToList();
            this.MazeGridLenght = MazeGridLenght;
            this.MazeGridWith = MazeGridWith;
            this.gridUnitSize = gridUnitSize;
        }

        private List<Rectangle> BoolsToHorizontalWalls(List<bool[]> HorizontalWalls)
        {
            List<Rectangle> horizontalWallLines = new List<Rectangle>();

            int startX = MazeStartEdge.X;
            int startY = MazeStartEdge.Y + gridUnitSize;

            foreach (bool[] row in HorizontalWalls)
            {
                int x1 = startX;
                int x2 = startX + gridUnitSize;
                int y1 = startY;
                int y2 = startY;

                foreach (bool wall in row)
                {
                    if (wall)
                    {
                        horizontalWallLines.Add(new Rectangle
                        {
                            Width = gridUnitSize, // Set the width of the rectangle
                            Height = WallThickness, // Set the height of the rectangle
                            X = x1,
                            Y = y1, 
                            
                        });
                        //horizontalWallLines.Add(new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 });
                    }
                    x1 += gridUnitSize;
                    x2 += gridUnitSize;
                }

                startY += gridUnitSize;
            }
            int doorXValue = MazeGridWith / 2;

            //Add above wall with door
            horizontalWallLines.Add(new Rectangle { X = MazeStartEdge.X, Y = MazeStartEdge.Y, Width = (MazeGridWith / 2) * gridUnitSize, Height = WallThickness });
            horizontalWallLines.Add(new Rectangle { X = MazeStartEdge.X + (MazeGridWith / 2) * gridUnitSize + gridUnitSize, Y = MazeStartEdge.Y, Width = (MazeGridWith / 2) * gridUnitSize, Height = WallThickness });

            ////Add below wall with door
            horizontalWallLines.Add(new Rectangle { X = MazeStartEdge.X, Y = MazeStartEdge.Y + gridUnitSize * MazeGridWith + gridUnitSize, Width = (MazeGridWith / 2) * gridUnitSize, Height = WallThickness });
            horizontalWallLines.Add(new Rectangle { X = MazeStartEdge.X + (MazeGridWith / 2) * gridUnitSize + gridUnitSize, Y = MazeStartEdge.Y + gridUnitSize * MazeGridWith + gridUnitSize, Width = (MazeGridWith / 2) * gridUnitSize, Height = WallThickness });

            return horizontalWallLines;
        }

        private List<Rectangle> BoolsToVerticalWalls(List<bool[]> VerticalWalls)
        {
            List<Rectangle> verticalWallLines = new List<Rectangle>();

            int startX = MazeStartEdge.X + gridUnitSize;
            int startY = MazeStartEdge.Y;

            foreach (bool[] row in VerticalWalls)
            {
                int x1 = startX;
                int x2 = startX;
                int y1 = startY;
                int y2 = startY + gridUnitSize;

                foreach (bool wall in row)
                {
                    if (wall)
                    {
                        verticalWallLines.Add(new Rectangle
                        {
                            Width = WallThickness, // Set the width of the rectangle
                            Height = gridUnitSize, // Set the height of the rectangle
                            X = x1,
                            Y = y1,

                        });
                    }
                    x1 += gridUnitSize;
                    x2 += gridUnitSize;
                }

                startY += gridUnitSize;
            }
            //Add left wall
            verticalWallLines.Add(new Rectangle { X = MazeStartEdge.X, Y = MazeStartEdge.Y, Width = WallThickness, Height = gridUnitSize * MazeGridWith + gridUnitSize });
            ////Add right wall
            verticalWallLines.Add(new Rectangle { X = MazeStartEdge.X + gridUnitSize * MazeGridWith + gridUnitSize, Y = MazeStartEdge.Y, Width = WallThickness, Height = gridUnitSize * MazeGridWith + gridUnitSize });

            return verticalWallLines;
        }

        private List<Rectangle> GraphToWalls(List<Node> Nodes)
        {
            List<bool[]> HorizontalWalls = Enumerable.Range(0, MazeGridLenght)
           .Select(_ => Enumerable.Repeat(true, MazeGridWith + 1).ToArray())
           .ToList();

            List<bool[]> VerticalWalls = Enumerable.Range(0, MazeGridLenght + 1)
           .Select(_ => Enumerable.Repeat(true, MazeGridWith).ToArray())
           .ToList();

            foreach (Node node1 in Nodes)
            {
                foreach (Node node2 in node1.Edges)
                {
                    if (node1.Location.X == (node2.Location.X + 1)) { VerticalWalls[node2.Location.Y][node2.Location.X] = false; }
                    if (node1.Location.Y == (node2.Location.Y + 1)) { HorizontalWalls[node2.Location.Y][node2.Location.X] = false; }
                    if (node1.Location.X == (node2.Location.X - 1)) { VerticalWalls[node1.Location.Y][node1.Location.X] = false; }
                    if (node1.Location.Y == (node2.Location.Y - 1)) { HorizontalWalls[node1.Location.Y][node1.Location.X] = false; }
                }
            }

            List<Rectangle> output = BoolsToHorizontalWalls(HorizontalWalls).Concat(BoolsToVerticalWalls(VerticalWalls)).ToList();
            return output;
        }
    }
}
