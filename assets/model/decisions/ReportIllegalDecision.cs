using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model.Decisions;

public class ReportIllegalDecision : Decision
{
    public override DecisionResolution DoDecision(Player player, Game game)
    {
        Player other = game.OtherPlayer(player);
        GameEndedResponse resolution = other.DidAnIllegalAction ?
            new GameEndedResponse{
                Way = GameEndedResponse.Reason.ILLEGAL,
                Result = player.color
            }
            :
            new GameEndedResponse {
                Way = GameEndedResponse.Reason.REPORT,
                Result = other.color
            };

        game.SetGameEnded(resolution);

        DecisionResponse response = new DecisionResponse
        {
            Type = "ReportIllegal",
            Data = new Dictionary<string, object?> {
                { "color", player.color },
                { "result", resolution.Result }
            }
        };

        return new DecisionResolution
        {
            Color = player.color,
            Resolution = resolution,
            Response = response
        };
    }
}