using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    private GameObject[] nodes;

    public List<Transform> FindShortestPath(Transform start, Transform end)
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");

        List<Transform> result = new List<Transform>();
        Transform node = AStarAlgo(start, end);

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
            if(current == end)
            {   
                return end;
            }

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {
                    // Get the distance of the object.
                    float distance = Vector2.Distance(neighNode.position, current.position);
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
            
            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.GetNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.IsWalkable())
                {
                    // Get the distance of the object.
                    float distance = Vector2.Distance(neighNode.position, current.position) + Vector2.Distance(neighNode.position, end.position);
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
}
