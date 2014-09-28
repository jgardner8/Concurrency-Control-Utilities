using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace SemaphoreTest
{
	class Program
	{
		private static jamesCC.Semaphore _semaphore = new jamesCC.Semaphore(2);
		private static Random _rand = new Random();

		private static void AcquireTokens()
		{
			while (true)
			{
				bool timedOut = !_semaphore.TryAcquire(5000);
				if (!timedOut)
					Console.WriteLine(Thread.CurrentThread.Name + " ACQUIRED token!");
				else
				{
					Console.WriteLine(Thread.CurrentThread.Name + " timed out...");
					break;
				}
				Thread.Sleep(100);
			}
		}

		private static void ReleaseTokens()
		{
			while (true)
			{
				Console.WriteLine("\nToken released!");
				_semaphore.Release();
				Thread.Sleep(_rand.Next(1000, 3000));
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
