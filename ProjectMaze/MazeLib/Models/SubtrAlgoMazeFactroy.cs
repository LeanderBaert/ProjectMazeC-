using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Models
{
    public class SubtrAlgoMazeFactroy : IMazeFactory
    {
        private readonly int MazeGridLenght = MazeProperties.MazeGridLength  ;
        private readonly int MazeGridWith = MazeProperties.MazeGridWidth ;

        private Node EndingNode;
        private Node BeginningNode;
        private Random random = new Random();

        private Point MazeStartEdge;
        public IMaze Maze2d => new Maze(CreateMazeGraph(), MazeStartEdge);
        public IMaze3d Maze3d => new Maze3d(CreateMazeGraph(), MazeStartEdge);

        public SubtrAlgoMazeFactroy(Point MazeStartEdge)
        {
            this.MazeStartEdge = MazeStartEdge;
        }

        private List<Node> CreateMazeGraph()
        {
            List<Node> UnexploredNodes = new List<Node>();
            List<Node> ExploredNodes = new List<Node>();

            for (int x = 0; x < MazeGridWith; x++)
            {
                for (int y = 0; y < MazeGridLenght; y++)
                {
                    UnexploredNodes.Add(new Node(new Point(x, y)));
                }
            }
            int doorXLocation = MazeGridWith / 2;

            BeginningNode = UnexploredNodes.Where(node => node.Location.X == doorXLocation && node.Location.Y == 0).First();
            EndingNode = UnexploredNodes.Where(node => node.Location.X == doorXLocation && node.Location.Y == MazeGridLenght - 1).First();
            //UnexploredNodes.RemoveAll(node => node.Location.X == MazeGridLenght && node.Location.Y == 0);

            return RecursiveGraphGenerator(UnexploredNodes, ExploredNodes, BeginningNode);
        }

        private List<Node> RecursiveGraphGenerator(List<Node> UnexploredNodes, List<Node> ExploredNodes, Node currentNode)
        {
            List<Node> neighbourNodes = GetPossibleEdgeNodes(currentNode, ExploredNodes);

            //---Null neighbourNodes find new current node--- 
            if ((neighbourNodes.Count == 0 ) && UnexploredNodes.Count != 0) 
            {
                Node randomExploredNode = SelectGoodRandomNode(ExploredNodes);
                return RecursiveGraphGenerator(UnexploredNodes, ExploredNodes, randomExploredNode);

            }
            //---Set radom neighbour as current--- 
            else if(UnexploredNodes.Count != 0)
            {
                Node randomNeighbourNode = neighbourNodes[random.Next(0, neighbourNodes.Count)];

                //---If the node isnt explored yet set to explored---
                if (UnexploredNodes.Where(node => node.Location == randomNeighbourNode.Location).ToList().Count() > 0)
                {
                    ExploredNodes.Add(randomNeighbourNode);
                    UnexploredNodes = UnexploredNodes.Where(node => node.Location != randomNeighbourNode.Location).ToList();
                    //---Add conection---
                    AddEdge(currentNode, randomNeighbourNode);

                    //---If exit is found select random explored next neighbour---
                    if (randomNeighbourNode.Location == EndingNode.Location)
                    {
                        Node randomExploredNode = SelectGoodRandomNode(ExploredNodes);
                        return RecursiveGraphGenerator(UnexploredNodes, ExploredNodes, randomExploredNode);
                    }
                }
                
                return RecursiveGraphGenerator(UnexploredNodes, ExploredNodes, randomNeighbourNode);
            }
            return ExploredNodes;
        }

        private List<Node> GetPossibleEdgeNodes(Node node, List<Node> ExploredNodes)
        {
            List<Node> possibleNodes = new List<Node>
            {
                new Node(new Point(node.Location.X, node.Location.Y + 1)),  //Above node
                new Node(new Point(node.Location.X - 1, node.Location.Y)),  //Left node
                new Node(new Point(node.Location.X, node.Location.Y - 1)),  //Below node
                new Node(new Point(node.Location.X + 1, node.Location.Y))   //Right node
            };

            possibleNodes = possibleNodes.Where(node => NodeIsInMaze(node)).ToList();
            possibleNodes = possibleNodes.Where(node1 => !ExploredNodes.Any(node2 => node2.Location == node1.Location)).ToList();
            return possibleNodes;
        }

        private Node SelectGoodRandomNode(List<Node> nodes) 
        {
            Node randomNode = nodes[random.Next(0, nodes.Count)];
            while(randomNode.Location == BeginningNode.Location || randomNode.Location == EndingNode.Location)
            {
                randomNode = nodes[random.Next(0, nodes.Count)];
            }
            return randomNode;
        }

        private bool NodeIsInMaze(Node node)
        {
            if (node.Location.X < 0 || node.Location.X >= MazeGridWith) return false;
            if (node.Location.Y < 0 || node.Location.Y >= MazeGridLenght) return false;
            return true;
        }

        private void AddEdge(Node node1, Node node2)
        {
            node1.Edges.Add(node2);
            node2.Edges.Add(node1);
        }
    }
}
