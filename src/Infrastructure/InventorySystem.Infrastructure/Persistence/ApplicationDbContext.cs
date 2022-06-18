using InventorySystem.Application.Contracts.Infrastructure;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IUserService _userService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserService userService) : base(options)
        {
            _userService = userService;
        }

        public DbSet<Warehouse> Warehouses { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _userService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = _userService.UserId;
                        break;
                }
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
