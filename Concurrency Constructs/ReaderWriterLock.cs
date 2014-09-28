namespace ConcurrencyConstructs
{
	public class ReaderWriterLock
	{
		private SimpleSemaphore _writePerm = new SimpleSemaphore(1);
		private Lightswitch _readPerm;
		private SimpleSemaphore _readTurnstile = new SimpleSemaphore(1);
		private SimpleSemaphore _writeTurnstile = new SimpleSemaphore(1);

		public ReaderWriterLock()
		{
			_readPerm = new Lightswitch(_writePerm);
		}

		public void ReaderAcquire()
		{
			_readTurnstile.Acquire();
			_readTurnstile.Release();
			_readPerm.Acquire();
		}

		public void ReaderRelease()
		{
			_readPerm.Release();
		}

		public void WriterAcquire()
		{
			_writeTurnstile.Acquire();
			_readTurnstile.Acquire();
			_writePerm.Acquire();
			_readTurnstile.Release();
		}

		public void WriterRelease()
		{
			_writePerm.Release();
			_writeTurnstile.Release();
		}
	}
}
