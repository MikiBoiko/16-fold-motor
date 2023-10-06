using Fold.Motor.Resources.Resolution;

namespace Fold.Motor.Model;

public abstract class Action
{
    public abstract ActionResolution DoAction(Player player, Board board);
}

public class EmptyPositionListException : Exception { }