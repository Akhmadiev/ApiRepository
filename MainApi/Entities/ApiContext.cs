namespace MainApi
{
    using ApiAdditional;
    using System.Data.Entity;

    public class ApiContext : DbContext
    {
        public ApiContext() : base("DbConnection1") { }

        public virtual DbSet<Country> Countries { get; set; }
    }
}