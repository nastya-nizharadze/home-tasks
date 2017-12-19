using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scan
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var registers = new Register[2]; //make 2 registers
            for (var i = 0; i < 2; i++)
            {
                registers[i] = new Register  //init them    
                {
                    id = i,
                    data = 0,
                    bitmask = new bool[2],
                    view = new int[2]
                 };
            }
            var tasks = new List<Task>(); // make tasks for changing registers and check if our algorithm is working right
            var scanning = new Scanning(registers);
            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0) // even for update, odd for scan, its easy to check code now
                {
                    var t = i + 2;
                    Task.Run(() =>
                    {
                        Console.WriteLine("Task No {0} begins to Update() register No {1} with val = {2}...", Task.CurrentId, t / 4 % 2, t);
                        scanning.Update(t / 4 % 2, t);  
                    });
                }
                else
                {
                    tasks.Add(Task.Run(() =>
                    {
                        Console.WriteLine("Task No {0} begins to Scan()...", Task.CurrentId);
                        var scan = scanning.Scan();
                        Console.WriteLine("Task No {0} Scaned {{ {1} , {2} }}\n", Task.CurrentId, scan[0], scan[1]);
                    }));
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.ReadKey();
        }
    }
}
