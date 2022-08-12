using System.Net.Http.Headers;

using Taxes.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<SelicTaxSearchService>(c =>
{
    c.BaseAddress = new Uri("http://api.bcb.gov.br/dados/serie/bcdata.sgs.11/dados");
    c.DefaultRequestHeaders.Add("User-Agent", "C# Automation");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
