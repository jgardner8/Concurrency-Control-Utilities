using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace ChannelTest
{
	class Program
	{
		private static jamesCC.BoundedChannel<int> _channel = new jamesCC.BoundedChannel<int>(3);
		private static Random _rand = new Random();

		private static void Produce()
		{
			int randomData;
			while (true)
			{
				randomData = _rand.Next(100);
				if (_channel.Offer(randomData, 5000))
					Console.WriteLine("{0} put {1}", Thread.CurrentThread.Name, randomData);
				else
					Console.WriteLine("{0} gave up", Thread.CurrentThread.Name);
			}
		}

		private static void Consume()
		{
			int consumed;
			while (true)
			{
				consumed = _channel.Take();
				Console.WriteLine("\t {0} took {1}", Thread.CurrentThread.Name, consumed);
				Thread.Sleep(_rand.Next(2000, 8000));
			}
		}

		static void Main(string[] args)
		{
			Console.Title = "Bounded Channel Test";

			Thread[] producers = new Thread[5];
			Thread[] consumers = new Thread[2];

			for (int i = 0; i < producers.Length; i++)
			{
				producers[i] = new Thread(Produce);
				producers[i].Name = "P" + i;
				producers[i].Start();
			}

			for (int i = 0; i < consumers.Length; i++)
			{
				consumers[i] = new Thread(Consume);
				consumers[i].Name = "C" + i;
				consumers[i].Start();
			}
		}
	}
}
