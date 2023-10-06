namespace Fold.Motor.Resources.Resolution;

public class DecisionResolution
{
    public Request.DecisionRequest? Request { set; get; }
    public CardColor Color { set; get; }
    public Response.GameEndedResponse? Resolution { set; get; }
    public Response.DecisionResponse? Response { set; get; }
}