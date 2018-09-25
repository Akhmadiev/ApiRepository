namespace MainApi
{
    using ApiAdditional;
    using MainApi.Entities;
    using System.Data.Entity;

    public class ApiContext : DbContext
    {
        public ApiContext() : base("DbConnection4") { }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<MaxReport> MaxReports { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Report> Reports { get; set; }
    }
}