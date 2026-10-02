using Raylib_cs;

class Game : IDisposable {
    private static Game? Instance;

    static bool Paused = false;

    public List<Action> Disposers = [];
    public List<Parentable<dynamic>> Entities = [];
    public bool IsActive {
        get => Raylib.WindowShouldClose() == false;
    }
    public float DeltaTime {
        get => Raylib.GetFrameTime();
    }

    private Game() {
        Raylib.InitWindow(Config.Width, Config.Height, Config.Title);
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(Config.FPS);

        foreach (var entity in Entities) {
            if (entity is IBootable bootable) bootable.Init();
        }
    }

    public static Game Fetch(List<Parentable<dynamic>>? entities = null) {
        if (Instance == null) {
            Instance = new Game {
                Entities = entities ?? []
            };

            return Instance;
        }

        return Instance;
    }

    public static Game Retry() {
        Instance = new Game();
        return Instance;
    }

    public Game Ready() {
        foreach (var entity in Entities) {
            if (entity is IPreparable preparable) preparable.Ready();
        }

        return this;
    }

    public Game UseEntities(Action<List<Parentable<dynamic>>> builder) {
        builder(Entities);
        return this;
    }

    public Game AddEntity(Parentable<dynamic> entity) {
        if (!Entities.Any((element) => element.Id == entity.Id)) return this;
        Entities.Add(entity);
        if (entity is IPreparable preparable) preparable.Ready();
        return this;
    }

    public Game RemoveEntity(string id) {
        Entities = [.. Entities.Where(entity => entity.Id != id)];
        return this;
    }

    public Game OnDispose(Action dispose) {
        Disposers.Add(dispose);
        return this;
    }

    public void Loop(Action? preprocess = null) {
        Entities = [.. Entities.Where(entity => {
            if (entity is IFreeable freeable) return !freeable.Freed;
            return true;
        })];

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Input();
        if (preprocess != null) preprocess();
        Process();
        Draw();

        Raylib.EndDrawing();
    }

    public void Dispose() {
        foreach (var entity in Entities) {
            if (entity is IDisposable disposable) disposable.Dispose();
        }

        foreach (var disposer in Disposers) disposer();

        Entities = [];
        Disposers = [];

        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    private void Input() {
        foreach (var entity in Entities) {
            if (entity is IInputable inputable) inputable.Input();
        }
    }

    private void Process() {
        foreach (var entity in Entities) {
            if (entity.ProcessMode == ProcessMode.DISABLED) continue;
            if (entity.ProcessMode == ProcessMode.PAUSABLE && Paused) continue;
            if (entity.ProcessMode == ProcessMode.WHEN_PAUSED && !Paused) {
                entity.Process(DeltaTime);
                continue;
            }
            if (entity.ProcessMode == ProcessMode.ALWAYS) {
                entity.Process(DeltaTime);
                continue;
            }

            entity.Process(DeltaTime);
        }
    }

    private void Draw() {
        foreach (var entity in Entities) {
            if (entity is IDrawable drawable) drawable.Draw();
        }
    }
}
