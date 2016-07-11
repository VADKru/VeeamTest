using System;
using System.Linq;
using System.Threading;
using SignatureTask.DataDumper;
using SignatureTask.DataHasher;

namespace SignatureTask
{
    /// <summary>
    /// This class is designed for data processing
    /// Implements reading in a separate thread
    /// Taking from the dumper QueueDataDumper.EnqueueBlock
    /// </summary>
    sealed class DataHandler
    {
        private readonly Thread[] _workers;
        private readonly HashType _hashType;

        private readonly IDataDumper<Block.Block> _dataDumper;
        public DataHandler(int threadsNumber, IDataDumper<Block.Block> dataDumper, HashType hashType)
        {
            _dataDumper = dataDumper;
            _hashType = hashType;

            _workers = new Thread[threadsNumber];
            for (int i = 0; i < threadsNumber; i++)
            {
                int buf = i;
                _workers[i] = new Thread(() => BlockHandling(buf.ToString()));
                _workers[i].Priority = ThreadPriority.Lowest;
            }
        }
        public void Start()
        {
            for (int i = 0; i < _workers.Count(); i++)
            {
                _workers[i].IsBackground = true;
                _workers[i].Start();
            }
        }
        public void Stop()
        {
            for (int i = 0; i < _workers.Length; i++)
                _dataDumper.EnqueueBlock(null);
            foreach (Thread w in _workers)
                w.Join();
        }

        public void BlockHandling(Object obj)
        {
            IDataHasher dataHasher = SimpleHasherFactory.GetHasher(_hashType);
            while (true)
            {
                try
                {
                    bool isContinue = true;
                    Block.Block block = _dataDumper.DequeueBlock(ref isContinue);
                    if (block != null)
                    {
                        string result = dataHasher.GetSignature(block.Data);
                        //Console.WriteLine("\nThread number:" + obj.ToString());
                        Console.WriteLine("\nID: {0},\tSignature: {1}", block.Id, result);
                    }
                    if (isContinue == false)
                        return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nHandle error:\n" + ex.Message + "\n" + ex.StackTrace);
                    return;
                }
            }

        }
    }
}
