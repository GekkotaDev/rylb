using Raylib_cs;

public interface IAnimated : ITimed {
    public float FPS { get; }
    public List<Texture2D> Frames { get; }
    public Texture2D Frame { get; }
}

public abstract class Animated : IAnimated {
    abstract public float FPS { get; }

    public List<Texture2D> Frames { get; } = [];
    public float Cycle { get => FPS; }
    public float Clock {
        get;
        set {
            field = value;
            if (value < 0) field = 0;
        }
    } = 0;
    public Texture2D Frame {
        get;
        set;
    }

    private int FrameIndex = -1;

    public void Tick(float deltaTime) {
        if (FrameIndex < 0) FrameIndex = Frames.Count - 1;
        if (Clock >= 0) return;

        Clock = Cycle;
        FrameIndex = FrameIndex >= Frames.Count - 1 ? 0 : FrameIndex + 1;
        Frame = Frames[FrameIndex];
    }
}
