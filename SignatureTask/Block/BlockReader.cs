using System.IO;

namespace SignatureTask.Block
{
    sealed class BlockReader
    {
        private readonly int _blockSize;
        private readonly FileStream _fileStream;
        private static uint _counter = 0;

        public BlockReader (int blockSize, FileStream fileStream)
        {
            _blockSize = blockSize;
            _fileStream = fileStream;
        }

        public Block GetBlock()
        {
            byte[] data = new byte[_blockSize];
            _fileStream.Read(data, 0, _blockSize);
            Block block = new Block
            {
                Id = _counter++,
                Data = data
            };
            return block;
        }
    }
}
