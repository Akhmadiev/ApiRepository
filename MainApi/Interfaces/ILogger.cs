namespace MainApi.Interfaces
{
    public interface ILogger
    {
        string LogInfoPath { get; }

        string LogErrorPath { get; }

        void LogInfo(string msg);

        void LogError(string msg);
    }
}
