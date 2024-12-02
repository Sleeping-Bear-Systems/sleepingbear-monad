namespace SleepingBear.Monad.Error;

public record ValidationError(string Message, string Tag) : Core.Error
{
    
}