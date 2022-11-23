using SkrillClientAPI.Services;
using SkrillClientAPI.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SkrillAPIService, SkrillAPIService>();
builder.Services.AddSingleton<ClientRequest, ClientRequest>();

var app = builder.Build();

#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif

app.UseAuthorization();
app.MapControllers();
app.Run();
