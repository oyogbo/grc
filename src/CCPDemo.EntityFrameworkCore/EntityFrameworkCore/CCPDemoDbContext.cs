using CCPDemo.Risks;
using CCPDemo.RiskTypes;
using CCPDemo.RiskTransactions;
using CCPDemo.Departments;
using CCPDemo.VRisks;
using CCPDemo.Regulations;
using CCPDemo.ManagementReviews;
using CCPDemo.Projects;
using CCPDemo.Assessments;
using CCPDemo.ThreatGroupings;
using CCPDemo.ThreatCatalogs;
using CCPDemo.Closures;
using CCPDemo.RiskCloseReason;
using CCPDemo.RiskCategory;
using CCPDemo.RiskModels;
using CCPDemo.ReviewLevels;
using CCPDemo.RiskLevels;
using CCPDemo.RiskGroupings;
using CCPDemo.RiskFunctions;
using CCPDemo.RiskCatalogs;
using CCPDemo.Employee;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CCPDemo.Authorization.Delegation;
using CCPDemo.Authorization.Roles;
using CCPDemo.Authorization.Users;
using CCPDemo.Chat;
using CCPDemo.Editions;
using CCPDemo.Friendships;
using CCPDemo.MultiTenancy;
using CCPDemo.MultiTenancy.Accounting;
using CCPDemo.MultiTenancy.Payments;
using CCPDemo.Storage;
using CCPDemo.Persons;
using CCPDemo.Phones;
using CCPDemo.PhoneTypeEntityDir;

namespace CCPDemo.EntityFrameworkCore
{
    public class CCPDemoDbContext : AbpZeroDbContext<Tenant, Role, User, CCPDemoDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Risk> Risks { get; set; }

        public virtual DbSet<RiskType> RiskTypes { get; set; }

        public virtual DbSet<RiskTransaction> RiskTransactions { get; set; }
        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<VRisk> VRisks { get; set; }

        public virtual DbSet<Regulation> Regulations { get; set; }

        public virtual DbSet<ManagementReview> ManagementReviews { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Assessment> Assessments { get; set; }

        public virtual DbSet<ThreatGrouping> ThreatGrouping { get; set; }

        public virtual DbSet<ThreatCatalog> ThreatCatalog { get; set; }

        public virtual DbSet<Closure> Closures { get; set; }

        public virtual DbSet<CloseReason> CloseReasons { get; set; }

        public virtual DbSet<Category> Category { get; set; }

        public virtual DbSet<RiskModel> RiskModels { get; set; }

        public virtual DbSet<ReviewLevel> ReviewLevels { get; set; }

        public virtual DbSet<RiskLevel> RiskLevels { get; set; }

        public virtual DbSet<RiskGrouping> RiskGroupings { get; set; }

        public virtual DbSet<RiskFunction> RiskFunctions { get; set; }

        public virtual DbSet<RiskCatalog> RiskCatalogs { get; set; }

        public virtual DbSet<Employees> Employees { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<PhoneType> PhoneTypes { get; set; }

        public virtual DbSet<PhonePb> Phones { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public CCPDemoDbContext(DbContextOptions<CCPDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<VRisk>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Regulation>(r =>
                                             {
                                                 r.HasIndex(e => new { e.TenantId });
                                             });
            modelBuilder.Entity<ManagementReview>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Project>(p =>
                                  {
                                      p.HasIndex(e => new { e.TenantId });
                                  });
            modelBuilder.Entity<Assessment>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ThreatGrouping>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ThreatCatalog>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Closure>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CloseReason>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Category>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RiskModel>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ReviewLevel>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RiskLevel>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RiskGrouping>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RiskFunction>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RiskCatalog>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Employees>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}