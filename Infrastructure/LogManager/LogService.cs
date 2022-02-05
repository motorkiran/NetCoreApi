using Sentry;

public class LogService : ILogService
{
    public LogService()
    {

    }

    public void Info(string message)
    {
        using (SentrySdk.Init(o =>
        {
            o.Dsn = Constants.SentryDsn;
        }))
        {
            SentrySdk.CaptureMessage(message, SentryLevel.Info);
        }
    }

    public void Trace(string message)
    {
        using (SentrySdk.Init(o =>
        {
            o.Dsn = Constants.SentryDsn;
        }))
        {
            SentrySdk.CaptureMessage(message, SentryLevel.Warning);
        }
    }

    public void Debug(string message)
    {
        using (SentrySdk.Init(o =>
        {
            o.Dsn = Constants.SentryDsn;
        }))
        {
            SentrySdk.CaptureMessage(message, SentryLevel.Debug);
        }
    }

    public void Error(string message)
    {
        using (SentrySdk.Init(o =>
        {
            o.Dsn = Constants.SentryDsn;
        }))
        {
            SentrySdk.CaptureMessage(message, SentryLevel.Error);
        }
    }

    public void Fatal(string message)
    {
        using (SentrySdk.Init(o =>
        {
            o.Dsn = Constants.SentryDsn;
        }))
        {
            SentrySdk.CaptureMessage(message, SentryLevel.Fatal);
        }
    }
}