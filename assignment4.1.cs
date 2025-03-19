using System;

public class LinkedList<T>
{
    public class Node
    {
        public T Data { get; set; }
        public Node Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public Node Head { get; private set; }

    public void Add(T data)
    {
        Node newNode = new Node(data);
        if (Head == null)
        {
            Head = newNode;
        }
        else
        {
            Node current = Head;
            while (current.Next != null)
                current = current.Next;
            current.Next = newNode;
        }
    }
}

public static class LinkedListExtensions
{
    public static void ForEach<T>(this LinkedList<T> list, Action<T> action)
    {
        LinkedList<T>.Node current = list.Head;
        while (current != null)
        {
            action(current.Data);
            current = current.Next;
        }
    }
}

class Program
{
    static void Main()
    {
        LinkedList<int> list = new LinkedList<int>();
        list.Add(10);
        list.Add(20);
        list.Add(30);

        // 使用扩展方法打印元素
        Console.WriteLine("Elements:");
        list.ForEach(x => Console.WriteLine(x));

        // 其他操作同上...
    }
}