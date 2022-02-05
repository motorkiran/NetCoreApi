    public interface ILogService
    {
        void Info(string message);
        void Trace(string message);
        void Debug(string message);
        void Error(string message);
        void Fatal(string message);
    }