using System.Collections.Generic;
using System.Threading;

namespace ConcurrencyConstructs
{
	// Handles interruptions, but not abort/suspend
	public class Channel<T>
	{
		private Queue<T> _items = new Queue<T>();
		private Semaphore _takePerm = new Semaphore();

		public virtual void Put(T val)
		{
			Offer(val, -1);
		}

		public virtual T Take()
		{
			T val;
			Poll(-1, out val);
			return val;
		}

		public virtual bool Offer(T val, int ms)
		{
			lock (this)
				_items.Enqueue(val);
			_takePerm.ForceRelease();
			return true;
		}

		// If this returns false the value in "val" isn't valid.
		public virtual bool Poll(int ms, out T val)
		{
			if (_takePerm.TryAcquire(ms))
			{
				try
				{
					lock (this)
						val = _items.Dequeue();
				}
				catch (ThreadInterruptedException)
				{
					_takePerm.Release();
					throw;
				}
				return true;
			}
			val = default(T);
			return false;
		}
	}
}