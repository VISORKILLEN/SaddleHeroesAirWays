using Microsoft.EntityFrameworkCore;
using SaddleHeroesAirWays.Library.Models;

namespace SaddleHeroesAirWays.API
{
    public class DbContextAPI : DbContext
    {
        public DbContextAPI(DbContextOptions<DbContextAPI> options) : base(options)
        {
        }

        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public DbSet<Airport> airport { get; set; }


    }
}
