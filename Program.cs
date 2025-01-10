using System.Text;
using boilerplate_app.Application;
using boilerplate_app.Application.Services;
using boilerplate_app.Core.Entities;
using boilerplate_app.Infrastructure.Data;
using boilerplate_app.Infrastructure.Repositories;
using boilerplate_app.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
            };
        });
        builder.Services.AddAuthorization();

        //register dbcontext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        // Register Identity services

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


        //builder.Services.AddIdentity<User, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();

        // 
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        // Register AutoMapper in the DI container
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

       
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IJwtService, JwtService>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Global error handler
        app.UseMiddleware<ErrorHandelingMiddleware >();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        var scope = app.Services.CreateScope();
        await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
        await RoleSeeder.SeedAdminAsync(scope.ServiceProvider);

        //seeding
        //using (var scope = app.Services.CreateScope()) 
        //{ 

        //}

        app.Run();
    }
}