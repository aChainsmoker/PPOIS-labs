using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFS_and_AStar
{
    internal class Graph
    {
        private List<Node> nodes;
        private Reader vertexReader;

        public List<Node> Nodes { get => nodes; set => nodes = value; }

        public Graph()
        {
            nodes = new List<Node>();  
            vertexReader = new Reader();    
        }

        public void AssignTheHeuristicUsingDFS(Node startNode, Dictionary<string, int> heuristics)
        {
            if(startNode.VisitedByDFS == true)
                return;
            
            startNode.VisitedByDFS = true;
            startNode.Heuristic = heuristics[startNode.Name];
            
            for (int i = 0; i < startNode.AdjacentNodes.Count; i++)
            {
                AssignTheHeuristicUsingDFS(startNode.AdjacentNodes[i].Key, heuristics);
            }
        }

        public List<string> FindShortestWay(Node startNode, Node endNode)
        {
            endNode.Heuristic = 0;
            PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();
            priorityQueue.Enqueue(startNode, startNode.Heuristic); //Временное решение
            Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
            Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();
            came_from.Add(startNode, null);
            cost_so_far.Add(startNode, 0);

            while (priorityQueue.Count > 0)
            {
                Node currentNode = priorityQueue.Dequeue();

                if (currentNode == endNode)
                {
                    return FormPath(came_from, endNode);
                }

                foreach (var nextNode in currentNode.AdjacentNodes)
                {
                    int newCost = cost_so_far[currentNode] + nextNode.Value;
                    if (cost_so_far.ContainsKey(nextNode.Key) == false || newCost < cost_so_far[nextNode.Key])
                    {
                        cost_so_far[nextNode.Key] = newCost;
                        int priority = newCost + nextNode.Key.Heuristic;
                        priorityQueue.Enqueue(nextNode.Key, priority);
                        came_from[nextNode.Key] = currentNode;
                    }
                }
            }

            return null;
        }


        public List<string> FormPath(Dictionary<Node, Node> came_from, Node endNode)
        {
            List<string> path = new List<string>();
            Node previousNode = endNode;
            while (previousNode != null)
            {
                path.Insert(0, previousNode.Name);
                previousNode = came_from[previousNode];
            }
            return path;
        }
        public void ReadVertexes(string filePath)
        {
            vertexReader.ReadVertexes(this, filePath);
        }

        public Dictionary<string, int> GetHeuristics(string filePath)
        {
            return vertexReader.ReadHeuristics(this, filePath);
        }
    }
}
