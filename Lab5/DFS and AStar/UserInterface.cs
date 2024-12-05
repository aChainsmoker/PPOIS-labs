namespace DFS_and_AStar;

internal class UserInterface
{
    public void Run()
    {
        Node startNode, endNode;
        Console.WriteLine("Enter the path to vertexes data");
        string vertexesPath = Console.ReadLine();
        Console.WriteLine("Enter the path to heuristics data");
        string heuristicsPath = Console.ReadLine();
        
        Graph graph = new Graph();
        graph.ReadVertexes(vertexesPath);
        graph.AssignTheHeuristicUsingDFS(graph.Nodes[0], graph.GetHeuristics(heuristicsPath));
        
        Console.WriteLine("Enter the name of the start node");
        string startNodeName = Console.ReadLine();
        try {startNode = graph.Nodes.First(x => x.Name == startNodeName); } catch { throw new Exception("Graph does not contain such Node"); }
        Console.WriteLine("Enter the name of the end node");
        string endNodeName = Console.ReadLine();
        try {endNode = graph.Nodes.First(x => x.Name == endNodeName); } catch { throw new Exception("Graph does not contain such Node"); }

        DisplayResult(graph.FindShortestWay(startNode, endNode));
    }

    public void DisplayResult(List<string> result)
    {

        foreach (var node in result)
        {
            if(result.IndexOf(node) != result.Count - 1)
                Console.Write($"{node}->");
            else
                Console.Write(node);
        }
    }
}