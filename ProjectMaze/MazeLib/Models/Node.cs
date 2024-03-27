using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib.Models
{
    public class Node
    {
        public Point Location { get; set; }

        public List<Node> Edges { get; set; } = new List<Node>();

        public Node(Point Location, List<Node> Edges)
        {
            this.Location = Location;
            this.Edges = Edges;
        }

        public Node(Point Location)
        {
            this.Location = Location;
        }
    }
}
