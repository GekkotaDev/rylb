public interface ITimed : ITicked {
    public float Cycle { get; }
    public float Clock { get; set; }
}