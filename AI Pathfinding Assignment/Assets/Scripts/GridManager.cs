using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Stop
    }
    
    public int gridSizeX;
    public int gridSizeY;

    public int minGridSizeX;
    public int minGridSizeY;
    public int maxGridSizeX;
    public int maxGridSizeY;

    public Node nodePrefab;
    public Node[,] nodes;

    public bool isPlaying = false;

    public float updateInterval = 0.1f;
    float counter;

    public int generation;
    public Text genText;

    // Singleton
    public static GridManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateGrid(gridSizeX, gridSizeY);
    }

    public void CreateGrid(int newGridSizeX, int newGridSizeY)
    {
        gridSizeX = newGridSizeX;
        gridSizeY = newGridSizeY;

        nodes = new Node[gridSizeX, gridSizeY];

        for (int i = 0; i < gridSizeY; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                Node n = Instantiate(nodePrefab, new Vector2(i, j), Quaternion.identity);

                n.InitNode(i, j);

                nodes[i, j] = n;
            }
        }
    }

    public void ResetNodes()
    {
        // Checks if there are cells in the grid or not.
        if (nodes == null) return;

        generation = 0;
        genText.text = generation.ToString("000");

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                nodes[i, j].ClearNode();
            }
        }
    }

    public void UpdateNodes()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                nodes[i, j].NodeUpdate();
            }
        }
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                nodes[i, j].ApplyNodeUpdate();
            }
        }

        generation++;
        genText.text = generation.ToString("000");
    }

    public void RemoveGrid()
    {

    }

    public void Run()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            counter = 0.0f;
        }
    }

    public void Stop()
    {
        if (isPlaying)
        {
            isPlaying = false;
            counter = 0.0f;
        }
    }
}
