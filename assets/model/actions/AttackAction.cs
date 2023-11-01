using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Actions;

public class AttackAction : Action
{
    private List<BoardPosition> _from;
    private BoardPosition _to;

    public AttackAction(List<BoardPosition> from, BoardPosition to)
    {
        if (from.Count == 0)
            throw new EmptyPositionListException();

        _from = from;
        _to = to;
    }

    public override ActionResolution DoAction(Player player, Board board)
	{
		// Find all attacking card stacks
		CardColor? actionColor = null;
		List<CardStack> attackingCardStacks = new List<CardStack>();
		foreach (BoardPosition fromPosition in _from)
		{
			CardStack attackingStack = board.FindCardStack(fromPosition);

			actionColor = actionColor == null
				? attackingStack.OwnerColor
				: actionColor != attackingStack.OwnerColor
					? CardColor.both
					: actionColor;

			attackingCardStacks.Add(attackingStack);
		}

		// Defend attack
		CardStack defendingStack = board.FindCardStack(_to);
		CardDefense defense = defendingStack.Defend(
			attackingCardStacks.ConvertAll(
				new Converter<CardStack, Card>(CardStack.ToCard)
			),
			_from,
			_to
		);

		//Console.WriteLine(String.Format("Attack: {0}, {1}", defense.topCard.value, defense.result));

		// Apply the defense result
		bool switchCard = defense.TopCard != defendingStack.Card;

		if (defense.Result == Result.draw)
			board.RemoveCardStack(_to);

		List<string> fromStringList = new();

		foreach (BoardPosition fromPosition in _from)
		{
			if (defense.Result != Result.draw)
			{
				CardStack attackingStack = board.FindCardStack(fromPosition);
				if (switchCard && attackingStack.Card == defense.TopCard)
				{
					attackingStack.MergeUnder(defendingStack);
					board.RemoveCardStack(_to);
					board.AddCardStack(_to, attackingStack);
					switchCard = false;

				}
				else
				{
					defendingStack.MergeUnder(attackingStack);
				}
			}
			fromStringList.Add(fromPosition.ToString());
			board.RemoveCardStack(fromPosition);
		}

		return new ActionResolution
		{
			Color = actionColor ?? CardColor.both,
			GameEndedResponse = (board.PlayerCardStackCount[0] > 0 && board.PlayerCardStackCount[1] > 0) ?
						null
				:
				(board.PlayerCardStackCount[0] > 0) ?
						new GameEndedResponse
						{
							Way = GameEndedResponse.Reason.MATERIAL,
							Result = CardColor.red
						}
						:
						board.PlayerCardStackCount[1] > 0 ?
								new GameEndedResponse
								{
									Way = GameEndedResponse.Reason.MATERIAL,
									Result = CardColor.black
								}
								:
								new GameEndedResponse
								{
									Way = GameEndedResponse.Reason.MATERIAL,
									Result = CardColor.both
								},
			AllResponse = new ActionResponse
			{
				Type = "Attack",
				Data = new Dictionary<string, object?>
				{
					{ "from", fromStringList },
					{ "to", _to.ToString() },
					{ "defense", defense },
				}
			}
		};
	}
}