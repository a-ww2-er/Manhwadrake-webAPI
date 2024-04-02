using Manhwadrake_webAPI.Entities;

const string GetGameEndpointName = "GetGame";

List<Game> games = new()
{
    new Game()
    {
        Id = 0,
        Name = "game 0",
        Genre = "action",
        Price = 10.99M,
        Description = "an action game",
        ReleaseDate = DateTime.Now.ToLocalTime(),
        ImageUri = "https://placehold.co/100"
    },
      new Game()
    {
        Id = 1,
        Name = "game 1",
        Genre = "adventure",
        Price = 34.99M,
        Description = "an action game",
        ReleaseDate = new DateTime(2022,9,23),
        ImageUri = "https://placehold.co/100"
    },
        new Game()
    {
        Id = 2,
        Name = "game 2",
        Genre = "action",
        Price = 12.99M,
        Description = "another action game",
        ReleaseDate = new DateTime(2023,1,13),
        ImageUri = "https://placehold.co/100"
    },
          new Game()
    {
        Id = 3,
        Name = "game 3",
        Genre = "racing",
        Price = 49.99M,
        Description = "a racing game",
        ReleaseDate = DateTime.Now.ToLocalTime(),
        ImageUri = "https://placehold.co/100"
    },
            new Game()
    {
        Id = 4,
        Name = "game 4",
        Genre = "action",
        Price = 19.99M,
        Description = "an action game",
        ReleaseDate = new DateTime(2023,3,14),
        ImageUri = "https://placehold.co/100"
    },
              new Game()
    {
        Id = 5,
        Name = "game 5",
        Genre = "action",
        Price = 10.99M,
        Description = "an action game",
        ReleaseDate = new DateTime(2023,4,1),
        ImageUri = "https://placehold.co/100"
    },
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
Console.WriteLine("code is running");

//Root endpoint
app.MapGet("/", () => "end points are : 1. /games");

//Get all 
app.MapGet("/games", () => games);

//Get by id
app.MapGet("/games/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(game);
}).WithName(GetGameEndpointName);

//Post
app.MapPost("/create", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    //By doing this we're creating a location header in the response, that points to the
    //created resource location
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

//Update
app.MapPut("/games/{id}", (int id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    existingGame.ImageUri = updatedGame.ImageUri;

    return Results.NoContent();

});

//Delete
app.MapDelete("/games/{id}", (int id) =>
{
    Game? existingGame = games.Find(game => game.Id == id);

    if (existingGame is not null)
    {
        games.Remove(existingGame);
    }

    return Results.NoContent();
});
//..app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
