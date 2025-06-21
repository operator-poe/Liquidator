namespace Liquidator;

public enum LogLevel
{
    None,
    Error,
    Warning,
    Debug,
};

public class Log
{
    private static LogLevel LogLevel
    {
        get
        {
            switch (Liquidator.Instance.Settings.LogLevel)
            {
                case "Debug":
                    return LogLevel.Debug;
                case "Warning":
                    return LogLevel.Warning;
                case "Error":
                    return LogLevel.Error;
                default:
                    return LogLevel.None;
            }
        }
    }
    public static void Debug(string message)
    {
        if (LogLevel < LogLevel.Debug)
            return;
        Liquidator.Instance.LogMsg($"Liquidator: {message}");
    }

    public static void Warning(string message)
    {
        if (LogLevel < LogLevel.Warning)
            return;
        Liquidator.Instance.LogMsg($"Liquidator: {message}");
    }

    public static void Error(string message)
    {
        if (LogLevel < LogLevel.Error)
            return;
        Liquidator.Instance.LogError($"Liquidator: {message}");
    }

}