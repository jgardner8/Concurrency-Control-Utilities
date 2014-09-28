namespace ConcurrencyConstructs
{
	public class Rendezvous<T>
	{
		private T _data;
		private bool _first = true;
		private SimpleSemaphore _firstGoPerm = new SimpleSemaphore();
		private SimpleSemaphore _turnstile = new SimpleSemaphore(1);

		public T Exchange(T toExchange)
		{
			_turnstile.Acquire();
			lock (this)
			{
				if (_first) 
				{
					_first = false;
					_data = toExchange;
					_turnstile.Release();
				}
				else
				{
					T result = _data;
					_data = toExchange;
					_firstGoPerm.Release();
					return result;
				}
			}
			_firstGoPerm.Acquire();
			_first = true;
			lock (this)
			{
				_turnstile.Release();
				return _data;
			}
		}
	}
}
