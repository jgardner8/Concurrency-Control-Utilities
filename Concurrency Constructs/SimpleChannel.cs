using System.Collections.Generic;

namespace ConcurrencyConstructs
{
	public class SimpleChannel<T>
	{
		private Queue<T> _items = new Queue<T>();
		private SimpleSemaphore _takePerm = new SimpleSemaphore();

		public virtual void Put(T toPut)
		{
			lock (this)
				_items.Enqueue(toPut);
			_takePerm.Release();
		}

		public virtual T Take()
		{
			_takePerm.Acquire();
			lock (this) 
				return _items.Dequeue();
		}
	}
}
