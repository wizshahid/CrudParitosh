using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", b =>
    {
        b
            .WithOrigins(builder.Configuration["AllowedOrigins"].Split(";"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

app.UseCors("CorsPolicy");
await app.UseOcelot();

app.Run();
