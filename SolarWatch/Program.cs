using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Data;
using SolarWatch.Models;
using SolarWatch.Repository;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;
using SolarWatch.Services.SolarWatchData;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");
        

        AddServices();
        ConfigureSwagger();
        AddDbContext();
        AddAuthentication();
        AddIdentity();

        var app = builder.Build();

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Testing")
        {
            AddRoles();
            AddAdmin();
        }

//Migrate InventoryManagementDBContext
        DbMigration();


// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

// Add CORS middleware here
        app.UseCors(builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.UseHttpsRedirection();

//Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

        void DbMigration()
        {
            // migrate any database changes on startup (includes initial db creation)
            using (var scope = app.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
                if (dataContext.Database.IsRelational())
                {
                    dataContext.Database.Migrate();
                }
            }
        }

        void AddServices()
        {
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<ISolarWatchRepository, SolarWatchRepository>();
            builder.Services.AddScoped<ISolarWatchService, SolarWatchService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ICoordinatesProvider, GeoCodingApi>();
            builder.Services.AddScoped<IGeoCodingJsonProcessor, GeoCodingJsonProcessor>();
            builder.Services.AddScoped<ISolarWatchDataProvider, SolarWatchDataProvider>();
            builder.Services.AddScoped<ISolarWatchJsonProcessor, SolarWatchJsonProcessor>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
        }

//Configure Swagger
        void ConfigureSwagger()
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

//Add DbContext
        void AddDbContext()
        {
            builder.Services.AddDbContext<SolarWatchContext>(options =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
                {
                    options.UseInMemoryDatabase("SolarWatchTestDb");
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            });

            builder.Services.AddDbContext<UsersContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

//Add authentication
        void AddAuthentication()
        {
            //This will add a JWT token authentication scheme to your API. This piece of code is required to validate a JWT.
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("ASPNETCORE_VALIDISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("ASPNETCORE_VALIDAUDIENCE"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ASPNETCORE_ISSUERSIGNINGKEY") ??
                            throw new InvalidOperationException()))
                    };
                });
        }

//Add identity user
        void AddIdentity()
        {
            //User requirements
            builder.Services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<IdentityRole>() //Enable Identity roles 
                .AddEntityFrameworkStores<UsersContext>();
        }

//Add roles
        void AddRoles()
        {
            using var
                scope = app.Services
                    .CreateScope(); // RoleManager is a scoped service, therefore we need a scope instance to access it
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var tAdmin = CreateAdminRole(roleManager);
            tAdmin.Wait();

            var tUser = CreateUserRole(roleManager);
            tUser.Wait();
        }

//Create roles
        async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(
                new IdentityRole("Admin")); //The role string should better be stored as a constant or a value in appsettings
        }

        async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(
                new IdentityRole("User")); //The role string should better be stored as a constant or a value in appsettings
        }

        void AddAdmin()
        {
            var tAdmin = CreateAdminIfNotExists();
            tAdmin.Wait();
        }

//Create Admin if not exists
        async Task CreateAdminIfNotExists()
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminInDb == null)
            {
                var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
                var adminCreated = await userManager.CreateAsync(admin, "admin123");

                if (adminCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
