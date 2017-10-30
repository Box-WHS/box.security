using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Box.Security.Data
{
    public class UserDataContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<User> Users { get; set; }

        public UserDataContext(IConfiguration configuration, DbContextOptions<UserDataContext> options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);
            modelBuilder.Entity<User>()
                .HasIndex(user => user.UserName)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(user => user.FirstName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.PasswordHash)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasOne(user => user.Role);


            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);
            modelBuilder.Entity<Role>()
                .HasIndex(role => role.Name);
            modelBuilder.Entity<Role>()
                .HasMany(role => role.Policies);

            modelBuilder.Entity<Policy>()
                .HasKey(policy => policy.PolicyId);
            modelBuilder.Entity<Policy>()
                .HasIndex(policy => policy.PolicyName)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseMySql(Configuration.GetConnectionString("MySqlLocal"));
    }
}
