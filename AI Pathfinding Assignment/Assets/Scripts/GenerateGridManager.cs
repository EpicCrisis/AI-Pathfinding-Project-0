using System.Collections.Generic;
using UnityEngine;

public class GenerateGridManager : MonoBehaviour
{

    public int row = 5;
    public int column = 5;
    public float padding = 3f;
    public Transform nodePrefab;
    [SerializeField] private bool enableDiagonal = false;

    public List<Transform> grid = new List<Transform>();

    public static GenerateGridManager instance;

    void Start()
    {
        GridInit();
    }

    public void SetDiagonal(bool value)
    {
        this.enableDiagonal = value;
        this.GenerateNeighbours();
    }

    private void GridInit()
    {
        this.GenerateGrid();
        this.GenerateNeighbours();
    }

    private void GenerateGrid()
    {
        int counter = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Transform node = Instantiate(nodePrefab, new Vector2(j * padding, i * padding), Quaternion.identity);
                node.transform.parent = gameObject.transform;
                node.name = "node (" + counter + ")";
                grid.Add(node);
                counter++;
            }
        }
    }

    private void GenerateNeighbours()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            Node currentNode = grid[i].GetComponent<Node>();
            currentNode.GetNeighbourNode().Clear();
            int index = i + 1;

            // For those on the left, with no left neighbours
            if (index % column == 1)
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);         // North node
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);         // South node
                }

                if (enableDiagonal)
                {
                    if (i + column + 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column + 1]);     // North-East node
                    }
                    if (i - column + 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column + 1]);     // South-East node
                    }
                }

                currentNode.AddNeighbourNode(grid[i + 1]);                  // East node
            }
            // For those on the right, with no right neighbours
            else if (index % column == 0)
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);         // North node
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);         // South node
                }

                if (enableDiagonal)
                {
                    if (i + column - 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column - 1]);     // North-West node
                    }
                    if (i - column - 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column - 1]);     // South-West node
                    }
                }

                currentNode.AddNeighbourNode(grid[i - 1]);                  // West node
            }
            else
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.AddNeighbourNode(grid[i + column]);         // North node
                }
                if (i - column >= 0)
                {
                    currentNode.AddNeighbourNode(grid[i - column]);         // South node
                }

                if (enableDiagonal)
                {
                    if (i + column + 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column + 1]);     // North-East node
                    }
                    if (i + column - 1 < column * row)
                    {
                        currentNode.AddNeighbourNode(grid[i + column - 1]);     // North-West node
                    }
                    if (i - column + 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column + 1]);     // South-East node
                    }
                    if (i - column - 1 >= 0)
                    {
                        currentNode.AddNeighbourNode(grid[i - column - 1]);     // South-West node
                    }
                }

                currentNode.AddNeighbourNode(grid[i + 1]);                  // East node
                currentNode.AddNeighbourNode(grid[i - 1]);                  // West node
            }
        }
    }
}
