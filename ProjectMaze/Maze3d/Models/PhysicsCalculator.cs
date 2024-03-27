using MazeLib.Enums;
using MazeLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Maze3d.Models
{
    public class PhysicsCalculator : IPhysicsCalculator
    {
        private IBall Ball { get; set; }
        public IMaze3d Maze {  get; set; }

        private DispatcherTimer Timer;
        private int TicksPerSeconds = 100;
        private int TickDuration => 1000 / TicksPerSeconds;
        public PhysicsCalculator(IBall Ball, IMaze3d Maze) {
            this.Ball = Ball;
            this.Maze = Maze;

            //<<<--- Create game tick clock --->>>
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(TickDuration);
            Timer.Tick += PhysicsLoop;
            Timer.Start();
        }

        private void PhysicsLoop(object sender, EventArgs e)
        {
            Ball.AccelerationX = ((double)Ball.Roll)/ (TicksPerSeconds * 0.3);
            Ball.AccelerationZ = ((double)Ball.Pitch)/ (TicksPerSeconds * 0.3);

            Ball.SpeedX += Ball.AccelerationX;
            Ball.SpeedZ += Ball.AccelerationZ;

            Ball.SpeedX = Ball.SpeedX * Ball.RollEnergyLosse;
            Ball.SpeedZ = Ball.SpeedZ * Ball.RollEnergyLosse;

            BallCollisionCalculator();
            Ball.X -= Ball.SpeedX;
            Ball.Z += Ball.SpeedZ;
        }

        private void BallCollisionCalculator()
        {
            double nextX = Ball.X - Ball.SpeedX;
            double nextZ = Ball.Z + Ball.SpeedZ;

            foreach (Cuboid cuboid in Maze.MazeWalls3d)
            {
                if (cuboid.HasCollision) // To ignore the floor cuboid
                {
                    foreach (KeyValuePair<CuboidSide, Line> CollisionLine in cuboid.CollisionLine)
                    {

                        HasColisionZ(CollisionLine, (int)Ball.X, (int)nextZ);
                        HasColisionX(CollisionLine, (int)nextX, (int)Ball.Z);
                    }
                }
            }
        }

        private void HasColisionZ(KeyValuePair<CuboidSide, Line> CollisionLine, int ballX, int ballZ)
        {
            Line line = CollisionLine.Value;
            CuboidSide cuboidSide = CollisionLine.Key;

            if (CirkelOverlapsLine((float)line.X1, (float)line.Y1, (float)line.X2, (float)line.Y2, ballX, ballZ, Ball.BallSize))
            {
                if (cuboidSide == CuboidSide.Front)
                {
                    Ball.SpeedZ = TurnNegative(Ball.SpeedZ);
                    Ball.SpeedZ = (int)(Ball.SpeedZ * Ball.BounceEnergyLosse);
                }
                else if (cuboidSide == CuboidSide.Back)
                {
                    Ball.SpeedZ = TurnPositive(Ball.SpeedZ);
                    Ball.SpeedZ = (int)(Ball.SpeedZ * Ball.BounceEnergyLosse);
                }
            }
        }

        private void HasColisionX(KeyValuePair<CuboidSide, Line> CollisionLine, int ballX, int ballZ)
        {
            Line line = CollisionLine.Value;
            CuboidSide cuboidSide = CollisionLine.Key;

            if (CirkelOverlapsLine((float)line.X1, (float)line.Y1, (float)line.X2, (float)line.Y2, ballX, ballZ, Ball.BallSize))
            {
                if (cuboidSide == CuboidSide.Right)
                {
                    Ball.SpeedX = TurnNegative(Ball.SpeedX);
                    Ball.SpeedX = (int)(Ball.SpeedX * Ball.BounceEnergyLosse);
                }
                else if (cuboidSide == CuboidSide.Left)
                {
                    Ball.SpeedX = TurnPositive(Ball.SpeedX);
                    Ball.SpeedX = (int)(Ball.SpeedX * Ball.BounceEnergyLosse);
                }
            }
        }

        private bool CirkelOverlapsLine(float x1, float y1, float x2, float y2, float xc, float yc, float straal)
        {
            /*Bron -->
            Java OO programming eind project
            class --> Meetkunde
            */

            float A = xc - x1;
            float B = yc - y1;
            float C = x2 - x1;
            float D = y2 - y1;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float param = -1;
            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            float xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            float dx = xc - xx;
            float dy = yc - yy;

            return dx * dx + dy * dy <= Math.Pow(straal, 2);
        }

        private double TurnPositive(double input)
        {
            if(input < 0) return 0 - input;
            return input;
        }

        private double TurnNegative(double input)
        {
            if (input > 0) return 0 - input;
            return input;
        }
    }
}
