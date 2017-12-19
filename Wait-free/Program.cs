using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scan
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var registers = new Register[2];

            for (var i = 0; i < 2; i++)
            {
                registers[i] = new Register(0, i, 2);
            }

            var tasks = new List<Task>();
            var regman = new RegisterManager(registers);

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    var t = i + 2;
                    Task.Run(() =>
                    {
                        Console.WriteLine("Task No {0} begins to Update() register No {1} with val = {2}...",
                            Task.CurrentId, t / 4 % 2, t);
                        regman.Update(t / 4 % 2, t);
                    });
                }
                else
                    tasks.Add(Task.Run(() =>
                    {
                        Console.WriteLine("Task No {0} begins to Scan()...", Task.CurrentId);
                        var arr = regman.Scan();
                        Console.WriteLine("Task No {0} Scaned {{ {1} , {2} }}\n", Task.CurrentId, arr[0], arr[1]);
                    }));

            }
            Task.WaitAll(tasks.ToArray());


            Console.ReadKey();
        }
    }
}
