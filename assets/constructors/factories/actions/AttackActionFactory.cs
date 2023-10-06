using System.Text.Json;
using System.Linq;
using Fold.Motor.Model.Actions;

namespace Fold.Motor.Constructors.Factories.Actions;

public class AttackActionFactory : IFactory<Model.Action>
{
    public Model.Action Instantiate(Dictionary<string, object?> data)
    {
        if (!data.ContainsKey("from"))
            throw new FactoryParseException("Action request doesn't contain key \"from\".");

        if (!data.ContainsKey("to"))
            throw new FactoryParseException("Action request doesn't contain key \"from\".");

        if (data["from"] == null)
            throw new ArgumentNullException();

        if (data["to"] == null)
            throw new ArgumentNullException();

        JsonElement fromBoardPositionsJson = (JsonElement)(data["from"] ?? new());
        Console.WriteLine(fromBoardPositionsJson.ToString());
        List<Model.BoardPosition> fromBoardPosition = fromBoardPositionsJson
            .EnumerateArray()
            .Select(x => new Model.BoardPosition(x.ToString()))
            .ToList();

        return new AttackAction(
            fromBoardPosition,
            new Model.BoardPosition((data["to"] ?? new()).ToString())
        );
    }
}