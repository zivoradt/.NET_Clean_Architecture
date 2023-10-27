using Domain;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Configuration.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class LeaveManagementDbContext : DbContext
    {
        public LeaveManagementDbContext(DbContextOptions<LeaveManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaveManagementDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());

            modelBuilder.Entity<LeaveType>().HasData(Data());
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected LeaveType[] Data()
        {
            LeaveType[] data = new LeaveType[]
            {
            new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Vacation",
            },
            new LeaveType
            {
                Id = 2,
                DefaultDays = 12,
                Name = "Sick",
            }
            };

            return data;
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocation { get; set; }
    }
}