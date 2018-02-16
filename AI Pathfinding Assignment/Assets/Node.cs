using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        _walkable = walkable;
        _worldPos = worldPosition;
        _gridX = gridX;
        _gridY = gridY;
    }
}
