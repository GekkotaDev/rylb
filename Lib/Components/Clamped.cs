using System.Numerics;

public interface IClamped<T> where T : INumber<T> {
    public T MaximumValue { get; }
    public T MinimumValue { get; }
    public T Value { get; set; }
}

public abstract class Clamped<T> : IClamped<T> where T : INumber<T> {
    abstract public T MaximumValue { get; }
    abstract public T MinimumValue { get; }

    required public T Value {
        get;
        set {
            field = value;
            if (value > MaximumValue) field = MaximumValue;
            if (value < MinimumValue) field = MinimumValue;
        }
    }
}
