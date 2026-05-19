using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.API.Services;
using SaddleHeroesAirWays.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using SaddleHeroesAirWays.API;
using SaddleHeroesAirWays.Library.Models;
using FluentValidation;

namespace SaddleHeroesAirWays.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<DbContextAPI>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUserService, UserService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            //test
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
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
}
