using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class TimelyContext : DbContext
    {
        public TimelyContext()
            : base("name=TimelyContext")
        {
        }

        static TimelyContext()
        {
            Database.SetInitializer(new TimelyContextInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasMany(c => c.users)
                .WithMany()
                .Map(x =>
                    {
                        x.MapLeftKey("team_id");
                        x.MapRightKey("user_id");
                        x.ToTable("TeamMember");
                    });
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var changeSet = ChangeTracker.Entries<Entity>();

            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
                {
                    entry.Entity.updated_at = DateTime.UtcNow;
                }

                foreach (var entry in changeSet.Where(c => c.State == EntityState.Added))
                {
                    entry.Entity.created_at = DateTime.UtcNow;
                }

                foreach (var dbEntityEntry in changeSet.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
                {
                    dbEntityEntry.Entity.version =+ 1;
                }
            }
            return base.SaveChanges();
        }
    }
}