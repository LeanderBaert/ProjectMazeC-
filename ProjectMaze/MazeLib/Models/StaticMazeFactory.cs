using System;
using System.Collections.Generic;
using System.Drawing;


namespace MazeLib.Models
{
    public class StaticMazeFactory : IMazeFactory
    {
        private List<bool[]> HorizontalWalls = new List<bool[]>();
        private List<bool[]> VerticalWalls = new List<bool[]>();

        public readonly int MazeGridLenght = 13;
        public readonly int MazeGridWith = 13;
        public readonly int gridUnitSize = 40;

        private Point MazeStartEdge;

        public IMaze Maze2d => new Maze(HorizontalWalls, VerticalWalls, MazeGridLenght, MazeGridWith, gridUnitSize, MazeStartEdge);
        public IMaze3d Maze3d => new Maze3d(HorizontalWalls, VerticalWalls, MazeGridLenght, MazeGridWith, gridUnitSize, MazeStartEdge);

        public StaticMazeFactory(Point MazeStartEdge) 
        {
            this.MazeStartEdge = MazeStartEdge;

            VerticalWalls.Add(new bool[] { false, true, true, false, false, true, true, false, false, true, false, false });
            VerticalWalls.Add(new bool[] { true, true, true, true, false, true, false, true, true, true, false, true});
            VerticalWalls.Add(new bool[] { true, true, true, false, true, false, true, true, true, false, false, false });
            VerticalWalls.Add(new bool[] { true, true, false, false, false, true, false, true, false, false, false, false });
            VerticalWalls.Add(new bool[] { true, false, false, false, true, false, true, true, true, false, true, true });
            VerticalWalls.Add(new bool[] { false, false, false, true, false, true, false, false, true, true, true, true });
            VerticalWalls.Add(new bool[] { false, true, true, false, true, false, false, false, true, false, false, true });
            VerticalWalls.Add(new bool[] { false, false, true, true, false, true, false, true, false, false, false, true });
            VerticalWalls.Add(new bool[] { false, true, false, true, true, true, true, true, false, false, false, true });
            VerticalWalls.Add(new bool[] { false, false, true, true, true, false, true, false, false, false, true, false});
            VerticalWalls.Add(new bool[] { true, true, true, true, false, false, false, false, false, true, false, false });
            VerticalWalls.Add(new bool[] { true, false, true, false, false, false, false, true, true, true, true, true });
            VerticalWalls.Add(new bool[] { false, false, false, false, false, false, true, false, true, false, true, false });


            HorizontalWalls.Add(new bool[] { false, false, false, false, true, true, false, false, false, false, false, true, false });
            HorizontalWalls.Add(new bool[] { false, false, false, false, false, false, false, true, false, false, true, true, false });
            HorizontalWalls.Add(new bool[] { false, false, false, true, true, true, true, false, false, true, true, true, true });
            HorizontalWalls.Add(new bool[] { false, false, true, true, true, false, true, false, false, true, true, false, false });
            HorizontalWalls.Add(new bool[] { false, true, true, true, false, true, false, false, false, false, false, false, false });
            HorizontalWalls.Add(new bool[] { true, false, true, true, true, false, true, true, true, false, true, false, false, false });
            HorizontalWalls.Add(new bool[] { false, true, false, false, false, false, true, true, false, true, true, false, false });
            HorizontalWalls.Add(new bool[] { true, false, false, true, false, true, false, false, false, true, true, true, false });
            HorizontalWalls.Add(new bool[] { false, true, true, false, false, false, false, false, true, true, false, true, false });
            HorizontalWalls.Add(new bool[] { false, true, false, false, false, true, true, true, true, true, false, true, true });
            HorizontalWalls.Add(new bool[] { false, false, false, false, true, true, true, true, true, false, true, true, false });
            HorizontalWalls.Add(new bool[] { false, true, true, true, true, true, true, false, false, false, false, false, false });        
        }
    }
}
