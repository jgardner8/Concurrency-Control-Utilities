using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jamesCC = ConcurrencyConstructs;

namespace TestLightswitch
{
	class Program
	{
		private static jamesCC.SimpleSemaphore _goPerm = new jamesCC.SimpleSemaphore(1);
		private static jamesCC.Lightswitch _lightswitch = new jamesCC.Lightswitch(_goPerm);
		private static Random _rand = new Random();

		private static void EnterExitRoom()
		{
			_lightswitch.Acquire();
			Console.WriteLine("Thread {0} has arrived. Threads = {1}", Thread.CurrentThread.Name, _lightswitch.NumThreads);
			Thread.Sleep(_rand.Next(500, 7500));
			_lightswitch.Release();
			Console.WriteLine("\tThread {0} has left. Threads = {1}", Thread.CurrentThread.Name, _lightswitch.NumThreads);
		}

		static void Main(string[] args)
		{
			Console.Title = "Lightswitch Test";

			Thread t;
			for (int i = 0; i < 10; i++)
			{
				t = new Thread(EnterExitRoom);
				t.Name = "t" + i;
				t.Start();
			}

			Thread.Sleep(1000);

			_goPerm.Acquire();
			Console.WriteLine("Main thread got past the room! (shouldn't happen until everyone leaves)");

			Console.ReadLine();
		}
	}
}
