namespace ConcurrencyConstructs
{
	public class Barrier
	{
		private uint _numThreads, _desiredThreads;
		private SimpleSemaphore _goPerm = new SimpleSemaphore();
		private SimpleSemaphore _turnstile = new SimpleSemaphore(1);

		public Barrier(uint desiredThreads)
		{
			_desiredThreads = desiredThreads;
		}

		public void Arrive()
		{
			_turnstile.Acquire();

			if (++_numThreads == _desiredThreads)
				_goPerm.Release(_numThreads);
			else
				_turnstile.Release();
			
			_goPerm.Acquire();
			
			lock (this)
				if (--_numThreads == 0)
					_turnstile.Release();
		}
	}
}
