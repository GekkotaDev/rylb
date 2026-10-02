var game = Game
    .Fetch()
    .Ready()
    .UseEntities((entities) => {

    })
    .OnDispose(() => {

    });

using (game) while (game.IsActive) {
    game.Loop();
}
