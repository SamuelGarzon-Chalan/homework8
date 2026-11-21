// PART A
//
// A1. After each insertion, what is the value of the top element of the max heap?
// Insert 15: [15]                     0 swaps
// Insert 40: [40, 15]                 1 swap
// Insert 25: [40, 15, 25]             0 swaps
// Insert 50: [50, 40, 25, 15]         2 swaps
// Insert 10: [50, 40, 25, 15, 10]     0 swaps
// Insert 45: [50, 40, 45, 15, 10, 25] 1 swap
//
// A2. RemoveMax
// First call returns 50. Heap: [45, 40, 25, 15, 10]
// Second call returns 45. Heap: [40, 15, 25, 10]
//
// A3. BuildMaxHeap
// The first call starts at index 7 / 2 - 1 = 2, the last parent.
// Leaves from index 3 to 6 do not have children to check so they are ignored.
// Heapify at index 2: [5, 12, 15, 20, 8, 3, 1]
// Heapify at index 1: [5, 20, 15, 12, 8, 3, 1]
// Heapify at index 0: [20, 12, 15, 5, 8, 3, 1]
//
// A4. The part of the list on the right side of | is already sorted.
// Round 1: [15, 12, 3, 5, 8, 1 | 20]
// Round 2: [12, 8, 3, 5, 1 | 15, 20]
//
// A5. Final min heap: [10, 15, 25, 50, 40, 45]
//
// A6. Big O
// Implementation            Insert      Peek    RemoveMax
// Unsorted list             O(1)*       O(n)    O(n)
// Sorted list (max at end)  O(n)        O(1)    O(1)
// Binary heap               O(log n)*   O(1)    O(log n)
// * Amortized if underlying list is going to resize.
//
// will return the highest-priority item whenever it is called.// Peek will return the highest-priority item the next time it is called, because the highest-priority item is at the root of a binary heap.
// takes constant time. The heap property is restored when Insert and RemoveMax are called
//Apart from the linear time that it takes for the first two to follow one path,  runs in logarithmic time,
//Occasional resize(s) while insertion.
//
// A7. Real-life examples
// 1. The items in this triage are patients and the priority is medical urgency.
// 2. The items are print jobs; and the priority is the urgency of each print job.
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("B1. MinPriorityQueue");

        MinPriorityQueue queue = new MinPriorityQueue();
        int[] priorities = { 15, 40, 25, 50, 10, 45 };

        foreach (int priority in priorities)
        {
            queue.Insert(priority);
        }

        Console.WriteLine($"Count: {queue.Count}");
        Console.WriteLine($"Peek: {queue.Peek()}");

        List<int> removed = new List<int>();
        while (!queue.IsEmpty)
        {
            removed.Add(queue.RemoveMin());
        }
        Console.WriteLine(string.Join(", ", removed));

        try
        {
            queue.RemoveMin();
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"Empty RemoveMin: {exception.Message}");
        }

        try
        {
            queue.Peek();
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"Empty Peek: {exception.Message}");
        }

        Console.WriteLine("\nB2. IsMaxHeap");

        int[][] testArrays =
        {
            new[] { 90, 70, 80, 30, 60, 50 },
            new[] { 50, 40, 30, 45, 10 },
            new[] { 10, 20, 30 },
            new[] { 7 },
            Array.Empty<int>()
        };

        foreach (int[] values in testArrays)
        {
            Console.WriteLine($"[{string.Join(", ", values)}]: {IsMaxHeap(values)}");
        }

        Console.WriteLine("\nB3. HeapSort");

        int[] valuesToSort = { 5, 12, 3, 20, 8, 15, 1 };
        int[] heap = (int[])valuesToSort.Clone();

        HeapSort(valuesToSort);
        Console.WriteLine(string.Join(", ", valuesToSort));

        BuildMaxHeap(heap);
        Console.WriteLine($"Built heap: [{string.Join(", ", heap)}]");
        Console.WriteLine($"IsMaxHeap: {IsMaxHeap(heap)}");

        Console.WriteLine("\nB4. TopK");

        int[] scores = { 42, 17, 99, 63, 8, 75, 31 };
        Console.WriteLine(string.Join(", ", TopK(scores, 3)));
    }

    public static bool IsMaxHeap(int[] arr)
    {
        for (int child = 1; child < arr.Length; child++)
        {
            int parent = (child - 1) / 2;

            if (arr[parent] < arr[child])
            {
                return false;
            }
        }

        return true;
    }

    public static void Heapify(int[] arr, int size, int i)
    {
        while (i < size / 2)
        {
            int left = 2 * i + 1;
            int right = left + 1;
            int largerChild = left;

            if (right < size && arr[right] > arr[left])
            {
                largerChild = right;
            }

            if (arr[i] >= arr[largerChild])
            {
                return;
            }

            (arr[i], arr[largerChild]) = (arr[largerChild], arr[i]);
            i = largerChild;
        }
    }

    public static void BuildMaxHeap(int[] arr)
    {
        for (int i = arr.Length / 2 - 1; i >= 0; i--)
        {
            Heapify(arr, arr.Length, i);
        }
    }

    public static void HeapSort(int[] arr)
    {
        BuildMaxHeap(arr);

        for (int end = arr.Length - 1; end > 0; end--)
        {
            (arr[0], arr[end]) = (arr[end], arr[0]);
            Heapify(arr, end, 0);
        }
    }

    public static int[] TopK(int[] scores, int k)
    {
        if (k < 0 || k > scores.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        if (k == 0)
        {
            return Array.Empty<int>();
        }

        PriorityQueue<int, int> best = new PriorityQueue<int, int>();

        foreach (int score in scores)
        {
            best.Enqueue(score, score);

            if (best.Count > k)
            {
                best.Dequeue();
            }
        }

        int[] result = new int[k];

       
        for (int i = k - 1; i >= 0; i--)
        {
            result[i] = best.Dequeue();
        }

        return result;
    }
}

public class MinPriorityQueue
{
    private readonly List<int> heap = new List<int>();

    public int Count => heap.Count;
    public bool IsEmpty => Count == 0;

    public void Insert(int priority)
    {
        heap.Add(priority);
        int child = Count - 1;

        while (child > 0)
        {
            int parent = (child - 1) / 2;

            if (heap[child] >= heap[parent])
            {
                return;
            }

            (heap[child], heap[parent]) = (heap[parent], heap[child]);
            child = parent;
        }
    }

    public int Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        return heap[0];
    }

    public int RemoveMin()
    {
        int minimum = Peek();
        int last = Count - 1;

        heap[0] = heap[last];
        heap.RemoveAt(last);

        int parent = 0;

        while (parent < Count / 2)
        {
            int left = 2 * parent + 1;
            int right = left + 1;
            int smallerChild = left;

            if (right < Count && heap[right] < heap[left])
            {
                smallerChild = right;
            }

            if (heap[parent] <= heap[smallerChild])
            {
                break;
            }

            (heap[parent], heap[smallerChild]) = (heap[smallerChild], heap[parent]);
            parent = smallerChild;
        }

        return minimum;
    }
}