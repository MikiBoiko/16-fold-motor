namespace Fold.Motor.Resources.Response;

public class GameStartedResponse
{
    public required CardColor Turn { set; get; }
    public required List<int> PickedCards { set; get; }
}

public class GameEndedResponse
{
    public enum Reason
    {
        AGREED = 0,
        PASSING = 1,
        MATERIAL = 2,
        TIME = 3,
        REPORT = 4,
        ILLEGAL = 5
    }

    public Reason Way { set; get; }
    public CardColor Result { set; get; }
}