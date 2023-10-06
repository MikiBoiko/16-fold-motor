using Fold.Motor.Resources;

namespace Fold.Motor.Constructors;

public interface IBuilder<T>
{
    static abstract T Build(Message message);
}

public class BuilderParseException : Exception
{
    public BuilderParseException(string message) : base(message) { }
}