using System;
using System.IO;
using System.Threading;
using SignatureTask.Block;
using SignatureTask.DataDumper;

namespace SignatureTask
{
    /// <summary>
    /// This class is designed to read data
    /// Implements reading in a separate thread
    /// To record in the dumper using QueueDataDumper.EnqueueBlock
    /// </summary>
    sealed class DataReader
    {
        private readonly Thread _worker;

        private readonly FileStream _fileStream;

        private readonly BlockReader _blockReader;

        private readonly IDataDumper<Block.Block> _dataDumper;
        public DataReader(FileStream fileStream, uint blockSize, IDataDumper<Block.Block> dataDumper)
        {
            _dataDumper = dataDumper;
            _fileStream = fileStream;

            _blockReader = new BlockReader((int)blockSize, _fileStream);

            _worker = new Thread(ReadStream);
            _worker.Priority = ThreadPriority.AboveNormal;
        }
        public void Start()
        {
            _worker.Start();
        }
        public void Stop()
        {
            _worker.Join();
        }
        private void ReadStream()
        {
            while (_fileStream.Position < _fileStream.Length)
            {
                try
                {
                    _dataDumper.EnqueueBlock(_blockReader.GetBlock());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nRead error:\n" + ex.Message + "\n" + ex.StackTrace);
                    return;
                }
            }
        }
    }
}
