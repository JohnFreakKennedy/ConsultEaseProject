using ConsultEaseAPI.Startup;
using Microsoft.AspNetCore.Identity;

namespace ConsultEaseAPI;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterApplicationServices(builder.Configuration);

        var app = builder.Build();
        app.ConfigureMiddleware();
        
        await using var scope = app.Services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await CreateRolesAsync(roleManager);
        
        await app.RunAsync();
    }
    
    private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<string> {"Admin", "Professor", "Student"};
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
   
}

