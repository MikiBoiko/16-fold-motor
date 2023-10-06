namespace Fold.Motor.Resources;

public abstract class Message
{
    public required string Type { set; get; }
    public required Dictionary<string, Object?> Data { set; get; }
}