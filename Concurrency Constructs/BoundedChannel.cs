namespace ConcurrencyConstructs
{
	public class BoundedChannel<T> : Channel<T>
	{
		private Semaphore _putPerm;

		public BoundedChannel(uint capacity) : base()
		{
			_putPerm = new Semaphore(capacity);
		}

		public override bool Offer(T val, int ms)
		{
			if (_putPerm.TryAcquire(ms))
			{
				base.Offer(val, -1);
				return true;	
			}	
			return false;
		}

		public override bool Poll(int ms, out T val)
		{
			if (base.Poll(ms, out val))
			{
				_putPerm.Release();
				return true;
			}
			return false;
		}
	}
}
