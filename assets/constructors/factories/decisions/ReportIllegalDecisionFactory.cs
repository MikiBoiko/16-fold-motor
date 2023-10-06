using Fold.Motor.Model.Decisions;

namespace Fold.Motor.Constructors.Factories.Decisions;

public class ReportIllegalDecisionFactory : IFactory<Model.Decision>
{
    public Model.Decision Instantiate(Dictionary<string, object?> data)
    {
        return new ReportIllegalDecision();
    }
}