using Fold.Motor.Resources.Resolution;

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

    public override ActionResolution DoAction(Player player, Board board) => board.Attack(_from, _to);
}