using System;
using System.Threading;

namespace ConcurrencyConstructs
{
	public class Semaphore 
	{
		protected uint _tokens;
		private uint _waitingThreads;

		public Semaphore(uint tokens=0)
		{
			_tokens = tokens;
		}

		public void Acquire()
		{
			TryAcquire(-1);
		}

		public bool TryAcquire(int ms)
		{
			DateTime startTime = DateTime.Now;
			TimeSpan alreadyWaited = TimeSpan.Zero;
			TimeSpan toWait = new TimeSpan(0, 0, 0, 0, ms);
			lock (this)
			{
				while (_tokens == 0)
				{
					_waitingThreads++;
					try
					{
						if (!Monitor.Wait(this, toWait - alreadyWaited)) 
						{	//Timed out
							if (_tokens > 0) //if I got a token, pulsed at the same time as timeout
								break;
							return false;
						}
						if (ms > 0)
						{
							alreadyWaited = DateTime.Now - startTime;
							if (alreadyWaited >= toWait) //else redo loop, wait for difference
								return false;	
						}
					}
					catch (ThreadInterruptedException)
					{
						if (_tokens > 0)
							Monitor.Pulse(this); //I took someone's pulse
						throw;
					}
					finally
					{
						_waitingThreads--;	
					}
				}
				_tokens--;
				return true;
			}
		}

		public virtual void Release(uint tokens=1)
		{
			lock (this)
			{
				_tokens += tokens;
				if (_waitingThreads > 0 && _waitingThreads <= tokens)
					Monitor.PulseAll(this);
				else
					for (int i = 0; i < _waitingThreads; i++)
						Monitor.Pulse(this);
			}
		}

		public void ForceRelease(uint tokens=1)
		{
			ThreadInterruptedException ex = null;
			while (true)
			{
				try
				{
					Release(tokens);
					if (ex != null)
						throw ex;
					return;
				}
				catch (ThreadInterruptedException e)
				{
					ex = e;
				}
			}
		}
	}
}