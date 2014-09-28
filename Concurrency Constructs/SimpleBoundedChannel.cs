namespace ConcurrencyConstructs
{
	public class SimpleBoundedChannel<T> : SimpleChannel<T>
	{
		private SimpleSemaphore _putPerm;

		public SimpleBoundedChannel(uint capacity) : base()
		{
			_putPerm = new SimpleSemaphore(capacity);
		}

		public override void Put(T toPut)
		{
			_putPerm.Acquire();
			base.Put(toPut);
		}

		public override T Take()
		{
			T toReturn = base.Take();
			_putPerm.Release();
			return toReturn;
		}
	}
}
