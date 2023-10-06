namespace Fold.Motor.Constructors;

public interface IFactory<T>
{
    public T Instantiate(Dictionary<string, Object?> data);
}

public class FactoryParseException : Exception
{
    public FactoryParseException(string message) : base(message) { }
}