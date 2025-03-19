using System;
using System.Timers;

public class AlarmClock2
{
    private Timer timer;

    public event EventHandler Tick;
    public event EventHandler Alarm;

    public DateTime AlarmTime { get; set; }

    public void Start()
    {
        timer = new Timer(1000);
        timer.Elapsed += (sender, e) =>
        {
            Tick?.Invoke(this, EventArgs.Empty);
            if (DateTime.Now >= AlarmTime)
            {
                Alarm?.Invoke(this, EventArgs.Empty);
                timer.Stop();
            }
        };
        timer.AutoReset = true;
        timer.Start();
    }
}

class Program
{
    static void Main()
    {
        var clock = new AlarmClock2();
        clock.AlarmTime = DateTime.Now.AddSeconds(5);
        clock.Tick += (s, e) => Console.WriteLine($"[Tick] {DateTime.Now.ToString("HH:mm:ss")}");
        clock.Alarm += (s, e) => Console.WriteLine($"[Alarm] Time's up at {DateTime.Now.ToString("HH:mm:ss")}!");
        clock.Start();
        Console.ReadLine();
    }
}