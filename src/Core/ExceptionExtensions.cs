namespace SleepingBear.Monad.Core;

/// <summary>
///     Extensions methods for <see cref="Exception" /> classes.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    ///     Checks if the supplied exception is fatal and terminates the program.
    /// </summary>
    public static Exception FailFastIfCritical(this Exception exception, string? message)
    {
        switch (exception)
        {
            case OutOfMemoryException:
            case AccessViolationException:
            case StackOverflowException:
            case AppDomainUnloadedException:
            case BadImageFormatException:
                Environment.FailFast(message ?? "A critical exception was thrown.", exception);
                break;
        }

        return exception;
    }
}