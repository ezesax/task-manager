using System.Data.Entity;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=TaskManagerConnectionString")
        {
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }  // Asegúrate de que `Projects` esté incluido
        public DbSet<User> Users { get; set; }  // Agrega `Users`
    }
}
