using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BST
{
    static partial class BST
    {
        class Program
        {
            public static int key;
            public static string value;
            public static bool flag = true;

            static void GetKeyValue(string temp)
            {
                int pos = temp.IndexOf(' ');
                if (pos == -1)
                {
                    flag = false;
                    return;
                }
                try
                {
                    key = Convert.ToInt32(temp.Substring(0,pos));
                }
                catch (Exception)
                {
                    flag = false;
                    return;
                }
                value = temp.Substring(pos + 1);
                flag = true;
            }


            static void Main()
            {
                BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
                Console.WriteLine("Ручной ввод или Тестирование скорости распараллеливания?\n" +
                    "Введие 1 для ручного ввода\n" +
                    "Введите 2 для Тестирования скорости распараллеливания");
                var choose = 1;
                while (flag)
                {
                    try
                    {
                        choose = Convert.ToInt32(Console.ReadLine());
                        flag = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Неверный ввод. Введите 1 или 2");
                    }
                }
                flag = true;
                if (choose == 1)
                {
                    char cur = 'I';
                    while (cur != 'E')
                    {
                        if (cur == 'I')
                        {
                            Console.WriteLine("Вводите через Enter узлы на insert вида \"key value\" (без \"\") ");
                            Console.WriteLine("Если захотите выйти в основное меню, введите 'M' (без '') ");
                            while (cur == 'I')
                            {
                                var temp1 = Console.ReadLine();
                                if (temp1 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    GetKeyValue(temp1);
                                    if (!flag) Console.WriteLine("Неправильный ввод. \n" +
                                        "Вводите через Enter узлы на insert вида \"key value\" (без \"\") \n" +
                                        "Если захотите выйти в основное меню, введите 'M' (без '') ");
                                    else
                                    {
                                        tree.Insert(key, value);
                                    }
                                }
                            }
                        }
                        else if (cur == 'M')
                        {
                            bool flagcase = true;
                            do
                            {
                                Console.WriteLine("Меню\n" +
                                "Если вы хотите ввести новые узлы в дерево, введите 'I'\n" +
                                "Если вы хотите найти значение по ключу, введите 'F'\n" +
                                "Если вы хотите удалить узел, введите 'D'\n" +
                                "Eсли вы хотите выйти, введите 'E'\n" +
                                "Для вывода дерева введите 'P'");

                                var temp2 = Console.ReadLine();
                                switch (temp2)
                                {
                                    case "I":
                                        cur = 'I';
                                        flagcase = false;
                                        break;
                                    case "E":
                                        cur = 'E';
                                        flagcase = false;
                                        break;
                                    case "D":
                                        cur = 'D';
                                        flagcase = false;
                                        break;
                                    case "F":
                                        cur = 'F';
                                        flagcase = false;
                                        break;
                                    case "P":
                                        cur = 'P';
                                        flagcase = false;
                                        break;
                                    default:
                                        Console.WriteLine("Неправильный ввод");
                                        break;
                                }
                            } while (flagcase);

                        }
                        else if (cur == 'D')
                        {
                            Console.WriteLine("Вводите ключи для удаления узлов");
                            Console.WriteLine("Если захотите выйти в основное меню, введите 'M' (без '') ");
                            while (cur == 'D')
                            {
                                var temp3 = Console.ReadLine();
                                if (temp3 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    try
                                    {
                                        key = Convert.ToInt32(temp3);
                                        tree.Delete(key);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Неправильный ввод. Введите числовой ключ \n" +
                                        "Введите ключ для удаления узла \n" +
                                        "Если захотите выйти в основное меню, введите 'M' (без '') ");
                                    }
                                }
                            }
                        }
                        else if (cur == 'F')
                        {
                            Console.WriteLine("Вводите ключи для поиска значений");
                            Console.WriteLine("Если захотите выйти в основное меню, введите 'M' (без '') ");
                            while (cur == 'F')
                            {
                                var temp4 = Console.ReadLine();
                                if (temp4 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    try
                                    {
                                        key = Convert.ToInt32(temp4);
                                        value = tree.Find(key);
                                        if (value == default(string)) Console.WriteLine("нет значений по такому ключу");
                                        else Console.WriteLine("{0}", value);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Неправильный ввод. Введите числовой ключ \n" +
                                        "Введите ключ для удаления узла \n" +
                                        "Если захотите выйти в основное меню, введите 'M' (без '') ");
                                    }
                                }
                            }
                        }
                        else if (cur == 'P')
                        {
                            Print<int, string> print = new Print<int, string>();
                            print.PrintTree(tree);
                            cur = 'M';
                        }
                    }
                }
                else if (choose == 2)
                {
                    int[] forInsert = new int[10000000];
                    int[] forFind = new int[10000000];
                    int[] forDelete = new int[5000000];

                    Random rnd = new Random();
                    for (int j =0; j< forInsert.Length; j++)
                    {
                        forInsert[j] = (rnd.Next(1,10000000));
                    }
                    for (int j = 0; j < forFind.Length; j++)
                    {
                        forFind[j] = forInsert[(rnd.Next(1, 10000000))];
                    }
                    for (int j = 0; j < forDelete.Length; j++)
                    {
                        forDelete[j] = forInsert[(rnd.Next(1, 10000000))];
                    }
                    BinarySearchTree<int, string> parallel_tree = new BinarySearchTree<int, string>();
                    BinarySearchTree<int, string> non_parallel_tree = new BinarySearchTree<int, string>();

                    Stopwatch parallel_insert = Stopwatch.StartNew();
                    Parallel.ForEach(forInsert, key =>
                    {
                        parallel_tree.Insert(key,".");
                    });
                    parallel_insert.Stop();
                    Console.WriteLine("parallel insert: {0}", parallel_insert.Elapsed);
                    if (!parallel_tree.IsBreak(parallel_tree.root)) throw new Exception("Tree is break after parallel insert");

                    Stopwatch parallel_find = Stopwatch.StartNew();
                    Parallel.ForEach(forFind, key =>
                     {
                         parallel_tree.Find(key);
                     });
                    parallel_find.Stop();
                    Console.WriteLine("parallel find: {0}", parallel_find.Elapsed);
                    if (!parallel_tree.IsBreak(parallel_tree.root)) throw new Exception("Tree is break after parallel find");

                    Stopwatch parallel_delete = Stopwatch.StartNew();
                    Parallel.ForEach(forDelete, key =>
                    {
                        parallel_tree.Delete(key);
                    });
                    parallel_delete.Stop();
                    Console.WriteLine("parallel delete: {0}", parallel_delete.Elapsed);
                    if (!parallel_tree.IsBreak(parallel_tree.root)) throw new Exception("Tree is break after parallel delete");

                    Stopwatch non_parallel_insert = Stopwatch.StartNew();
                    foreach(int key in forInsert)
                    {
                        non_parallel_tree.Insert(key,".");
                    }
                    non_parallel_insert.Stop();
                    Console.WriteLine("non parallel insert: {0}", non_parallel_insert.Elapsed);
                    if (!non_parallel_tree.IsBreak(non_parallel_tree.root)) throw new Exception("Tree is break after non parallel insert");

                    Stopwatch non_parallel_find = Stopwatch.StartNew();
                    foreach (int key in forFind)
                    {
                        non_parallel_tree.Find(key);
                    }
                    non_parallel_find.Stop();
                    Console.WriteLine("non parallel find: {0}", non_parallel_find.Elapsed);
                    if (!non_parallel_tree.IsBreak(non_parallel_tree.root)) throw new Exception("Tree is break after non parallel find");

                    Stopwatch non_parallel_delete = Stopwatch.StartNew();
                    foreach (int key in forDelete)
                    {
                        non_parallel_tree.Delete(key);
                    }
                    non_parallel_delete.Stop();
                    Console.WriteLine("non parallel delete: {0}", non_parallel_delete.Elapsed);
                    if (!non_parallel_tree.IsBreak(non_parallel_tree.root)) throw new Exception("Tree is break after non parallel delete");
                    Console.ReadKey();
                }
            }
        }
    }
}
