using boilerplate_app.Application;
using boilerplate_app.Application.Services;
using boilerplate_app.Infrastructure.Data;
using boilerplate_app.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //register dbcontext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register AutoMapper in the DI container
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

        //// Register IUserRepository and UserRepository
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        //// Register IUserService and UserService
        builder.Services.AddScoped<IUserService, UserService>();



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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}