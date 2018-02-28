using System.Collections.Generic;
using UnityEngine;

public class MinHeap : MonoBehaviour
{
    public class BinaryNode
    {
        Transform node;

        public BinaryNode(Transform node)
        {
            this.node = node;
        }

        public Transform GetNode()
        {
            Transform result = this.node;
            return result;
        }

        public float GetWeight()
        {
            Node n = node.GetComponent<Node>();
            float result = n.GetWeight();
            return result;
        }
    }

    private List<BinaryNode> heap;

    // Creating the heap.
    public void CreateHeap(Transform node)
    {
        // Generate the heap list.
        heap = new List<BinaryNode>();

        // Add the first node into the heap.
        heap.Add(new BinaryNode(node));
    }

    public void Insert(Transform node)
    {
        // Create the node.
        BinaryNode bNode = new BinaryNode(node);

        // Add to the heap.
        heap.Add(bNode);

        // Bubble up to sort the heap.
        this.BubbleUp(heap.Count - 1);
    }

    public Transform Extract()
    {
        // Swap the root with the last time.
        BinaryNode temp = heap[heap.Count - 1];
        heap[heap.Count - 1] = heap[0];
        heap[0] = temp;

        // Remove the last item from the heap.
        Transform result = heap[heap.Count - 1].GetNode();
        heap.RemoveAt(heap.Count - 1);

        // Heapify the heap.
        this.Heapify(0);

        // Return the smallest node.
        return result;
    }

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    private void BubbleUp(int index)
    {

        if (index <= 0)
        {
            return;
        }

        int position = index % 2;

        int parent;
        // We know that current position is on the right
        if (position == 0)
        {
            parent = Mathf.FloorToInt((index / 2) - 1);
        }

        // We know the current position is on the left
        else
        {
            parent = Mathf.FloorToInt((index / 2));
        }

        // We swap the position if the parent is bigger than the child.
        BinaryNode parentNode = heap[parent];
        BinaryNode node = heap[index];
        if (parentNode.GetWeight() > node.GetWeight())
        {
            BinaryNode temp = heap[index];
            heap[index] = parentNode;
            heap[parent] = temp;

            this.BubbleUp(parent); // Continue bubble up if it's not the root node.

        }
    }

    private void Heapify(int index)
    {
        // Calculate the position for left and right node.
        int leftIndex = (2 * index) + 1;
        int rightIndex = (2 * index) + 2;
        int smallest = index;

        // Check if left child or right child has the smallest value.
        if (leftIndex <= heap.Count - 1 && heap[leftIndex].GetWeight() <= heap[smallest].GetWeight())
        {
            smallest = leftIndex;
        }

        if (rightIndex <= heap.Count - 1 && heap[rightIndex].GetWeight() <= heap[smallest].GetWeight())
        {
            smallest = rightIndex;
        }

        // If there is a smallest child, swap and heapify again.
        if (smallest != index)
        {
            BinaryNode temp = heap[index];
            heap[index] = heap[smallest];
            heap[smallest] = temp;

            this.Heapify(smallest);
        }
    }

    public void displayHeap()
    {
        print("==================================");
        int counter = 0;
        foreach (BinaryNode bNode in heap)
        {
            print("index " + counter + " : " + bNode.GetNode().name + " (Weight: " + bNode.GetWeight() + ")");
            counter++;
        }
        print("==================================");
    }

    public Transform root()
    {
        Transform result = heap[0].GetNode();
        return result;
    }
}