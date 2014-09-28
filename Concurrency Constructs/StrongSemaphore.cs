using System;

namespace ConcurrencyConstructs
{
	public class StrongSemaphore
	{
		private uint _room1 = 0, _room2 = 0;
		private Semaphore _t1, _t2 = new Semaphore(0);
		private Object _lockRoom2 = new Object(); //lock(this) used to lock room1

		public StrongSemaphore(uint tokens=0)
		{
			_t1 = new Semaphore(tokens);			
		}

		public void Acquire()
		{
			lock (this)
				_room1++;

			_t1.Acquire();
			_room2++;
			lock (this)
			{
				_room1--;

				if (_room1 == 0)
					_t2.Release();
				else
					_t1.Release();
			}

			_t2.Acquire();
			lock (_lockRoom2)
				_room2--;
		}

		public void Release(uint tokens=1)
		{
			lock (_lockRoom2)
			{
				if (_room2 == 0)
					_t1.Release();
				else
					_t2.Release();
			} 
		}
	}
}
