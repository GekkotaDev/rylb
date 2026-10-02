public interface IScriptable {
    ProcessMode ProcessMode { get; set; }

    public void Process(float deltaTime);
}
