using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private float hCost;
    [SerializeField] private float gCost;
    [SerializeField] private float fCost;
    [SerializeField] private Transform parentNode = null;
    [SerializeField] private List<Transform> neighbourNode;
    [SerializeField] private bool walkable = true;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isSettled = false;

    void Start()
    {
        this.ResetNode();
    }

    public float GetHCost
    {
        get
        {
            return hCost;
        }
    }

    public void SetHCost(float value)
    {
        this.hCost = value;
    }

    public float GetGCost
    {
        get
        {
            return gCost;
        }
    }

    public void SetGCost(float value)
    {
        this.gCost = value;
    }

    public float GetFCost
    {
        get
        {
            return fCost = gCost + hCost;
        }
    }

    public bool GetSettled
    {
        get
        {
            return isSettled;
        }
    }

    public void SetSettled(bool value)
    {
        this.isSettled = value;
    }

    public void SetNeighbourNode(List<Transform> list)
    {
        this.neighbourNode = list;
    }

    public void ResetNode()
    {
        weight = int.MaxValue;
        parentNode = null;
    }

    public void SetParentNode(Transform node)
    {
        this.parentNode = node;
    }

    public void SetWeight(float value)
    {
        this.weight = value;
    }

    public void SetWalkable(bool value)
    {
        this.walkable = value;
    }

    public void AddNeighbourNode(Transform node)
    {
        this.neighbourNode.Add(node);
    }

    public List<Transform> GetNeighbourNode()
    {
        List<Transform> result = this.neighbourNode;
        return result;
    }

    public float GetWeight()
    {
        float result = this.weight;
        return result;
    }

    public Transform GetParentNode()
    {
        Transform result = this.parentNode;
        return result;
    }

    public bool IsWalkable()
    {
        bool result = walkable;
        return result;
    }

    public bool IsSelected()
    {
        bool result = isSelected;
        return result;
    }

    public void SetSelected(bool value)
    {
        this.isSelected = value;
    }
}
