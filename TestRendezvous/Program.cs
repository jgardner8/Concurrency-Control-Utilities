using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace TestRendezvous
{
	class Program
	{
		private static jamesCC.Rendezvous<string> _rendezvous = new jamesCC.Rendezvous<string>();
		
		private static void ExchangeData()
		{
			Console.WriteLine("Thread " + Thread.CurrentThread.Name + " has arrived!");
			string newData = _rendezvous.Exchange(Thread.CurrentThread.Name);
			Console.WriteLine("\t{0} has: {1}'s data", Thread.CurrentThread.Name, newData);
		}

		static void Main(string[] args)
		{
			Console.Title = "Rendezvous Test";

			Thread t;
			for (int i = 0; i < 10; i++)
			{
				t = new Thread(ExchangeData);
				t.Name = "t" + i;
				t.Start();
			}

			Console.ReadLine();
		}
	}
}
