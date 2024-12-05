using LibrarySystem.Api.Extensions;
using LibrarySystem.Api.Middlewares;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Repository.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static LibrarySystem.Repository.Data.Contexts.IdentityContextSeed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(connection);
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.IdentityServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        await LibraryContextSeed.SeedAsync(dbContext);
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        await SeedUsersAsync(userManager, roleManager);
        await SeedRolesAsync(roleManager);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration seeding.");
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddleware();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
