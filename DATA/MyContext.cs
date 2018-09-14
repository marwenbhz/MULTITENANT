using DATA.Configurations;
using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    public class MyContext : DbContext
    {
        public MyContext() : base("Name=PMToolCnx")
        {

        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<DOMAIN.Entities.Task> Tasks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Banque> Banques { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new TaskConfiguration());

        }
    }
}
