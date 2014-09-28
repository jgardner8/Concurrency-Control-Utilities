using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jamesCC = ConcurrencyConstructs;

namespace LatchTest
{
    class Program
    {
        private static jamesCC.Latch _latch = new jamesCC.Latch();

        private static void Wait()
        {
            _latch.Wait();
            Console.WriteLine(Thread.CurrentThread.Name + " released!");
        }

        static void Main(string[] args)
        {
            Console.Title = "Latch Test";

            Thread[] threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(Wait);
                threads[i].Name = "T" + (i+1);
                threads[i].Start();
            }

            Console.WriteLine("All threads initialised.");
            Console.WriteLine("Releasing in...");
            Thread.Sleep(1000);

            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }

            _latch.Release();
            Console.ReadLine();
        }
    }
}
