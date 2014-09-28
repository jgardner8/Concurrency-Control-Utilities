using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace TestMutex
{
	class Program
	{
		private static jamesCC.Mutex _mutex = new jamesCC.Mutex();
		private static Random _rand = new Random();
		
		private static void AcquireToken()
		{
			_mutex.Acquire();
			Console.WriteLine("Thread {0} has acquired the lock", Thread.CurrentThread.Name);
			Thread.Sleep(_rand.Next(400, 1500));
			Console.WriteLine("\tThread {0} has released the lock", Thread.CurrentThread.Name);
			_mutex.Release();
			Console.WriteLine("Try to release again...");
			_mutex.Release(); //release twice to see if exceptions work
		}
		
		static void Main(string[] args)
		{
			Console.Title = "Mutex Test";
			
			Thread t;
			for (int i = 0; i < 5; i++)
			{
				t = new Thread(AcquireToken);
				t.Name = "t" + i;
				t.Start();
			}

			Console.ReadLine();
		}
	}
}
