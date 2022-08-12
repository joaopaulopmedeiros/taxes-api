using Serilog;

using StackExchange.Redis;

using Taxes.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("RedisConnectionString") ?? "localhost"));
builder.Services.AddScoped<IDatabase>(s =>
{
    var db = s.GetService<IConnectionMultiplexer>()?.GetDatabase();
    if (db == null) throw new NullReferenceException("Unable to connect on redis. No database instance found.");
    return db;
});
builder.Services.AddTransient<ITaxesSearchService, SelicTaxSearchService>();
builder.Services.AddHttpClient("Selic", c =>
{
    var uri = Environment.GetEnvironmentVariable("BcbApiBaseUrl");
    if (uri == null) throw new NullReferenceException("Unable to connect on Banco Central. No api base url found.");
    c.BaseAddress = new Uri(uri);
    c.DefaultRequestHeaders.Add("User-Agent", "C# Automation");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
