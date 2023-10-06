using Fold.Motor.Resources.Resolution;

namespace Fold.Motor.Model;

public abstract class Decision
{
    public Decision() { }

    public abstract DecisionResolution DoDecision(Player player, Game game);
}