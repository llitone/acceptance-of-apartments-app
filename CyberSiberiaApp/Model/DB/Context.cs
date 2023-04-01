using CyberSiberiaApp.Model.DB.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace CyberSiberiaApp.Model.DB
{
    public class Context : DbContext
    {
        public Context()   
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //var basePath = FileSystem.AppDataDirectory;
            //var res = Path.Combine(basePath, DatabaseConstants.DATABASE_CONNECTION);
            options.UseSqlite(@$"Filename={DatabaseConstants.DATABASE_PATH}");
            
        }

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<EntityModels.Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
