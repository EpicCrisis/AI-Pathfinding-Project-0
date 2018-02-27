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

        public Transform getNode()
        {
            Transform result = this.node;
            return result;
        }

        public float getWeight()
        {
            Node n = node.GetComponent<Node>();
            float result = n.getWeight();
            return result;
        }
    }

    private List<BinaryNode> heap;

    // Creating the heap.
    public void createHeap(Transform node)
    {
        // Generate the heap list.
        heap = new List<BinaryNode>();

        // Add the first node into the heap.
        heap.Add(new BinaryNode(node));
    }

    public void insert(Transform node)
    {
        // Create the node.
        BinaryNode bNode = new BinaryNode(node);

        // Add to the heap.
        heap.Add(bNode);

        // Bubble up to sort the heap.
        this.bubbleUp(heap.Count - 1);
    }

    public Transform extract()
    {
        // Swap the root with the last time.
        BinaryNode temp = heap[heap.Count - 1];
        heap[heap.Count - 1] = heap[0];
        heap[0] = temp;

        // Remove the last item from the heap.
        Transform result = heap[heap.Count - 1].getNode();
        heap.RemoveAt(heap.Count - 1);

        // Heapify the heap.
        this.heapify(0);

        // Return the smallest node.
        return result;
    }

    public bool isEmpty()
    {
        return heap.Count == 0;
    }

    private void bubbleUp(int index)
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
        if (parentNode.getWeight() > node.getWeight())
        {
            BinaryNode temp = heap[index];
            heap[index] = parentNode;
            heap[parent] = temp;

            this.bubbleUp(parent); // Continue bubble up if it's not the root node.

        }

    }

    private void heapify(int index)
    {
        // Calculate the position for left and right node.
        int leftIndex = (2 * index) + 1;
        int rightIndex = (2 * index) + 2;
        int smallest = index;

        // Check if left child or right child has the smallest value.
        if (leftIndex <= heap.Count - 1 && heap[leftIndex].getWeight() <= heap[smallest].getWeight())
        {
            smallest = leftIndex;
        }

        if (rightIndex <= heap.Count - 1 && heap[rightIndex].getWeight() <= heap[smallest].getWeight())
        {
            smallest = rightIndex;
        }

        // If there is a smallest child, swap and heapify again.
        if (smallest != index)
        {
            BinaryNode temp = heap[index];
            heap[index] = heap[smallest];
            heap[smallest] = temp;

            this.heapify(smallest);
        }
    }

    public void displayHeap()
    {
        print("==================================");
        int counter = 0;
        foreach (BinaryNode bNode in heap)
        {
            print("index " + counter + " : " + bNode.getNode().name + " (Weight: " + bNode.getWeight() + ")");
            counter++;
        }
        print("==================================");
    }

    public Transform root()
    {
        Transform result = heap[0].getNode();
        return result;
    }
}