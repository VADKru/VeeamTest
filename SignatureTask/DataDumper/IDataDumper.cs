/// <summary>
/// Interface classes for dumpers
/// </summary>
namespace SignatureTask.DataDumper
{
    public interface IDataDumper<T>
    {
        void EnqueueBlock(T block);
        T DequeueBlock(ref bool isContinue);
    }
}
