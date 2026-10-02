public interface IParented<T> : IFreeable {
    public string Id { get; }
    public T Parent { get; }
}
