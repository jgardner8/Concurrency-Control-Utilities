using System;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrencyConstructs
{
    public class FifoSemaphore
    {
        protected uint _tokens;
        private Queue<Object> _lockQueue = new Queue<Object>();
		private SimpleSemaphore _turnstile = new SimpleSemaphore(1); //general locking
		private SimpleSemaphore _tStopReleaseOvertake = new SimpleSemaphore(1); //makes sure threads that have been released, 
						//but are just waiting on _turnstile, don't get overtaken by other released threads (violating FIFO)
															
        public FifoSemaphore(uint tokens = 0)
        {
            _tokens = tokens;
        }

        public void Acquire()
        {
            _turnstile.Acquire();
            while (_tokens == 0)
			{
				Object locker = new Object();
				_lockQueue.Enqueue(locker);
                lock (locker)
				{
					_turnstile.Release();
					Monitor.Wait(locker);
					_tStopReleaseOvertake.Acquire(); //claim first release
					_turnstile.Acquire();
					_tStopReleaseOvertake.Release(); //acquired turnstile before any other released threads 
				}
			}
            _tokens--;
			_turnstile.Release();
        }

		//If you release more than one token, strict FIFO is not guaranteed.
		//For example, if you release 3 tokens, the first 3 threads in will
		//be released (which is correct), but those 3 threads may not release 
		//in FIFO order amongst themselves.
        public virtual void Release(uint tokens=1)
        {
			_turnstile.Acquire();
            _tokens += tokens;
			long numToPulse = Math.Min(tokens, _lockQueue.Count);
			for (long i = 0; i < numToPulse; i++) 
			{
				Object temp = _lockQueue.Dequeue();
				lock (temp)
					Monitor.Pulse(temp);		
			}
			_turnstile.Release();
        }
    }
}
