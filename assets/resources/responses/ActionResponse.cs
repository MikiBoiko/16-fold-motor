namespace Fold.Motor.Resources.Response;

public class ActionResponse : Message
{
    public double TimeLeft { get; set; }
    public DateTime TimeStamp { get => DateTime.Now; } 
}