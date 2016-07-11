using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Implementation of the dumper by queue base
/// </summary>
namespace SignatureTask.DataDumper
{
    sealed class QueueDataDumper : IDataDumper<Block.Block>
    {
        private readonly uint _queueSize;
        private readonly Queue<Block.Block> _inputQueue;

        private readonly object _mutex;
        private readonly EventWaitHandle _enqueueWaitHandler;
        private readonly EventWaitHandle _dequeueWaitHandler;

        public QueueDataDumper(uint queueSize)
        {
            _queueSize = queueSize;
            _mutex = new object();
            _inputQueue = new Queue<Block.Block>();
            _enqueueWaitHandler = new EventWaitHandle(false, EventResetMode.AutoReset);
            _dequeueWaitHandler = new EventWaitHandle(false, EventResetMode.AutoReset);
        }


        public void EnqueueBlock(Block.Block block)
        {
            bool isQueueFull;
            lock (_mutex)
            {
                isQueueFull = _inputQueue.Count >= _queueSize;
            }

            if (isQueueFull)
                _enqueueWaitHandler.WaitOne();

            lock (_mutex)
            {
                //Console.WriteLine("\n Added, queue length: " + _inputQueue.Count.ToString());
                _inputQueue.Enqueue(block);
            }
            _dequeueWaitHandler.Set();
        }

        /// <summary>
        /// isContinue - false: data are over
        /// isContinue - true: data arrive or will arrive
        /// isContinue = false, if added null - abort thread
        /// </summary>
        public Block.Block DequeueBlock(ref bool isContinue)
        {
            Block.Block block = null;
            lock (_mutex)
            {
                if (_inputQueue.Count > 0)
                {
                    //Console.WriteLine("\n Took, queue length: " + _inputQueue.Count.ToString());
                    block = _inputQueue.Dequeue();
                    if (block == null)
                    {
                        isContinue = false;
                        return null;
                    }
                }
            }
            _enqueueWaitHandler.Set();

            if (block != null)
            {
                isContinue = true;
                return block;
            }
            else
            {
                _dequeueWaitHandler.WaitOne();
                isContinue = true;
                return null;
            }
            
        }
    }
}
