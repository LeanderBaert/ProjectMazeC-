using MazeLib.Models;
using Maze2d.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;
using DrawRectangle = System.Windows.Shapes.Rectangle;

namespace Maze2d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMazeFactory MazeFactory;
        private IMaze Maze;
        private IPlayer Player = new Player();

        private bool up, down, left, right = false;

        private Dictionary<string, IMazeFactory> AllFactories;

        public MainWindow()
        {
            InitializeComponent();

            //--- Configure the combobox---
            AllFactories = new Dictionary<string, IMazeFactory>() { { "Static maze", new StaticMazeFactory(MazeProperties.MazeStartEdge) }, { "Add walls generator", new AddiAlgoMazeFactroy(MazeProperties.MazeStartEdge) }, { "Delete walls generator", new SubtrAlgoMazeFactroy(MazeProperties.MazeStartEdge) } };
            FactorySelecter.DataContext = AllFactories.Keys.ToList();
            FactorySelecter.SelectedIndex = 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z || e.Key == Key.Up)
            {
                up = true;
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                down = true;
            }
            else if (e.Key == Key.Q || e.Key == Key.Left)
            {
                left = true;
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                right = true;
            }
            Player.movementInput(up, down, left, right, Maze.MazeWalls);
            RenderScene();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z || e.Key == Key.Up)
            {
                up = false;
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                down = false;
            }
            else if (e.Key == Key.Q || e.Key == Key.Left)
            {
                left = false;
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                right = false;
            }
            Player.movementInput(up, down, left, right, Maze.MazeWalls);
            RenderScene();
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = e.AddedItems[0].ToString();
            MazeFactory = AllFactories[selectedItem];

            Maze = MazeFactory.Maze2d;

            //FactorySelecter.DataContext = 

            RenderScene();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {

            Maze = MazeFactory.Maze2d;
            RenderScene();
        }

        private void RenderScene()
        {
            renderCanvas.Children.Clear();
            RenderPlayer();
            DrawMaze();
        }

        private void RenderPlayer()
        {
            Ellipse playerModel = Player.PlayerModel;

            int calibratedX = (int)(Player.Location.X - Player.PlayerModel.Width / 2);
            int calibratedY = (int)(Player.Location.Y - Player.PlayerModel.Width / 2);
            Canvas.SetLeft(playerModel, calibratedX);
            Canvas.SetTop(playerModel, calibratedY);
            renderCanvas.Children.Add(playerModel);
        }

        private void DrawMaze()
        {

            foreach (Rectangle rectangle in Maze.MazeWalls)
            {
                DrawRectangle newRectangle = new DrawRectangle
                {
                    Width = rectangle.Width,
                    Height = rectangle.Height,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(newRectangle, rectangle.X);
                Canvas.SetTop(newRectangle, rectangle.Y);

                newRectangle.Stroke = Brushes.Black;
                newRectangle.Fill = Brushes.Black;
                renderCanvas.Children.Add(newRectangle);
            }

        }
    }
}
