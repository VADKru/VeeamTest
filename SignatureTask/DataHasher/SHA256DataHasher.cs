using System;
using System.Security.Cryptography;


namespace SignatureTask.DataHasher
{
    sealed class SHA256DataHasher : IDataHasher
    {
        private readonly SHA256Managed _sha256;

        public SHA256DataHasher()
        {
            _sha256 = new SHA256Managed();
        }

        public string GetSignature(byte[] data)
        {
            try
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                byte[] hash = _sha256.ComputeHash(data);
                foreach (byte b in hash)
                {
                    result.AppendFormat("{0:X2}", b);
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }
    }
}
