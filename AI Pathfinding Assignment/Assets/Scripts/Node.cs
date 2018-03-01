using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private Transform parentNode = null;
    [SerializeField] private List<Transform> neighbourNode;
    [SerializeField] private bool walkable = true;
    [SerializeField] private bool isSelected = false;

    void Start()
    {
        this.ResetNode();
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
