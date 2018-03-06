using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform node;
    [SerializeField] private Transform startNode;
    [SerializeField] private Transform endNode;
    [SerializeField] private List<Transform> blockPath = new List<Transform>();
    [SerializeField] private List<Transform> selectedNode = new List<Transform>();

    [Header("SelectionPrefab")]
    [SerializeField] private Transform selection;
    
    void Update()
    {
        MouseInput();
        // Update colors.
        this.ColorBlockPath();
        this.UpdateNodeColor();
    }

    private void MouseInput()
    {
        if (!UIHoverListener.isUIOverride)
        {
            if (Input.GetButton("Fire1"))
            {
                // Get the raycast from the mouse position from screen.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
                {
                    // We now update the selected node.
                    node = hit.transform;
                    Node n = node.GetComponent<Node>();
                    Transform nt = node.GetComponent<Transform>();

                    if (node != null)
                    {
                        // Mark selection
                        if (!n.IsSelected())
                        {
                            Transform tempSelect = Instantiate(selection, node.transform.position, Quaternion.identity);
                            tempSelect.transform.parent = n.transform;
                            selectedNode.Add(nt);
                            n.SetSelected(true);
                        }
                    }
                    
                    // When selecting a node, check which type of node is being clicked
                    // Save selected node in a temp node, check for changes in current selected
                    // Replace newly selected with temp node, repeat when there are changes
                }
            }
            else if (Input.GetButton("Fire2"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Node")
                {
                    // We now update the selected node.
                    node = hit.transform;
                    Node n = node.GetComponent<Node>();
                    Transform nt = node.GetComponent<Transform>();

                    if (node != null)
                    {
                        // Remove selection
                        if (n.IsSelected())
                        {
                            SelectionComponent tempSelect = n.GetComponentInChildren<SelectionComponent>();
                            Destroy(tempSelect.gameObject);
                            selectedNode.Remove(nt);
                            n.SetSelected(false);
                        }
                        node = null;
                    }
                }
            }
        }
    }

    public void BtnStartNode()
    {
        /*if (node != null)
        {
            Node n = node.GetComponent<Node>();

            // Making sure only walkable node are able to set as start.
            if (n.IsWalkable() && node != endNode && node != startNode)
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
            }
        }*/

        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();
            if (!selectedNode.Contains(startNode))
            {
                if (n.IsWalkable() && sn != endNode && sn != startNode)
                {
                    if (startNode == null)
                    {
                        SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.blue;
                    }
                    else
                    {
                        // Reverse the color of the previous node
                        SpriteRenderer sRend = startNode.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.white;

                        // Set the new node as blue.
                        sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.blue;
                    }
                    
                    startNode = sn;
                }
            }
            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);
            
            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnEndNode()
    {
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();
            if (!selectedNode.Contains(endNode))
            {
                if (n.IsWalkable() && sn != startNode && sn != endNode)
                {
                    // If this is a new end node, we will just set it to cyan.
                    if (endNode == null)
                    {
                        SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.cyan;
                    }
                    else
                    {
                        // Reverse the color of the previous node
                        SpriteRenderer sRend = endNode.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.white;

                        // Set the new node as cyan.
                        sRend = sn.GetComponent<SpriteRenderer>();
                        sRend.material.color = Color.cyan;
                    }

                    endNode = sn;
                }
            }
            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnFindPath()
    {
        // Only find if there are start and end node.
        if (startNode != null && endNode != null)
        {
            // Execute Shortest Path.
            ShortestPath finder = gameObject.GetComponent<ShortestPath>();
            List<Transform> paths = finder.FindShortestPath(startNode, endNode);

            // Reset the colors created by the path.
            GenerateGridManager gridManager = gameObject.GetComponent<GenerateGridManager>();
            for (int i = 0; i < gridManager.grid.Count; ++i)
            {
                Node currentNode = gridManager.grid[i].GetComponent<Node>();
                SpriteRenderer sRend = currentNode.GetComponent<SpriteRenderer>();
                if (currentNode != startNode || currentNode != endNode || !currentNode.IsWalkable())
                {
                    sRend.material.color = Color.white;
                }
            }

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
        // Check all selections
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();
            
            if (!blockPath.Contains(sn))
            {
                blockPath.Add(sn);
            }

            if (sn == startNode)
            {
                startNode = null;
            }
            if (sn == endNode)
            {
                endNode = null;
            }
            
            SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.black;

            if (sn != startNode && sn != endNode)
            {
                n.SetWalkable(false);
            }

            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
    }

    public void BtnUnblockPath()
    {
        // For multiple selections
        foreach (Transform sn in selectedNode)
        {
            Node n = sn.GetComponent<Node>();

            SpriteRenderer sRend = sn.GetComponent<SpriteRenderer>();
            sRend.material.color = Color.white;

            n.SetWalkable(true);

            if (blockPath.Contains(sn))
            {
                blockPath.Remove(sn);
            }

            if (node == startNode)
            {
                startNode = null;
            }

            if (node == endNode)
            {
                endNode = null;
            }

            SelectionComponent tempSelect = sn.GetComponentInChildren<SelectionComponent>();
            Destroy(tempSelect.gameObject);

            n.SetSelected(false);
        }

        node = null;

        selectedNode.Clear();
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

    public void BtnDiagonal(Toggle value)
    {
        GenerateGridManager generate = gameObject.GetComponent<GenerateGridManager>();

        generate.SetDiagonal(value.isOn);
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
