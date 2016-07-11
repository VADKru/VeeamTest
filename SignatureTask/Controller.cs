using System;
using System.IO;
using SignatureTask.DataDumper;
using SignatureTask.DataHasher;

namespace SignatureTask
{
    /// <summary>
    /// The main class of the program
    /// </summary>
    sealed class Controller
    {
        private const uint PlacesPerThread = 4;

        private readonly uint _blockSize;
        private readonly FileStream _fileStream;

        private readonly IDataDumper<Block.Block> _dataDumper;

        private readonly DataHandler _dataHandler;
        private readonly DataReader _dataReader;

        public Controller(FileStream fileStream, uint blockSize, HashType hashType, DumperType dumperType)
        {
            _fileStream = fileStream;
            _blockSize = blockSize;
            _dataDumper = SimpleDumperFactory.GetDumper(dumperType, (uint) Environment.ProcessorCount*PlacesPerThread);
            _dataHandler = new DataHandler(Environment.ProcessorCount, _dataDumper, hashType);
            _dataReader = new DataReader(_fileStream, _blockSize, _dataDumper);
        }
        public void Start()
        {
            Console.WriteLine("\nThe number of threads: {0}", Environment.ProcessorCount);
            Console.WriteLine(
                "\nThe selected file:\n\t" + _fileStream.Name + "\nfile size: \n\t"
                + _fileStream.Length + "byte" + "\ndivided to blocks of size\n\t {0} byte\n\tBegin", _blockSize);

            _dataReader.Start();
            _dataHandler.Start();

            _dataReader.Stop();
            _dataHandler.Stop();
        }
    }
}
