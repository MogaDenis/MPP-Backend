using Microsoft.EntityFrameworkCore;

namespace MPP_Backend.Data.Models
{
    public class CarManagerContext : DbContext
    {
        public CarManagerContext() { }

        public CarManagerContext(DbContextOptions<CarManagerContext> options) 
            : base(options) { }

        public virtual DbSet<Car> Cars { get; set; }    

        public virtual DbSet<Owner> Owners { get; set; }

        public virtual DbSet<User> Users { get; set; }
    }
}
