using Maze3d.Models;
using MazeLib.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace Maze3d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int TiltLimit = 5;
        private int _pitch = 0;
        private int Pitch
        {
            get { return _pitch; }
            set { if (value <= TiltLimit && value >= -TiltLimit) _pitch = value; }
        }
        private int _Roll = 0;
        private int Roll
        {
            get { return _Roll; }
            set { if (value <= TiltLimit && value >= -TiltLimit) _Roll = value; }
        }

        private IBall Ball;
        private IMaze3d Maze;
        private ModelVisual3D BallModelVisual;
        private ModelVisual3D MazeModelVisual;

        private IPhysicsCalculator PhysicsCalculator;

        private DispatcherTimer Timer;
        private int TickRate = 20;

        private bool InputUp, InputDown, InputLeft, InputRight = false;

        private readonly Point3D SpawnPoint;


        public MainWindow()
        {
            InitializeComponent();

            //<<<--- Generate maze --->>>
            IMazeFactory mazeFactory = new SubtrAlgoMazeFactroy(new Point() { X = 0, Y = 0});
            Maze = mazeFactory.Maze3d;

            //<<<--- Create maze vieuw model --->>>
            MazeModelVisual = Maze.MazeVisual3d;

            //<<<--- Create ball vieuw model --->>>
            SpawnPoint = new Point3D(Maze.MazeWidth / 2, 10, 4);
            Ball = new Ball(SpawnPoint);
            BallModelVisual = Ball.BallModelVisual3d;

            //<<<--- Initialize physics class --->>>
            PhysicsCalculator = new PhysicsCalculator(Ball, Maze);

            //<<<--- Add to 3d canvas --->>>
            viewport3D.Children.Add(MazeModelVisual);
            viewport3D.Children.Add(BallModelVisual);


            //<<<--- Add camera to viewport3D --->>>
            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(900, 1050, 1000),
                LookDirection = new Vector3D(-0.85, -1.33, -1),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 37
            };
            viewport3D.Camera = camera;

            //<<<--- Create game tick clock --->>>
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(TickRate); 
            Timer.Tick += GameTick; 
            Timer.Start();

            //<<<--- Bring text label to foreground --->>>
            UserLabel.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z || e.Key == Key.Up)
            {
                InputUp = true;
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                InputDown = true;
            }
            else if (e.Key == Key.Q || e.Key == Key.Left)
            {
                InputLeft = true;
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                InputRight = true;
            }
        }
        private void GameTick(object sender, EventArgs e)
        {
            //<<<--- Save old entity values --->>>
            int oldPitch = Pitch;
            int oldRoll = Roll;
            double oldBallX = Ball.X;
            double oldBallY = Ball.Y;
            double oldBallZ = Ball.Z;

            //<<<--- Procces player input --->>>
            if (InputUp)
            {Pitch -= 1;}
            else if (InputDown)
            {Pitch += 1;}
            else if (InputLeft)
            {Roll += 1;}
            else if (InputRight)
            { Roll -= 1;}

            InputUp = false;
            InputDown = false;
            InputLeft = false;
            InputRight = false;

            //<<<--- sync Ball.Tilt with aniamtion tilt --->>>
            Ball.Roll = Roll;
            Ball.Pitch = Pitch;

            //<<<--- Render visuals --->>>
            RenderLoop(oldPitch, oldRoll, oldBallX, oldBallY, oldBallZ);

            //<<<--- Call CompletionEvent if ball finds exit --->>>
            if (Ball.Z >= Maze.MazeLength)
            {
                CompletionEvent();
            }
        }

        private void CompletionEvent()
        {
            Ball.Position = SpawnPoint;

            viewport3D.Children.Remove(MazeModelVisual);

            //<<<--- Generate maze --->>>
            IMazeFactory mazeFactory = new SubtrAlgoMazeFactroy(new Point() { X = 0, Y = 0 });
            Maze = mazeFactory.Maze3d;

            //<<<--- Create maze vieuw model --->>>
            MazeModelVisual = Maze.MazeVisual3d;

            PhysicsCalculator.Maze = Maze;
            viewport3D.Children.Add(MazeModelVisual);
        }

        private void RenderLoop(int oldPitch, int oldRoll, double oldBallX, double oldBallY, double oldBallZ)
        {
            SetAnimations(oldPitch, oldRoll, oldBallX, oldBallY, oldBallZ);
            UserLabel.Content = $"Maze tilt:\n" +
                $" Roll = {Roll}\n" +
                $" Pitch = {Pitch}\n" +
                $"Ball speed = {Math.Round(Ball.Speed, 1)}";
        }

        private void SetAnimations(int oldPitch, int oldRoll, double oldBallX, double oldBallY, double oldBallZ)
        {
            /*Bron -->
            WPF3D-demo
            Project --> Moving body

            https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-animate-a-3-d-rotation-using-rotation3danimation?view=netframeworkdesktop-4.8
            https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/3-d-graphics-overview?view=netframeworkdesktop-4.8
            */

            //<<<--- Create rotation transformation --->>>
            RotateTransform3D pitchRotate = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), oldPitch));
            RotateTransform3D rollRotate = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), oldRoll));

            pitchRotate.CenterX = Maze.MazeLength / 2;
            pitchRotate.CenterY = 0;
            pitchRotate.CenterZ = Maze.MazeLength / 2;

            rollRotate.CenterX = Maze.MazeLength / 2;
            rollRotate.CenterY = 0;
            rollRotate.CenterZ = Maze.MazeLength / 2;

            //<<<--- Create & apply rotation animation --->>>
            DoubleAnimation pitchAnimation = new DoubleAnimation(Pitch, new Duration(TimeSpan.FromSeconds(5000)));
            DoubleAnimation rollAnimation = new DoubleAnimation(Roll, new Duration(TimeSpan.FromSeconds(5000)));
            pitchRotate.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, pitchAnimation);
            rollRotate.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, rollAnimation);


            //<<--- Initialize translation transformation --->>>
            TranslateTransform3D translateTransform3D = new TranslateTransform3D()
            {
                OffsetX = oldBallX - SpawnPoint.X,
                OffsetY = oldBallY - SpawnPoint.Y,
                OffsetZ = oldBallZ - SpawnPoint.Z
            };

            //<<<--- Create & apply translation animation --->>>
            DoubleAnimation translateXAnimation = new DoubleAnimation(Ball.X, new Duration(TimeSpan.FromSeconds(20000)));
            DoubleAnimation translateYAnimation = new DoubleAnimation(Ball.Y, new Duration(TimeSpan.FromSeconds(20000)));
            DoubleAnimation translateZAnimation = new DoubleAnimation(Ball.Z, new Duration(TimeSpan.FromSeconds(20000)));

            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, translateXAnimation);
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetYProperty, translateYAnimation);
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, translateZAnimation);

            //<<<--- Create and apply transformation groups --->>>
            Transform3DGroup mazeTransformationGroup = new Transform3DGroup();
            mazeTransformationGroup.Children.Add(pitchRotate);
            mazeTransformationGroup.Children.Add(rollRotate);

            Transform3DGroup ballTransformationGroup = new Transform3DGroup();
            ballTransformationGroup.Children.Add(translateTransform3D);
            ballTransformationGroup.Children.Add(pitchRotate);
            ballTransformationGroup.Children.Add(rollRotate);

            MazeModelVisual.Transform = mazeTransformationGroup;
            BallModelVisual.Transform = ballTransformationGroup;
        }
    }
}
