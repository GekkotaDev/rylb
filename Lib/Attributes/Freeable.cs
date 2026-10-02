public interface IFreeable : IDisposable {
    /// <summary>
    /// Marks game object for garbage collection.
    /// </summary>
    public bool Freed { get; set; }
}
