using System;

public class LinkedList<T>
{
    private class Node<TNode>
    {
        public TNode Data { get; set; }
        public Node<TNode> Next { get; set; }

        public Node(TNode data)
        {
            Data = data;
            Next = null;
        }
    }

    private Node<T> head;

    public void Add(T data)
    {
        Node<T> newNode = new Node<T>(data);
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
    }

    public void ForEach(Action<T> action)
    {
        Node<T> current = head;
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
        list.Add(5);
        list.Add(20);
        list.Add(15);

        // 打印元素
        Console.WriteLine("链表元素:");
        list.ForEach(x => Console.WriteLine(x));

        // 求最大值、最小值和求和
        int max = int.MinValue;
        int min = int.MaxValue;
        int sum = 0;

        list.ForEach(x =>
        {
            if (x > max) max = x;
            if (x < min) min = x;
            sum += x;
        });

        Console.WriteLine($"最大值: {max}");
        Console.WriteLine($"最小值: {min}");
        Console.WriteLine($"和: {sum}");
    }
}


using System;
using System.Threading;

public class AlarmClock
{
    public DateTime CurrentTime { get; private set; } = DateTime.Now;
    public DateTime AlarmTime { get; set; }

    public event EventHandler Tick;
    public event EventHandler Alarm;

    public void Start()
    {
        Console.WriteLine($"闹钟启动，当前时间: {CurrentTime:HH:mm:ss}");
        Console.WriteLine($"闹铃设定时间: {AlarmTime:HH:mm:ss}");

        while (CurrentTime < AlarmTime)
        {
            Thread.Sleep(1000); 
            CurrentTime = CurrentTime.AddSeconds(1);
            Tick?.Invoke(this, EventArgs.Empty); 

            if (CurrentTime >= AlarmTime)
            {
                Alarm?.Invoke(this, EventArgs.Empty); 
            }
        }
    }
}

class Program
{
    static void Main()
    {
        AlarmClock clock = new AlarmClock();
        clock.AlarmTime = DateTime.Now.AddSeconds(5); 

      
        clock.Tick += (sender, e) =>
        {
            Console.WriteLine($"滴答... 当前时间: {clock.CurrentTime:HH:mm:ss}");
        };

    
        clock.Alarm += (sender, e) =>
        {
            Console.WriteLine("叮铃铃!!! 该起床了！");
        };

        clock.Start();
    }
}