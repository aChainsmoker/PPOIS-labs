namespace DFS_and_AStar
{
    internal class Reader
    {
        public void ReadVertexes(Graph graph, string filePath)
        {
            string[] vertexesStrings = null;
            try
            {
                string[] testStrings = File.ReadAllLines(filePath);
                vertexesStrings = testStrings;
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException("The file could not be found");
            }

            for (int i = 0; i < vertexesStrings.Length; i++)
            {
                string[] vertexData = vertexesStrings[i].Split();
                Node newNode = new Node(vertexData[0]);
                graph.Nodes.Add(newNode);
            }

            for (int i = 0; i < vertexesStrings.Length; i++)
            {
                string[] vertexData = vertexesStrings[i].Split();

                for (int j = 1; j < vertexData.Length; j++)
                {
                    string[] nodeEdgeCostPair = vertexData[j].Split(':');
                    KeyValuePair<Node, int> newNodeEdgeCostPair =
                        new KeyValuePair<Node, int>(graph.Nodes.First(x => x.Name == nodeEdgeCostPair[0]),
                            int.Parse(nodeEdgeCostPair[1]));
                    graph.Nodes[i].AdjacentNodes.Add(newNodeEdgeCostPair);
                }
            }
            
        }

        public Dictionary<string, int> ReadHeuristics(Graph graph, string filePath)
        {
            Dictionary<string, int> heuristics = new Dictionary<string, int>();

            try
            {
                string[] heuristicsStrings = File.ReadAllLines(filePath);
                for (int i = 0; i < heuristicsStrings.Length; i++)
                {
                    string[] heuristicData = heuristicsStrings[i].Split();
                    heuristics.Add(heuristicData[0], int.Parse(heuristicData[1]));
                }
            }
            catch (Exception e)
            {
                throw new Exception("The file could not be found or contains not appropriate data");
            }
            
            return heuristics;
        }
        
    }
}