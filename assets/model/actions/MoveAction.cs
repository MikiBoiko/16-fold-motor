using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Actions;

public class MoveAction : Action {
    private BoardPosition _from;
    private BoardPosition _to;

    public MoveAction(BoardPosition from, BoardPosition to) {
        if(!BoardPosition.AreAdjacent(from, to))
            throw new NotAdjecentException();

        _from = from;
        _to = to;
    }

    public override ActionResolution DoAction(Player player, Board board)
	{
		if (!board.IsInbound(_to))
			throw new OutOfBoardPositionException();

		if (board.IsOccupied(_to))
			throw new PositionOccupiedException();

		CardStack stack = board.RemoveCardStack(_from);
		board.AddCardStack(_to, stack);

		return new ActionResolution
		{
			Color = stack.OwnerColor,
			AllResponse = new ActionResponse
			{
				Type = "Move",
				Data = new Dictionary<string, object?> {
					{ "from", _from.ToString() },
					{ "to", _to.ToString() }
				}
			}
		};
	}
}