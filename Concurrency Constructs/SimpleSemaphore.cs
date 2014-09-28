using System.Threading;

namespace ConcurrencyConstructs
{
	public class SimpleSemaphore 
	{
		protected uint _tokens;

		public SimpleSemaphore(uint tokens=0)
		{
			_tokens = tokens;
		}

		public void Acquire()
		{
			lock (this)
			{
				while (_tokens == 0)
					Monitor.Wait(this);
				_tokens--;
			}
		}

		public virtual void Release(uint tokens=1)
		{
			lock (this)
			{
				_tokens += tokens;
				Monitor.PulseAll(this);
			}
		}
	}
}
