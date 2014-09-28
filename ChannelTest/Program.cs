using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace ChannelTest
{
	class Program
	{
		private static jamesCC.Channel<int> _channel = new jamesCC.Channel<int>();
		private static Random _rand = new Random();

		private static void Produce()
		{
			int randomData;
			while (true)
			{
				Thread.Sleep(_rand.Next(3000, 15000));
				randomData = _rand.Next(100);
				_channel.Put(randomData);
				Console.WriteLine("{0} put {1}", Thread.CurrentThread.Name, randomData);
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
			Console.Title = "Channel Test";

			Thread[] producers = new Thread[5];
			Thread[] consumers = new Thread[2];
 
			for (int i = 0; i < producers.Length; i++)
			{
				producers[i] = new Thread(Produce);
				producers[i].Name = "P " + i;
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
