using System.Text;
using ConsultEaseBLL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ConsultEaseBLL.MappingProfiles;
using ConsultEaseBLL.Services;
using ConsultEaseBLL.Services.Authentication;
using ConsultEaseBLL.Interfaces;
using ConsultEaseBLL.Interfaces.Authentication;
using ConsultEaseDAL.Context;
using ConsultEaseDAL.Entities.Auth;

namespace ConsultEaseAPI.Startup;

public static class ServiceInitializer
{
    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConsultEaseAPI", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            var security = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        UnresolvedReference = true
                    },
                    new List<string>() { }
                }
            };
            c.AddSecurityRequirement(security);
        });
    }
    
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration
        configuration)
    {
        RegisterCustomDependencies(services, configuration);
        RegisterSwagger(services);
        return services;
    }

    public static void RegisterCustomDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ConsultEaseMappingProfile));
        services.AddDbContext<ConsultEaseDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConsultEaseDbConnection")));
            
        services.AddScoped<DbContext, ConsultEaseDbContext>();
        services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ConsultEaseDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICounsellingCategoryService, CounsellingCategoryService>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowAllOrigins",
                builder =>
                {
                    //TODO: manage ports
                    builder.WithOrigins("http://localhost:3000", "https://localhost:3000");
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
        });
        
        
        services.Configure<DataProtectionTokenProviderOptions>(options 
            => options.TokenLifespan = TimeSpan.FromHours(2));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        services
            .AddAuthorization(options =>
                options.AddPolicy("ElevatedRights",
                    policy => policy.RequireRole("Admin", "Professor", "Student")))
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options => options.LoginPath = "/api/auth/signin")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                if (jwtSettings?.Secret != null)
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings?.Issuer,
                        ValidAudience = jwtSettings?.Issuer,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
            });
        // TODO: check if this is needed
        /*
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
        services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddScoped<IdentityErrorDescriber>();
        services.AddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
        services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, IdentityRole<int>>>();
        services.AddScoped<UserManager<User>>();
        services.AddScoped<SignInManager<User>>();
        services.AddScoped<RoleManager<IdentityRole<int>>>();
        */
    }
}
