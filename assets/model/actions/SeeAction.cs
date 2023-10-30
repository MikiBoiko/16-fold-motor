using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Actions;

public class SeeAction : Action
{
    private BoardPosition _from;

    public SeeAction(BoardPosition from)
    {
        _from = from;
    }

    public override ActionResolution DoAction(Player player, Board board) 
	{
		CardStack stack = board.FindCardStack(_from);

		if (!stack.IsHidden)
			throw new CardNotHiddenException();

		Card card = stack.Card;

		return new ActionResolution
		{
			Color = card.color,
			OwnerResponse = new ActionResponse
			{
				Type = "See",
				Data = new Dictionary<string, object?>
				{
					{ "player", player.color },
					{ "position", _from.ToString() },
					{ "card", card.State }
				}
			},
			AllResponse = new ActionResponse
			{
				Type = "See",
				Data = new Dictionary<string, object?>
				{
					{ "player", player.color },
					{ "position", _from.ToString() }
				}
			},
		};
	}

}