using CryptoApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CryptoOptions>(
    builder.Configuration.GetSection("Crypto"));

builder.Services.AddSingleton<CryptoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
