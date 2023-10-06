using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Decisions;

public class AddTimeDecision : Decision
{
    public override DecisionResolution DoDecision(Player player, Game game)
    {
        // TODO : for now hardcoded time added on decision
        double increment = 15000;
        Player other = game.OtherPlayer(player);
        other.Timer.AddTime(increment);

        DecisionResponse response = new DecisionResponse {
            Type = "AddTime",
            Data = new Dictionary<string, object?> {
                { "color", player.color },
                { "dueTime", other.Timer.DueTime }
            }
        };

        return new DecisionResolution {
            Color = other.color, 
            Response = response
        };
    }
}

public class NegativeTimeException : Exception { }