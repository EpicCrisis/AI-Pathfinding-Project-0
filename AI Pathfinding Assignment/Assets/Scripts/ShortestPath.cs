using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    private GameObject[] nodes;

    [SerializeField] private bool startExploreCount = false;
    [SerializeField] private bool startPathCount = false;

    [SerializeField] private float counter;

    [SerializeField] private float exploreInterval = 0.2f;
    [SerializeField] private float pathInterval = 0.2f;

    [SerializeField] private int exploreCounter;
    [SerializeField] private int resultCounter;

    [SerializeField] private List<Transform> explored;
    [SerializeField] private List<Transform> result;

    [SerializeField] private float weight = 1.0f;
    
    public enum AlgoType
    {
        Dijkstra = 0,
        AStar = 1
    }

    public AlgoType algorithm;

    public void SetWeight(float value)
    {
        this.weight = value;
    }

    //Reset the animation values on button press.
    public void ResetAnimation()
    {
        this.startExploreCount = false;
        this.startPathCount = false;

        this.explored.Clear();
        this.result.Clear();

        this.counter = 0.0f;

        this.exploreCounter = 0;
        this.resultCounter = 0;
    }

    public void StartExploreCount(bool value)
    {
        this.startExploreCount = value;
    }

    public void StartPathCount(bool value)
    {
        this.startPathCount = value;
    }

    private void Update()
    {
        if (startExploreCount)
        {
            PlayExplorePath();
        }
        if (startPathCount)
        {
            PlayWalkPath();
        }
    }

    private void PlayExplorePath()
    {
        counter += Time.deltaTime;
        if (counter > exploreInterval && exploreCounter < explored.Count)
        {
            counter = 0.0f;

            SpriteRenderer sRend = explored[exploreCounter].GetComponent<SpriteRenderer>();
            sRend.material.color = Color.yellow;

            ++exploreCounter;
        }
        else if (counter > exploreInterval && exploreCounter == explored.Count)
        {
            counter = 0.0f;
            exploreCounter = 0;

            explored.Clear();

            startExploreCount = false;
            startPathCount = true;
        }
    }

    private void PlayWalkPath()
    {
        counter += Time.deltaTime;
        if (counter > pathInterval && resultCounter < result.Count)
        {
            counter = 0.0f;

            SpriteRenderer sRend = result[resultCounter].GetComponent<SpriteRenderer>();
            sRend.material.color = Color.red;

            ++resultCounter;
        }
        else if (counter > pathInterval && resultCounter == result.Count)
        {
            counter = 0.0f;
            resultCounter = 0;

            result.Clear();

            startPathCount = false;
        }
    }

    public List<Transform> FindShortestPath(Transform start, Transform end)
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");

        result = new List<Transform>();
        Transform node = null;

        if (algorithm == AlgoType.Dijkstra)
        {
            node = DijkstrasAlgo(start, end);
        }
        else if (algorithm == AlgoType.AStar)
        {
            node = AStarAlgo(start, end);
        }

        // While there's still previous node, we will continue.
        while (node != null)
        {
            result.Add(node);
            Node currentNode = node.GetComponent<Node>();
            node = currentNode.GetParentNode();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    private Transform DijkstrasAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

        // Nodes that are unexplored
        List<Transform> unexplored = new List<Transform>();

        // We add all the nodes we found into unexplored.
        foreach (GameObject obj in nodes)
        {
            Node n = obj.GetComponent<Node>();
            if (n.IsWalkable())
            {
                n.ResetNode();
                unexplored.Add(obj.transform);
            }
        }

        // Set the starting node weight to 0;
        Node startNode = start.GetComponent<Node>();
        startNode.SetWeight(0);

        // Set the ending node weight to 0;
        //Node endNode = start.GetComponent<Node>();
        //endNode.SetWeight(0);

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Node>().GetWeight().CompareTo(y.GetComponent<Node>().GetWeight()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            // Note: This is used for games, as we just want to reduce computation, better way will be implementing A*

            // If we reach the end node, we will stop.
            if (current == end)
            {
                return end;
            }

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            //Add current node to allow looping of animation
            explored.Add(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                counter = 0;
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {
                    // Get the distance of the object.
                    float distance = CalculateDistance(neighNode.position, current.position) * weight;
                    distance = currentNode.GetWeight() + distance;

                    // If the added distance is less than the current weight.
                    if (distance < node.GetWeight())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.SetWeight(distance);
                        node.SetParentNode(current);
                    }
                }
            }
        }

        double endTime = (Time.realtimeSinceStartup - startTime);
        print("Compute time: " + endTime);

        print("Path completed!");

        return end;
    }

    private Transform AStarAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

        // Nodes that are unexplored
        List<Transform> unexplored = new List<Transform>();

        // We add all the nodes we found into unexplored.
        foreach (GameObject obj in nodes)
        {
            Node n = obj.GetComponent<Node>();
            if (n.IsWalkable())
            {
                n.ResetNode();
                unexplored.Add(obj.transform);
            }
        }

        // Set the starting node weight to 0;
        Node startNode = start.GetComponent<Node>();
        startNode.SetWeight(0);

        // Set the ending node weight to 0;
        //Node endNode = end.GetComponent<Node>();
        //endNode.SetWeight(0);

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Node>().GetWeight().CompareTo(y.GetComponent<Node>().GetWeight()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            // Note: This is used for games, as we just want to reduce computation, better way will be implementing A*
            // If we reach the end node, we will stop.
            if (current == end)
            {
                return end;
            }
            
            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            //Add current node to allow looping of animation
            explored.Add(current);
            
            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {
                    // Get the distance of the object.
                    float distance = CalculateDistance(current.position, neighNode.position);
                    float hCost = CalculateDistance(neighNode.position, end.position) * weight;
                    float gCost = CalculateDistance(neighNode.position, start.position) * weight;
                    float fCost = hCost + gCost;

                    // Set the calculated heuristics for the node.
                    node.SetHCost(hCost);
                    node.SetGCost(gCost);

                    distance = currentNode.GetWeight() + node.GetFCost + distance;
                    
                    // If the added distance is less than the current weight.
                    if (distance < node.GetWeight())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.SetWeight(hCost);
                        node.SetParentNode(current);
                    }
                }
            }
        }

        double endTime = (Time.realtimeSinceStartup - startTime);
        print("Compute time: " + endTime);

        print("Path completed!");

        return end;
    }
    
    public float CalculateDistance(Vector2 originNode, Vector2 targetNode)
    {
        GenerateGridManager grid = GetComponent<GenerateGridManager>();
        float h;

        if (!grid.GetDiagonal)
        {
            h = Mathf.Abs(originNode.x - targetNode.x) + Mathf.Abs(originNode.y - targetNode.y);
        }
        else
        {
            h = Mathf.Max(Mathf.Abs(originNode.x - targetNode.x),Mathf.Abs(originNode.y - targetNode.y));
        }
        return h;
    }
}
