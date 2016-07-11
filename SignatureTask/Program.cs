using System;
using System.IO;
using SignatureTask.DataDumper;
using SignatureTask.DataHasher;

namespace SignatureTask
{
    class Program
    {
        private static Controller _executor;
        private static FileStream _fileStream;
        private static uint _blockSize;
        private const byte ARGS_WORK_SIZE = 2;
        /// <summary>
        /// args[0]:file path
        /// args[1]:block size
        /// </summary>
        static void Main(string[] args)
        {
            try
            {

                if (args.Length != ARGS_WORK_SIZE)
                    throw new Exception("command line must takes two args: file path and block size");
                
                _blockSize = TryCheckBlockSize(args[1]);
                _fileStream = TryOpenStream(args[0]);

                _executor = new Controller(_fileStream, _blockSize, HashType.SHA256, DumperType.Queue);
                _executor.Start();

                Console.WriteLine("\n\tEnd");
                CloseStream(_fileStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                return;
            }

            Console.ReadLine();
        }
        private static FileStream OpenStream(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return fileStream;
        }
        private static void CloseStream(FileStream filesStream)
        {
            filesStream.Dispose();
            filesStream.Close();
        }
        private static uint TryCheckBlockSize(string arg)
        {
            uint blockSize = 0;
            try
            {
                blockSize = (uint)(Int32.Parse(arg));
                if (blockSize <= 0)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nSecond argument - block size: have to positive integer\n" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            return blockSize;
        }
        private static FileStream TryOpenStream(string arg)
        {
            string filePath = arg;
            FileStream fileStream = null;
            try
            {
                filePath = arg;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFirst argument - File path is empty\n" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            try
            {
                fileStream = OpenStream(filePath);
                if (fileStream.Length == 0)
                    Console.WriteLine("\nFile size is 0");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("\nFile path error:" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (FileLoadException ex)
            {
                Console.WriteLine("\nLoad error:\n" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message + "\n" + ex.StackTrace);
                Console.ReadLine();
                Environment.Exit(0);
            }
            return fileStream;
        }
    }
}
