namespace MainApi
{
    using MainApi.Interfaces;
    using System.IO;

    public class Logger : ILogger
    {
        public string LogInfoPath => $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs\\api.txt";

        public string LogErrorPath => $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs\\api_error.txt";

        public void LogError(string msg)
        {
            using (var stream = new StreamWriter(LogErrorPath, true))
            {
                stream.WriteLine(msg);
            }
        }

        public void LogInfo(string msg)
        {
            using (var stream = new StreamWriter(LogInfoPath, true))
            {
                stream.WriteLine(msg);
            }
        }
    }
}
