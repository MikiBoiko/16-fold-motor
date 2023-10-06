using Fold.Motor.Resources.Resolution;

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

    public override ActionResolution DoAction(Player player, Board board) => board.Move(_from, _to);
}