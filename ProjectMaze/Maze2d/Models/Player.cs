using MazeLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;

namespace Maze2d.Models
{
    public class Player : IPlayer
    {
        public Point Location { get; set; }
        private readonly int movementSpeed = 3;

        public Ellipse PlayerModel { get; } = new Ellipse() {
            Width = 10,
            Height = 10,
            Fill = Brushes.Red
        };

        public Player() {
            int beginNode = MazeProperties.MazeGridWidth  / 2;
            Location = new Point(beginNode * MazeProperties.gridUnitSize + MazeProperties.gridUnitSize / 2 + MazeProperties.gridUnitSize, MazeProperties.MazeStartEdge.Y - 5);
        }

        public void movementInput(bool up, bool down, bool left, bool right, List<Rectangle> walls)
        {
            Point newLocation = Location;

            for(int i = movementSpeed; i > 0; i--)
            {
                if (up && !down)
                {
                    --newLocation.Y;
                }
                if (down && !up)
                {
                    ++newLocation.Y;
                }
                if (left && !right)
                {
                    --newLocation.X;
                }
                if (right && !left)
                {
                    ++newLocation.X;
                }

                foreach (Rectangle wall in walls)
                {
                    if (CircleOverlapsRectangle((int)newLocation.X, newLocation.Y, (int)PlayerModel.Width / 2, wall.X, wall.Y, wall.Width, wall.Height))
                    {
                        return;
                    }
                }
            }
            Location = newLocation;
        }

        public static bool CircleOverlapsRectangle(double circleX, double circleY, double circleRadius, double rectangleX, double rectangleY, double rectangleWidth, double rectangleHeight)
        {
            //----------------------
            //   Bron --> chatgtp
            //----------------------

            // Calculate the center of the rectangle
            double rectCenterX = rectangleX + rectangleWidth / 2;
            double rectCenterY = rectangleY + rectangleHeight / 2;

            // Find the distance between the circle's center and the rectangle's center
            double dx = Math.Abs(circleX - rectCenterX);
            double dy = Math.Abs(circleY - rectCenterY);

            // Check if the circle is too far away to intersect with the rectangle
            if (dx > (rectangleWidth / 2 + circleRadius) || dy > (rectangleHeight / 2 + circleRadius))
            {
                return false;
            }

            // Check if the circle is within the rectangle bounds
            if (dx <= (rectangleWidth / 2) || dy <= (rectangleHeight / 2))
            {
                return true;
            }

            // Check if the circle intersects with the rectangle's corners
            double cornerDistance = Math.Pow(dx - rectangleWidth / 2, 2) + Math.Pow(dy - rectangleHeight / 2, 2);
            return cornerDistance <= (circleRadius * circleRadius);
        }
    }
}
