// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Axolotl.EFCore.Base;
using Axolotl.EFCore.Context;
using Axolotl.EFCore.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XClaim.Common.Entity;
using XClaim.Common.Enums;

namespace XClaim.Common.Context;

public class ServiceContext : IdentityDbContext<AccountEntity> {

    protected override void OnConfiguring(DbContextOptionsBuilder options) { }

    public ServiceContext(DbContextOptions<ServiceContext> options) : base(
        ChangeOptionsType<ServiceContext>(options)) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            modelBuilder.DateTimeOffsetToBinary();

        var entitiesAssembly = typeof(ServiceContext).Assembly;
        modelBuilder.RegisterAllEntities<BaseEntity<Guid>>(entitiesAssembly);
        modelBuilder.RegisterAllEntities<AuditableEntity<Guid>>(entitiesAssembly);
        modelBuilder.RegisterSoftDeleteFilter();
        modelBuilder.RegisterEnumConverters<NotificationChannels>(typeof(ICollection<NotificationChannels>));
        modelBuilder.RegisterEnumConverters<EventType>(typeof(ICollection<EventType>));

        // User - Company (Manager) One to One
        modelBuilder.Entity<CompanyEntity>()
            .HasOne(c => c.Manager)
            .WithOne(u => u.CompanyManaged)
            .HasForeignKey<CompanyEntity>(c => c.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        // User - Team (Manager) One to One
        modelBuilder.Entity<TeamEntity>()
            .HasOne(t => t.Manager)
            .WithOne(u => u.TeamManaged)
            .HasForeignKey<TeamEntity>(t => t.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public DbSet<ServerEntity> Server { get; set; }
    public override DbSet<AccountEntity> Users { get; set; }
    public DbSet<ProfileEntity> Profiles { get; set; }
    public DbSet<NotificationEntity> UserNotifications { get; set; } = default!;
    public DbSet<SettingsEntity> UserSetting { get; set; } = default!;
    public DbSet<BankAccountEntity> UserBankAccount { get; set; } = default!;
    public DbSet<TeamEntity> Teams { get; set; } = default!;
    public DbSet<BankEntity> Banks { get; set; } = default!;
    public DbSet<CompanyEntity> Companies { get; set; } = default!;
    public DbSet<CategoryEntity> Categories { get; set; } = default!;
    public DbSet<CommentEntity> Comments { get; set; } = default!;
    public DbSet<CurrencyEntity> Currencies { get; set; } = default!;
    public DbSet<EventEntity> Events { get; set; } = default!;
    public DbSet<ClaimEntity> Claims { get; set; } = default!;
    public DbSet<FileEntity> Files { get; set; } = default!;
    public DbSet<PaymentEntity> Payments { get; set; } = default!;
    public DbSet<ProfileTransferEntity> TransferRequests { get; set; } = default!;


    // TODO: Check things around
    // Review later
    protected static DbContextOptions<TContext> ChangeOptionsType<TContext>(DbContextOptions options) where TContext : DbContext
        => (DbContextOptions<TContext>)options;

    public override int SaveChanges(bool acceptAllChangesOnSuccess) {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    ) {
        OnBeforeSaving();
        return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
            cancellationToken));
    }

    private void OnBeforeSaving() {
        var entries = ChangeTracker.Entries();
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in entries) {
            if (entry.Entity is IAuditableEntity trackable) {
                switch (entry.State) {
                    case EntityState.Added:
                        entry.Property("UpdatedAt").IsModified = false;
                        entry.Property("DeletedAt").IsModified = false;
                        trackable.CreatedAt = utcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property("CreatedAt").IsModified = false;
                        entry.Property("DeletedAt").IsModified = false;
                        trackable.UpdatedAt = utcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        // ReSharper disable once UnusedVariable
                        bool all = entry.References.All(e => e.IsModified = true);
                        entry.Property("CreatedAt").IsModified = false;
                        entry.Property("UpdatedAt").IsModified = false;
                        trackable.DeletedAt = utcNow;
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    default:
                        break;
                }
            }
        }
    }
}