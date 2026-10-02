public interface IParentable<T> : IDisposable {
    public string Id { get; }
    public List<IParented<T>> Children { get; set; }
}

/// <summary>
/// Any game object that may contain children. All in game entities at the root
/// level are required to be parentable.
/// </summary>
/// <typeparam name="T">
/// Parent type signature
/// </typeparam>
abstract class Parentable<T> : IParentable<T>, IScriptable {
    public string Id { get; } = Guid.NewGuid().ToString();
    public List<IParented<T>> Children { get; set; } = [];
    public ProcessMode ProcessMode { get; set; } = ProcessMode.PAUSABLE;

    public void Dispose() {
        foreach (var child in Children) child.Dispose();
    }

    public void Process(float deltaTime) {
        foreach (var child in Children) {
            if (child.Freed) child.Dispose();
        }

        Children = [.. Children.Where(child => child.Freed == false)];
    }

    public Parentable<T> AddChild(IParented<T> child) {
        if (!Children.Any((element) => element.Id == child.Id)) return this;
        Children.Add(child);
        return this;
    }
}
