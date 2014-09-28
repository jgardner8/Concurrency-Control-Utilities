using System;

namespace ConcurrencyConstructs
{
	public class Mutex : Semaphore
	{
		public Mutex() : base(1)
		{ }

		public override void Release(uint tokens=1)
		{
			if (tokens != 1)
				throw new ArgumentException("can only throw 1 token");
			lock (this)
			{
				if (_tokens == 0)
					base.Release(1);
				else
					throw new InvalidOperationException("mutex already released");
			}
		}
	}
}
