using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace Barrier
{
    class Program
    {
        private static jamesCC.Barrier _barrier = new jamesCC.Barrier(5);
        private static Random _rand = new Random();

        private static void HitBarrier()
        {
            Thread.Sleep(_rand.Next(10000));
            Console.WriteLine(Thread.CurrentThread.Name + " arrived at the barrier.");
            _barrier.Arrive();
            Console.WriteLine(Thread.CurrentThread.Name + " passed the barrier.");
        }

        static void Main(string[] args)
        {
            Console.Title = "Barrier Test";

            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(HitBarrier);
                threads[i].Name = "T" + (i + 1);
                threads[i].Start();
            }

            Console.ReadLine();
        }
    }
}
