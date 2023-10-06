using Fold.Motor.Resources.Resolution;

namespace Fold.Motor.Model.Actions;

public class SeeAction : Action
{
    private BoardPosition _from;

    public SeeAction(BoardPosition from)
    {
        _from = from;
    }

    public override ActionResolution DoAction(Player player, Board board) => board.See(player.color, _from);
}