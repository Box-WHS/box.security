using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Box.Security.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Box.Security.Data
{
    public class UserDataContext : DbContext
    {
        private IConfiguration Configuration { get; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

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
                .HasMany(user => user.UserRoles)
                .WithOne(userRole => userRole.User);

            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);
            modelBuilder.Entity<Role>()
                .HasIndex(role => role.Name)
                .IsUnique();
            modelBuilder.Entity<Role>()
                .HasIndex(role => role.DisplayName)
                .IsUnique();
            modelBuilder.Entity<Role>()
                .HasMany(role => role.UserRoles)
                .WithOne(userRole => userRole.Role);

            modelBuilder.Entity<UserRole>()
                .HasKey(userRole => userRole.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(Configuration.GetConnectionString("MySqlLocal"));
        }
        
        public async Task InitDb()
        {
            if (!Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role()
                    {
                        DisplayName = "Normaler Benutzer",
                        Name = "user"
                    },
                    new Role()
                    {
                        DisplayName = "Administrativer Benutzer",
                        Name = "admin"
                    }
                };
                await AddRangeAsync(roles);
                await SaveChangesAsync();
            }
        }

        public override EntityEntry Add(object entity)
        {
            User user;
            if ((user = entity as User) != null)
            {
                UserRoles.Add(new UserRole
                {
                    Role = Roles.FirstOrDefault(role => role.Name.Equals("user")),
                    User = user
                });
            }
            return base.Add(entity);
        }

        public override async Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new CancellationToken())
        {
            User user;
            if ((user = entity as User) != null)
            {
                await UserRoles.AddAsync(new UserRole
                {
                    Role = await Roles.FirstOrDefaultAsync(role => role.Name.Equals("user"), cancellationToken),
                    User = user
                }, cancellationToken);
            }
            return await base.AddAsync(entity, cancellationToken);
        }

        public override async Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            User user;
            if ((user = entity as User) != null)
            {
                await UserRoles.AddAsync(new UserRole
                {
                    Role = await Roles.FirstOrDefaultAsync(role => role.Name.Equals("user"), cancellationToken),
                    User = user
                }, cancellationToken);
            }
            return await base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            User user;
            if ((user = entity as User) != null)
            {
                UserRoles.Add(new UserRole
                {
                    Role = Roles.FirstOrDefault(role => role.Name.Equals("user")),
                    User = user
                });
            }
            return base.Add(entity);
        }
    }
}
