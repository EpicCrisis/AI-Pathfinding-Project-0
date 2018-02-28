using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private Transform node;
    private Transform startNode;
    private Transform endNode;
    private List<Transform> blockPath = new List<Transform>();
    
    void Update()
    {
        MouseInput();
        // Update colors.
        this.ColorBlockPath();
        this.UpdateNodeColor();
    }

    private void MouseInput()
    {
        if (Input.GetButton("Fire1"))
        {
            // Get the raycast from the mouse position from screen.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
            {
                //unmark previous
                SpriteRenderer sRend;
                if (node != null)
                {
                    sRend = node.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.white;
                }

                // We now update the selected node.
                node = hit.transform;

                // Mark it
                sRend = node.GetComponent<SpriteRenderer>();
                sRend.material.color = Color.green;
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
            {
                node = hit.transform;

                if (node != null)
                {
                    SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
                    Node n = node.GetComponent<Node>();

                    if (n.IsWalkable())
                    {
                        // Set selected node to not walkable
                        n.SetWalkable(false);

                        // Add the node to the block path list.
                        blockPath.Add(node);

                        // Render the selected node to black.
                        sRend.material.color = Color.black;

                        // If the block path is start node, we remove start node.
                        if (node == startNode)
                        {
                            startNode = null;
                        }

                        // If the block path is end node, we remove end node.
                        if (node == endNode)
                        {
                            endNode = null;
                        }

                        node = null;
                    }
                    else if (!n.IsWalkable())
                    {
                        // Set selected node to not walkable
                        n.SetWalkable(true);

                        // Add the node to the block path list.
                        blockPath.Remove(node);

                        // Render the selected node to black.
                        sRend.material.color = Color.white;

                        node = null;
                    }
                }
            }
        }
    }

    public void BtnStartNode()
    {
        if (node != null)
        {
            Node n = node.GetComponent<Node>();

            // Making sure only walkable node are able to set as start.
            if (n.IsWalkable())
            {
                // If this is a new start node, we will just set it to blue.
                if (startNode == null)
                {
                    SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.blue;
                }
                else
                {
                    // Reverse the color of the previous node
                    SpriteRenderer sRend = startNode.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.white;

                    // Set the new node as blue.
                    sRend = node.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.blue;
                }

                startNode = node;
                node = null;
            }
        }
    }

    public void BtnEndNode()
    {
        if (node != null)
        {
            Node n = node.GetComponent<Node>();

            // Making sure only walkable node are able to set as end.
            if (n.IsWalkable())
            {
                // If this is a new end node, we will just set it to cyan.
                if (endNode == null && n != startNode)
                {
                    SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.cyan;
                }
                else
                {
                    // Reverse the color of the previous node
                    SpriteRenderer sRend = endNode.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.white;

                    // Set the new node as cyan.
                    sRend = node.GetComponent<SpriteRenderer>();
                    sRend.material.color = Color.cyan;
                }

                endNode = node;
                node = null;
            }
        }
    }

    public void BtnFindPath()
    {
        // Only find if there are start and end node.
        if (startNode != null && endNode != null)
        {
            // Execute Shortest Path.
            ShortestPath finder = gameObject.GetComponent<ShortestPath>();
            List<Transform> paths = finder.findShortestPath(startNode, endNode);

            // Colour the node red.
            foreach (Transform path in paths)
            {
                SpriteRenderer sRend = path.GetComponent<SpriteRenderer>();
                sRend.material.color = Color.red;
            }
        }
    }

    public void BtnBlockPath()
    {
        if (node != null)
        {
            // Render the selected node to black.
            SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.black;

            // Set selected node to not walkable
            Node n = node.GetComponent<Node>();
            n.SetWalkable(false);

            // Add the node to the block path list.
            blockPath.Add(node);

            // If the block path is start node, we remove start node.
            if (node == startNode)
            {
                startNode = null;
            }

            // If the block path is end node, we remove end node.
            if (node == endNode)
            {
                endNode = null;
            }

            node = null;
        }
    }

    public void BtnUnblockPath()
    {
        if (node != null)
        {
            // Set selected node to white.
            SpriteRenderer sRend = node.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;

            // Set selected not to walkable.
            Node n = node.GetComponent<Node>();
            n.SetWalkable(true);

            // Remove selected node from the block path list.
            blockPath.Remove(node);

            node = null;
        }
    }

    public void BtnClearBlock()
    {
        // For each blocked path in the list
        foreach (Transform path in blockPath)
        {
            // Set walkable to true.
            Node n = path.GetComponent<Node>();
            n.SetWalkable(true);

            // Set their color to white.
            SpriteRenderer sRend = path.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;

        }
        // Clear the block path list and 
        blockPath.Clear();
    }

    public void BtnRestart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    private void ColorBlockPath()
    {
        foreach (Transform block in blockPath)
        {
            SpriteRenderer sRend = block.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.black;
        }
    }

    private void UpdateNodeColor()
    {
        if (startNode != null)
        {
            SpriteRenderer sRend = startNode.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.blue;
        }

        if (endNode != null)
        {
            SpriteRenderer sRend = endNode.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.cyan;
        }
    }
}
