namespace Fold.Motor.Resources.Resolution;

public class ActionResolution
{
    public Request.ActionRequest? Request { set; get; }
    public CardColor Color { set; get; }
    public Response.GameEndedResponse? GameEndedResponse { set; get; }
    public required Response.ActionResponse AllResponse { set; get; }
    public Message? OwnerResponse { set; get; }
}