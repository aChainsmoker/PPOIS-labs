using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFS_and_AStar
{
    internal class Node
    {
        private List<KeyValuePair<Node, int>> adjacentNodes;
        private string name;
        private int heuristic;
        private bool visitedByDFS;

        public List<KeyValuePair<Node, int>> AdjacentNodes { get => adjacentNodes; set => adjacentNodes = value; }
        public int Heuristic { get => heuristic; set => heuristic = value; }
        public bool VisitedByDFS { get => visitedByDFS; set => visitedByDFS = value; }
        public string Name { get => name; set => name = value; }    
        public Node(string name)
        {
            this.name = name;
            adjacentNodes = new List<KeyValuePair<Node, int>>();
        }
    }
}
