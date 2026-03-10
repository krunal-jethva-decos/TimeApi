using TimeApi.Hubs;
using TimeApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Custom services for Multi-Role support
builder.Services.AddSingleton<ActiveRoleTracker>();
builder.Services.AddSingleton<IRoleDataService, RoleDataService>();

builder.Services.AddSignalR();
builder.Services.AddHostedService<TimeWorker>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true) // Allow any origin
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();


app.MapHub<TimeHub>("/timehub");

app.MapHub<RoleHub>("/rolehub");

app.Run();

