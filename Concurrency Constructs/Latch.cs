namespace ConcurrencyConstructs
{
	public class Latch
	{
		private SimpleSemaphore _goPerm = new SimpleSemaphore();

		public void Wait()
		{
			_goPerm.Acquire();
			_goPerm.Release();
		}

		public void Release()
		{
			_goPerm.Release();
		}

		public void Reset()
		{
			_goPerm = new SimpleSemaphore();
		}
	}
}
