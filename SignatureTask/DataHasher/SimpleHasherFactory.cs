using System;

namespace SignatureTask.DataHasher
{
    static class SimpleHasherFactory
    {
        public static IDataHasher GetHasher(HashType hashType)
        {
            switch (hashType)
            {
                case HashType.SHA256:
                    return new SHA256DataHasher();
                default:
                    throw new Exception("Unknown hasher type");
            }
        }
    }
}
