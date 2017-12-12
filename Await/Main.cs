using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Await
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите URL страницы");
            string URL = Console.ReadLine();
            Parser myClass = new Parser();
            try
            {
                var result = myClass.GetPages(URL);
                result.Wait();
                Console.WriteLine("------------------END------------------");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }   
}
