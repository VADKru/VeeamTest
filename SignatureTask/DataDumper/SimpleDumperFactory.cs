using System;

namespace SignatureTask.DataDumper
{
    static class SimpleDumperFactory
    {
        public static IDataDumper<Block.Block> GetDumper(DumperType dumperType, uint size)
        {
            switch (dumperType)
            {
                case DumperType.Queue:
                    return new QueueDataDumper(size);
                default:
                    throw new Exception("Unknown dumper type");
            }
        }
    }
}
