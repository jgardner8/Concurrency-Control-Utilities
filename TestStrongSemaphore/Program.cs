using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace SemaphoreTest
{
	/// <summary>
	/// Exactly the same as TestSemaphore, except uses a StrongSemaphore.
	/// As sem.Acq() is seperated into rooms, the same thread should never acquire
	/// repeatedly as often happens in TestSemaphore
	/// </summary>
	class Program
	{
		private static jamesCC.StrongSemaphore _semaphore = new jamesCC.StrongSemaphore();
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
