namespace ConcurrencyConstructs
{
	public class Lightswitch
	{
		private int _numThreads;
		private SimpleSemaphore _goPerm;

		public Lightswitch(SimpleSemaphore perm)
		{
			_goPerm = perm;
		}

		public void Acquire()
		{
			lock (this)
				if (_numThreads++ == 0)
					_goPerm.Acquire();
		}

		public void Release()
		{
			lock (this)
				if (--_numThreads == 0)
					_goPerm.Release();
		}

		public int NumThreads
		{
			get { lock(this) return _numThreads; }
		}
	}
}
