using Benchmark;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BenchmarkDbContextPg>(options =>
    options.UseNpgsql("Server=localhost;Database=benchmark;Username=postgres;Password=123"));

var mongoClient = new MongoClient("mongodb://mongo:123@localhost:27017");
builder.Services.AddSingleton<IMongoClient>(mongoClient);
var databaseName = builder.Configuration["MongoDbDatabase"] ?? "benchmark";
var database = mongoClient.GetDatabase(databaseName);
builder.Services.AddSingleton(database);


var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/person/pg", async (BenchmarkDbContextPg context) =>
{
    var name = new NamePg
    {
        FirstName = GenerateRandomString(8),
        LastName = GenerateRandomString(10)
    };

    context.Names.Add(name);
    await context.SaveChangesAsync();

    var person = new PersonPg
    {
        NameId = name.Id,
        Name = name,
        Age = new Random().Next(18, 100)
    };

    context.Person.Add(person);
    await context.SaveChangesAsync();

    return Results.Ok(person);
});

app.MapGet("/person/pg", async (BenchmarkDbContextPg context) =>
{
    var persons = await context.Person.ToListAsync();
    return Results.Ok(persons);
});


app.MapGet("/person/mg", async (BenchmarkDbContextPg context) =>
{
    var personCollection = database.GetCollection<PersonMg>("persons");
    var personsCursor = await personCollection.FindAsync(Builders<PersonMg>.Filter.Empty);
    var persons = await personsCursor.ToListAsync();
    return Results.Ok(persons);
});

app.MapPost("/person/mg", async (BenchmarkDbContextPg context) =>
{
    var namepg = new NamePg
    {
        FirstName = GenerateRandomString(8),
        LastName = GenerateRandomString(10)
    };

    context.Names.Add(namepg);
    await context.SaveChangesAsync();

    var personpg = new PersonPg
    {
        NameId = namepg.Id,
        Name = namepg,
        Age = new Random().Next(18, 100)
    };

    context.Person.Add(personpg);
    await context.SaveChangesAsync();
    
    var namesCollection = database.GetCollection<NameMg>("names");
    var personsCollection = database.GetCollection<PersonMg>("persons");
    
    var namemg = new NameMg()
    {
        FirstName = namepg.FirstName,
        LastName = namepg.LastName,
    };
    
    await namesCollection.InsertOneAsync(namemg);

    var personmg = new PersonMg
    {
        Name = namemg,
        Age = personpg.Age,
    };
    
    await personsCollection.InsertOneAsync(personmg);

    return Results.Ok(personmg);
});

app.Run();


string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
}