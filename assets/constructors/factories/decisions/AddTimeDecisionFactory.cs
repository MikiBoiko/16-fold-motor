using Fold.Motor.Model.Decisions;

namespace Fold.Motor.Constructors.Factories.Decisions;

public class AddTimeDecisionFactory : IFactory<Model.Decision>
{
    public Model.Decision Instantiate(Dictionary<string, object?> data)
    {
        return new AddTimeDecision();
    }
}