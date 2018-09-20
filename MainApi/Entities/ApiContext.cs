namespace MainApi
{
    using System.Data.Entity;

    public class ApiContext : DbContext
    {
        public virtual DbSet<Country> Countries { get; set; }
    }
}