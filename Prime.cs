using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace prime
{
    class Program
    {
        public static int range = 0;
        public static int count = 0;
        private static Object counting = new Object();
        public static int completed = 0, toComplete = -1;
        private static Object add_in_list = new Object();
        public static List<int> global_list = new List<int>();
        public static ManualResetEvent allDone = new ManualResetEvent(initialState: false);

        //THREADS
        public static void Create_threads(int pr_from, int pr_to)
        {
            if ((pr_to - pr_from + 1) <= range) Func(pr_from, pr_to);
            else
            {
                Thread[] th = new Thread[2];
                lock (counting)
                {
                    count += 2;
                }
                th[0] = new Thread(() => Create_threads(pr_from, (pr_from + pr_to) / 2));
                th[1] = new Thread(() => Create_threads(((pr_from + pr_to) / 2) + 1, pr_to));
                foreach (Thread q in th) q.Start();
                foreach (Thread q in th) q.Join();
                
            }
        }

        //THREADPOOL

        public static void Create_queue_Ex(int pr_from, int pr_to)
        {
            if ((pr_to - pr_from + 1) <= range)
            {
                ThreadPool.QueueUserWorkItem((_ =>
                {
                    Func(pr_from, pr_to);
                }));
            }
            else
            {
                lock (counting)
                {
                    count += 2;
                }
                Create_queue_Ex(pr_from, (pr_from + pr_to) / 2);
                Create_queue_Ex(((pr_from + pr_to) / 2) + 1, pr_to);
            }
        }


        //TASKS
        public static void Create_tasks(int pr_from, int pr_to)
        {
            if ((pr_to - pr_from + 1) <= range) Func(pr_from, pr_to);
            else
            {
                lock (counting)
                {
                    count += 2;
                }
                Task leftTask = Task.Run(() => Create_tasks(pr_from, (pr_from + pr_to) / 2));
                Task rightTask = Task.Run(() => Create_tasks(((pr_from + pr_to) / 2) + 1, pr_to));

                Task.WaitAll(leftTask, rightTask);

            }
        }
        


        //CHECK PRIME IN RANGE
        public static void Func(int pr_from, int pr_to)
        {
            bool b = true;
            for (int i = pr_from; i <= pr_to; i++)
            {
                b = true;
                for (int j = 2; j <= Math.Ceiling(Math.Sqrt(i)); j++)
                {
                    if (((i % j) == 0) && (i != j))
                    {
                        b = false;
                        break;
                    }
                }
                lock (add_in_list)
                {
                    if (b && (i != 1)) global_list.Add(i);
                }
            }
            if (Interlocked.Increment(ref completed) == toComplete) allDone.Set();
        }

        //MAIN
        static void Main()
        {
            //GET RANGE
            int pr_from = 0, pr_to = 0;
            bool b = true;
            while (b)
            {
                Console.Write("Prime numbers from : ");
                try
                {
                    pr_from = Convert.ToInt32(Console.ReadLine());
                    b = false;
                }
                catch (Exception)
                {
                    Console.Write("You should write a number\n");
                }
            }
            b = true;
            while (b)
            {
                Console.Write("Prime numbers to : ");
                try
                {
                    pr_to = Convert.ToInt32(Console.ReadLine());
                    b = false;
                    if (pr_from > pr_to)
                    {
                        int buf = pr_from;
                        pr_from = pr_to;
                        pr_to = buf;
                    }
                }
                catch (Exception)
                {
                    Console.Write("You should write a number\n");
                    //TODO : нецелые числа
                }
            }

            //THREADS
            long time = 100000000;
            int opt_range = 0;
            Stopwatch time_threads = new Stopwatch();
            range = pr_to;
            do
            {
                time_threads.Restart();
                Create_threads(pr_from, pr_to);
                time_threads.Stop();
                if (time > time_threads.ElapsedMilliseconds)
                {
                    opt_range = range;
                    time = time_threads.ElapsedMilliseconds;
                }
                if (count == 0) count = 1;
                Console.Write("There is/are {0} thread/s. There are/is {1} prime number/s, {2} time spent, range = {3}\n", count, global_list.Count, time_threads.Elapsed, range);

                //global_list.Sort();
                //foreach (int number in global_list) Console.WriteLine(number);

                global_list.Clear();
                count = 0;
                range /= 2;
            } while (range > (pr_to / 64));

            //*********************//

            //THREADPOOL
            completed = 0;
            toComplete = 1;
            range = (opt_range/10)+1; 
            int buffer = pr_to;
            while (buffer > range)
            {
                buffer /= 2;
                toComplete *= 2;
            }
            time_threads.Restart();
            Create_queue_Ex(pr_from, pr_to);
            allDone.WaitOne();
            time_threads.Stop();
            Console.Write("ThreadPool. There are/is {0} prime number/s, {1} time spent\n",global_list.Count, time_threads.Elapsed);
            global_list.Clear();
            count = 0;
            //global_list.Sort();
            //foreach (int number in global_list) Console.WriteLine(number);

            completed = 0;
            toComplete = -1;

            //*********************//

            //TASKS
            time_threads.Restart();
            Create_tasks(pr_from, pr_to);
            time_threads.Stop();
            Console.Write("Tasks. There are/is {0} prime number/s, {1} time spent\n", global_list.Count, time_threads.Elapsed);

            //global_list.Sort();
            //foreach (int number in global_list) Console.WriteLine(number);
            Console.ReadKey();
        }
    }
}
