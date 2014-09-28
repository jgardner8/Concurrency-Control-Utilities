using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace SemaphoreTest
{
	class Program
	{
		private static jamesCC.SimpleSemaphore _semaphore = new jamesCC.SimpleSemaphore();
		private static Random _rand = new Random();

		private static void AcquireTokens()
		{
			while (true)
			{
				_semaphore.Acquire();
				Console.WriteLine(Thread.CurrentThread.Name + " ACQUIRED token!");
			}
		}

		private static void ReleaseTokens()
		{
			while (true)
			{
				Console.WriteLine("\nToken released!");
				_semaphore.Release();
				Thread.Sleep(_rand.Next(500, 1000));
			}
		}

		public static void Main(string[] args)
		{
			Console.Title = "Semaphore Test";

			Thread[] threads = new Thread[3];
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i] = new Thread(AcquireTokens);
				threads[i].Name = "T " + i;
				threads[i].Start();
			}

			ReleaseTokens();
		}
	}
}
