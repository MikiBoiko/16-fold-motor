using Fold.Motor.Constructors.Factories.Decisions;
using Fold.Motor.Resources;

namespace Fold.Motor.Constructors.Builders;

public class DecisionBuilder : IBuilder<Model.Decision>
{
    public readonly static Dictionary<string, IFactory<Model.Decision>> Factories = new() {
        { "AddTime", new AddTimeDecisionFactory() },
        { "ReportIllegal", new ReportIllegalDecisionFactory() }
    };

    public static Model.Decision Build(Message message)
    {
        if (!Factories.ContainsKey(message.Type))
            throw new BuilderParseException("Action type not buildable.");

        return Factories[message.Type].Instantiate(message.Data);
    }
}