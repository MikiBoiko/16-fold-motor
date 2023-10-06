using Fold.Motor.Constructors.Factories.Actions;
using Fold.Motor.Resources;

namespace Fold.Motor.Constructors.Builders;

public class ActionBuilder : Constructors.IBuilder<Model.Action>
{
    public readonly static Dictionary<string, Constructors.IFactory<Model.Action>> Factories = new() {
        { "Attack", new AttackActionFactory() },
        { "Move", new MoveActionFactory() },
        { "Passing", new PassingActionFactory() },
        { "See", new SeeActionFactory() }
    };

    public static Model.Action Build(Message message)
    {
        if (!Factories.ContainsKey(message.Type))
            throw new BuilderParseException("Action type not buildable.");

        return Factories[message.Type].Instantiate(message.Data);
    }
}