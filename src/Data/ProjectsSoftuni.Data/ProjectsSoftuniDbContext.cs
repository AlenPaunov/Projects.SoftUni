namespace ProjectsSoftuni.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Data.Common.Models;
    using ProjectsSoftuni.Data.Models;

    public class ProjectsSoftuniDbContext : IdentityDbContext<ProjectsSoftuniUser, ProjectsSoftuniRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ProjectsSoftuniDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ProjectsSoftuniDbContext(DbContextOptions<ProjectsSoftuniDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Setting> Settings { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }

        public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

        public virtual DbSet<Application> Applications { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<TeamUser> TeamsUsers { get; set; }

        public virtual DbSet<TeamUserStatus> TeamUserStatuses { get; set; }

        public virtual DbSet<Specification> Specifications { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);
            ConfigureApplicationRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            //var foreignKeys = entityTypes
            //    .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            //foreach (var foreignKey in foreignKeys)
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}
        }

        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.Entity<ProjectsSoftuniUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProjectsSoftuniUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProjectsSoftuniUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureApplicationRelations(ModelBuilder builder)
        {
            //builder.Entity<Application>().HasKey(a => new { a.ProjectId, a.UserId });

            //builder.Entity<ProjectUser>().HasKey(pu => new { pu.ProjectId, pu.UserId });

            builder.Entity<TeamUser>().HasKey(a => new { a.TeamId, a.UserId });

            builder.Entity<Application>().HasKey(a => new { a.ProjectId, a.TeamId });

            builder.Entity<Application>()
                .HasOne(a => a.ApplicationStatus)
                .WithMany()
                .HasForeignKey(e => e.ApplicationStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
              .HasOne(a => a.Status)
              .WithMany()
              .HasForeignKey(e => e.StatusId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasMany(p => p.Teams)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TeamUser>()
                .HasOne(tu => tu.TeamUserStatus)
                .WithMany()
                .HasForeignKey(tu => tu.TeamUserStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
