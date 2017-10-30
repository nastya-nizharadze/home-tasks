using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace md5
{
    class Program
    {
        public static byte[] GetHash(FileStream file_stream) // хэш от файла
        {
            var md5 = MD5.Create(); 
            return md5.ComputeHash(file_stream);
            
        }

        public static byte[] GetFolderHash(string path)
        {
            var paths = Directory.GetFiles(path, "*", SearchOption.AllDirectories); //все файлы из всех папок
            Array.Sort(paths);

            List<Byte[]> hash = new List<Byte[]>();

            var md5 = MD5.Create();
            
            List<Task<byte[]>> tasks = new List<Task<byte[]>>(); //список тасков по высчитыванию хэша

            foreach (var p in paths) // добавление в tasks тасков по высчитыванию хеша от всех файлов по всем путям
            {
                var stream = File.OpenRead(p);
                Task<byte[]> task = Task.Run(() => GetHash(stream));
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            foreach (Task<byte[]> task in tasks) //заполнение списка результатами 
            {
                hash.Add((task.Result)); 
            }

            byte[] array = hash.SelectMany(a => a).ToArray();

            return md5.ComputeHash(array); // хеш от всех хешей
            
        }


        static void Main(string[] args)
        {
            string path = "";
            Console.WriteLine ("write path to folder: ");
            bool b = true;
            while (b)
            {
                path = Console.ReadLine();
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("No such directory, Prease write again:");
                }
                else
                {
                    b = false;

                }
            }

            Stopwatch time = new Stopwatch();
            time.Start();
            var hash = GetFolderHash(path);
            time.Stop();

            Console.WriteLine("Time spent: {0}; Directory MD5 hash: {1}", time.Elapsed, BitConverter.ToString(hash).Replace("-", "").ToLower());

            Console.ReadKey();
        }
    }
}
