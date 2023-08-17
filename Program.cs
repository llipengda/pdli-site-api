using PDLiSiteAPI.Hubs;
using PDLiSiteAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IMinecraftService, MinecraftService>();
builder.Services.AddSingleton<ICodeServerService, CodeServerService>();
builder.Services.AddSingleton<IServiceStatusService, ServiceStatusService>();

builder.Services.AddCors(
    option =>
        option.AddPolicy(
            "localhost",
            policy =>
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
        )
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("localhost");

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<LogHub>("logHub");
});

app.Run();
