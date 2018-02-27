using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private Transform parentNode = null;
    [SerializeField] private List<Transform> neighbourNode;
    [SerializeField] private bool walkable = true;

    void Start()
    {
        this.resetNode();
    }

    public void resetNode()
    {
        weight = int.MaxValue;
        parentNode = null;
    }

    public void setParentNode(Transform node)
    {
        this.parentNode = node;
    }

    public void setWeight(float value)
    {
        this.weight = value;
    }

    public void setWalkable(bool value)
    {
        this.walkable = value;
    }

    public void addNeighbourNode(Transform node)
    {
        this.neighbourNode.Add(node);
    }

    public List<Transform> getNeighbourNode()
    {
        List<Transform> result = this.neighbourNode;
        return result;
    }

    public float getWeight()
    {
        float result = this.weight;
        return result;

    }

    public Transform getParentNode()
    {
        Transform result = this.parentNode;
        return result;
    }

    public bool isWalkable()
    {
        bool result = walkable;
        return result;
    }
}
