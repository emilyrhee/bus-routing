using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class RoadNetwork
{
    public List<Godot.Collections.Array> AdjacencyMatrix { get; private set; }

    /// <summary>
    /// List of all road nodes in the network.
    /// </summary>
    public List<RoadNode> RoadNodes { get; private set; }
    private Dictionary<RoadNode, int> _roadNodeIndexMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoadNetwork"/> class.
    /// </summary>
    /// <param name="roadNodeContainer">The container node that holds all road nodes in the level.</param>
    public RoadNetwork(Node roadNodeContainer)
    {
        RoadNodes = roadNodeContainer.GetChildren().OfType<RoadNode>().ToList();
        _roadNodeIndexMap = new Dictionary<RoadNode, int>();
        for (int i = 0; i < RoadNodes.Count; i++)
        {
            _roadNodeIndexMap[RoadNodes[i]] = i;
        }

        AdjacencyMatrix = new List<Godot.Collections.Array>(RoadNodes.Count);
        for (int i = 0; i < RoadNodes.Count; i++)
        {
            var row = new Godot.Collections.Array();
            row.Resize(RoadNodes.Count);
            for (int j = 0; j < RoadNodes.Count; j++)
            {
                row[j] = 0;
            }
            AdjacencyMatrix.Add(row);
        }

        for (int i = 0; i < RoadNodes.Count; i++)
        {
            var roadNode = RoadNodes[i];
            foreach (RoadNode neighbor in roadNode.Neighbors)
            {
                if (_roadNodeIndexMap.TryGetValue(neighbor, out int neighborIndex))
                {
                    AdjacencyMatrix[i][neighborIndex] = 1;
                }
            }
        }
    }

    /// <summary>
    /// Prints a nice looking adjacency matrix to the Godot console.
    /// </summary>
    public void PrintAdjacencyMatrix()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("--- Adjacency Matrix ---");

        var nodeNames = RoadNodes.Select(n => n.Name.ToString()).ToList();

        sb.Append("      "); // Padding for alignment
        sb.AppendLine(string.Join(" ", nodeNames));

        for (int i = 0; i < AdjacencyMatrix.Count; i++)
        {
            var row = AdjacencyMatrix[i];
            sb.Append(nodeNames[i].PadRight(6));

            var rowValues = new List<string>();
            for (int j = 0; j < row.Count; j++)
            {
                rowValues.Add(row[j].ToString().PadLeft(nodeNames[j].Length, ' '));
            }
            sb.AppendLine(string.Join(" ", rowValues));
        }

        GD.Print(sb.ToString());
    }
}