public interface IPreparable {
    public bool Readied { get; }

    /// <summary>
    /// Called to initialize entity.
    /// </summary>
    public void Ready();
}
