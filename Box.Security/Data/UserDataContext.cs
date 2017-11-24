using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Box.Security.Data
{
    public class UserDataContext : DbContext
    {
        private IConfiguration Configuration { get; }

        public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<AuthorizationUser> AuthorizationUsers { get; set; }
        public DbSet<AuthorizationRole> AuthorizationRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

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
                .HasMany(user => user.Authorizations);

            modelBuilder.Entity<AuthorizationUser>()
                .HasKey(authUser => authUser.Id);

            modelBuilder.Entity<AuthorizationRole>()
                .HasKey(authRole => authRole.Id);

            modelBuilder.Entity<Authorization>()
                .HasKey(auth => auth.AuthorizationId);
            modelBuilder.Entity<Authorization>()
                .HasMany(auth => auth.AuthorizationUsers)
                .WithOne(authUser => authUser.Authorization);
            modelBuilder.Entity<Authorization>()
                .HasMany(auth => auth.AuthorizationRoles)
                .WithOne(authRole => authRole.Authorization);
            modelBuilder.Entity<Authorization>()
                .HasIndex(auth => auth.SysName)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasKey(role => role.RoleId);
            modelBuilder.Entity<Role>()
                .HasMany(role => role.Authorizations);
            modelBuilder.Entity<Role>()
                .HasIndex(role => role.SysName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(Configuration.GetConnectionString("MySqlLocal"));
        }
        
        public async Task InitDb()
        {
            #region Init Authorizations
            await AddAuthorization(new Authorization{ Name = "Read Boxes from API", SysName = "api:read:boxes", Description = "Permission to read boxes from API"});
            await AddAuthorization(new Authorization{ Name = "Create Boxes in API", SysName = "api:create:boxes", Description = "Permission to create boxes in API"});
            await AddAuthorization(new Authorization{ Name = "Edit Boxes from API", SysName = "api:edit:boxes", Description = "Permission to edit boxes in API"});
            #endregion

            await SaveChangesAsync();
            
            #region Init Roles
            await AddRole(new Role{ Description = "Role for normal users", Name = "Normal User", SysName = "role:normal"}, 
                await Authorizations.Where(auth => auth.SysName.EndsWith("boxes")).ToListAsync());
            await AddRole(new Role { Description = "Role for administrative users", Name = "Admin", SysName = "role:admin"},
                await Authorizations.ToListAsync());
            #endregion
        }

        private async Task AddAuthorization(Authorization authorization)
        {
            if (!await Authorizations
                .AnyAsync(auth => auth.SysName == authorization.SysName))
            {
                await Authorizations.AddAsync(authorization);
                await SaveChangesAsync();
            }
        }

        private async Task AddRole(Role role, IEnumerable<Authorization> authorizations)
        {
            if (!await Roles
                .AnyAsync(r => r.SysName == role.SysName))
            {
                foreach (var auth in authorizations)
                {
                    await AuthorizationRoles.AddAsync(new AuthorizationRole{ Authorization = auth, Role = role});
                }
                await Roles.AddAsync(role);
                await SaveChangesAsync();
            }
        }
    }
}
