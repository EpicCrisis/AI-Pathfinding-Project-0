using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public enum NodeState
    {
        Walkable,
        Unwalkable,
        Settled,
        Start,
        End
    }

    public int x;
    public int y;

    public NodeState state;
    public NodeState nextState;

    public SpriteRenderer sRender;

    private void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();      
    }

    public Node(int gridX, int gridY)
    {
        gridX = x;
        gridY = y;
    }

    public int GetNeighbours()
    {
        int neighbours = 0;

        for (int i = x - 1; x <= x + 1; i++)
        {
            for (int j = y - 1; y <= y + 1; j++)
            {
                if (GridManager.instance.nodes[i, j].state == NodeState.Walkable)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    public void InitNode (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    private void UpdateSprite()
    {
        if (state == NodeState.Walkable)
        {
            sRender.color = Color.white;
        }
        else
        {
            sRender.color = Color.gray;
        }
    }

    public void ClearNode()
    {
        state = NodeState.Walkable;
        UpdateSprite();
    }

    public void NodeUpdate()
    {
        int aliveCells = GetNeighbours();

        nextState = state;

        if (state == NodeState.Walkable)
        {
            // Code for checking distance.
        }
        else
        {
            return;
        }
    }

    public void ApplyNodeUpdate()
    {
        state = nextState;
        UpdateSprite();
    }

    void OnMouseOver()
    {
        if (!UIHoverListener.isUIOverride)
        {
            //Debug.Log ("Mouse is over this cell");

            if (Input.GetButton("Fire1"))
            {
                //Debug.Log ("LMB this object : " + this.transform);

                state = NodeState.Unwalkable;
                UpdateSprite();
            }
            else if (Input.GetButton("Fire2"))
            {
                //Debug.Log ("RMB this object : " + this.transform);

                state = NodeState.Walkable;
                UpdateSprite();
            }
        }
    }
}
