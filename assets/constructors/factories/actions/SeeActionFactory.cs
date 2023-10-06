using Fold.Motor.Model.Actions;

namespace Fold.Motor.Constructors.Factories.Actions;

public class SeeActionFactory : IFactory<Model.Action>
{
    public Model.Action Instantiate(Dictionary<string, object?> data)
    {
        if (data == null)
            throw new FactoryParseException("Null data in SeeActionFactory.");

        if (!data.ContainsKey("from"))
            throw new FactoryParseException("Action request doesn't contain key \"from\".");

        if (data["from"] == null)
            throw new ArgumentNullException();

        return new SeeAction(
            new Model.BoardPosition((data["from"] ?? new()).ToString())
        );
    }
}