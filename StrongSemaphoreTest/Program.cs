using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace FifoSemaphoreTest
{
	class Program
	{	
		private static jamesCC.FifoSemaphore _semaphore = new jamesCC.FifoSemaphore();
		private static Random _rand = new Random();

		private static void AcquireTokens()
		{
			while (true)
			{
				//This can't 100% prove that the FIFO semaphore works, as the time between print
				//and acquire varies between threads... but it works 99% of the time.
                Console.WriteLine(Thread.CurrentThread.Name + " is waiting...");
				_semaphore.Acquire();
				Console.WriteLine("\t" + Thread.CurrentThread.Name + " ACQUIRED token!");
			}
		}

		private static void ReleaseTokens()
		{
			while (true)
			{
				_semaphore.Release();
				Thread.Sleep(_rand.Next(1000, 2000));
			}
		}

		public static void Main(string[] args)
		{
			Console.Title = "Fifo Semaphore Test";

			Thread[] threads = new Thread[10];
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i] = new Thread(AcquireTokens);
				threads[i].Name = "T " + i;
				threads[i].Start();
			}

			Thread.Sleep(2000);

			ReleaseTokens();
		}
	}
}
