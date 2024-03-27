using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Models
{
    public class AddiAlgoMazeFactroy : IMazeFactory
    {

        private readonly int MazeGridLenght = MazeProperties.MazeGridLength  ;
        private readonly int MazeGridWith = MazeProperties.MazeGridWidth ;

        private Random random = new Random();

        private Point MazeStartEdge;
        public IMaze Maze2d => new Maze(CreateMazeGraph(), MazeStartEdge);
        public IMaze3d Maze3d => new Maze3d(CreateMazeGraph(), MazeStartEdge);

        public AddiAlgoMazeFactroy(Point MazeStartEdge)
        {
            this.MazeStartEdge = MazeStartEdge;
        }

        private List<Node> CreateMazeGraph()
        {
            List<Node> UnexploredNodes = new List<Node>();
            List<Node> ExploredNodes = new List<Node>();

            //Create all nodes
            for (int x = 0; x < MazeGridWith; x++)
            {
                for (int y = 0; y < MazeGridLenght; y++)
                {
                    UnexploredNodes.Add(new Node(new Point(x, y)));
                }
            }

            //Link every node with each other
            for (int i = 0; i < UnexploredNodes.Count; i++)
            {
                Node node = UnexploredNodes[i];
                node.Edges = GetPossibleNeighbourNodes(node);
                UnexploredNodes[i] = node;
            }

            //Get beginning node
            int doorXLocation = MazeGridWith / 2;
            Node BeginningNode = UnexploredNodes.Where(node => node.Location.X == doorXLocation && node.Location.Y == 0).First();
            UnexploredNodes.Remove(BeginningNode);

            return RecursiveGraphGenerator(UnexploredNodes, new List<Node> { BeginningNode }, new List<Node> { });
        }

        private List<Node> RecursiveGraphGenerator(List<Node> unExploredNodes, List<Node> toExploreNodes, List<Node> exploredNodes)
        {
            //---Pop a random node from toExploreNodes and use it as the currentNode---
            Node currentNode = toExploreNodes[random.Next(0, toExploreNodes.Count)];
            toExploreNodes.Remove(currentNode);

            //<<<---- Remove edge with neighbours that are status toExplored --->>>
            List<Node> toExploreNeighbourNodes = toExploreNodes
            .Where(toExploredNode => currentNode.Edges.Any(neighbour => neighbour.Location == toExploredNode.Location))
            .ToList();

            foreach (Node node in toExploreNeighbourNodes)
            {
                RemoveEdge(node, currentNode);
            }

            //<<<---- Promote all unExplored neighbours to toExplored ---->>>
            List<Node> unexploredNeighbourNodes = unExploredNodes
                .Where(unExploredNode => currentNode.Edges.Any(neighbour => neighbour.Location == unExploredNode.Location))
                .ToList();

            foreach(Node node in unexploredNeighbourNodes) 
            {
                unExploredNodes = unExploredNodes.Where(unExploredNode => unExploredNode.Location != node.Location).ToList();
            }
            toExploreNodes.AddRange(unexploredNeighbourNodes);

            //<<<---- Promote currentNode to exploredNodes ---->>>
            exploredNodes.Add(currentNode);
            if (unExploredNodes.Count > 0 || toExploreNodes.Count > 0)
            {
                return RecursiveGraphGenerator(unExploredNodes, toExploreNodes, exploredNodes);
            }
            else
            {
                //If all nodes are explored then end the programm
                return exploredNodes;
            }
        }

        private void RemoveEdge(Node node1, Node node2)
        {
            node1.Edges = node1.Edges.Where(node => node.Location != node2.Location).ToList();
            node2.Edges = node2.Edges.Where(node => node.Location != node1.Location).ToList();
        }

        private List<Node> GetPossibleNeighbourNodes(Node node)
        {
            List<Node> possibleNodes = new List<Node>
            {
                new Node(new Point(node.Location.X, node.Location.Y + 1)),  //Above node
                new Node(new Point(node.Location.X - 1, node.Location.Y)),  //Left node
                new Node(new Point(node.Location.X, node.Location.Y - 1)),  //Below node
                new Node(new Point(node.Location.X + 1, node.Location.Y))   //Right node
            };

            possibleNodes = possibleNodes.Where(node => NodeIsInMaze(node)).ToList();
            return possibleNodes;
        }

        private bool NodeIsInMaze(Node node)
        {
            if (node.Location.X < 0 || node.Location.X >= MazeGridWith) return false;
            if (node.Location.Y < 0 || node.Location.Y >= MazeGridLenght) return false;
            return true;
        }
    }
}

