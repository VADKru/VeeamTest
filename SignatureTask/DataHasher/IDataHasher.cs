namespace SignatureTask.DataHasher
{
    public interface IDataHasher
    {
        string GetSignature(byte[] data);
    }
}
