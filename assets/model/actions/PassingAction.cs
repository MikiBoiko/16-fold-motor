using Fold.Motor.Resources.Resolution;

namespace Fold.Motor.Model.Actions;

public class PassingAction : Action {
    private BoardPosition _from;
    
    public PassingAction(BoardPosition from) {
        _from = from;
    }
    
    public override ActionResolution DoAction(Player player, Board board) => board.Passing(player.color, _from);
}