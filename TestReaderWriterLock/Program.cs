using System;
using System.Threading;
using jamesCC = ConcurrencyConstructs;

namespace TestReaderWriterLock
{
	class Program
	{
		private static jamesCC.ReaderWriterLock _fileLock = new jamesCC.ReaderWriterLock();
		private static Random _rand = new Random();

		private static void Read()
		{
			_fileLock.ReaderAcquire();
			Console.WriteLine("Thread {0} reading...", Thread.CurrentThread.Name);
			Thread.Sleep(_rand.Next(500, 3000));
			_fileLock.ReaderRelease();
		}

		private static void Write()
		{
			Console.WriteLine("Writer {0} is waiting", Thread.CurrentThread.Name);
			_fileLock.WriterAcquire();
			Console.WriteLine("\tThread {0} writing...", Thread.CurrentThread.Name);
			Thread.Sleep(_rand.Next(500, 3000));
			_fileLock.WriterRelease();
		}

		private static void ReadWriteRandomly()
		{
			while (true)
			{
				if (_rand.Next(5) < 1)
					Write();
				else
					Read();
			}
		}

		static void Main(string[] args)
		{
			Console.Title = "ReaderWriterLock Test";

			Thread t;
			for (int i = 0; i < 10; i++)
			{
				t = new Thread(ReadWriteRandomly);
				t.Name = "t" + i;
				t.Start();
			}

			Console.ReadLine();
		}
	}
}
