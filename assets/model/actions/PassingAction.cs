using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Actions;

public class PassingAction : Action
{
    private BoardPosition _from;

    public PassingAction(BoardPosition from)
    {
        _from = from;
    }

    public override ActionResolution DoAction(Player player, Board board)
    {
        if (!board.IsInbound(_from))
            throw new OutOfBoardPositionException();

        if (player.color == CardColor.red)
        {
            if (_from.y != Board.SIZE_Y - 1)
                throw new CardNotCloseToWinningException();
        }
        else if (player.color == CardColor.black)
        {
            if (_from.y != 0)
                throw new CardNotCloseToWinningException();
        }

        CardStack passingStack = board.FindCardStack(_from);

        return new ActionResolution
        {
            Color = passingStack.OwnerColor,
            GameEndedResponse = passingStack.OwnerColor == player.color ?
                        new GameEndedResponse
                        {
                            Way = GameEndedResponse.Reason.PASSING,
                            Result = player.color
                        }
                        :
                        new GameEndedResponse
                        {
                            // TODO : is this a good method to change color? maybe generalize it
                            Way = GameEndedResponse.Reason.REPORT,
                            Result = (CardColor)(((int)player.color + 1) % 2)
                        },
            AllResponse = new ActionResponse
            {
                Type = "Passing",
                Data = new Dictionary<string, object?> {
                    { "color", passingStack.OwnerColor },
                    { "from", _from.ToString() },
                    { "card", passingStack.Card.value }
                }
            }
        };
    }
}