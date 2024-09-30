using MediatR;
using Microsoft.EntityFrameworkCore;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.Extensions.Configuration;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Infrastructure
{
    public partial class DVPContext : DbContext, IUnitOfWork
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!;
        public DbSet<UserRole> UserRole { get; set; } = null!;    
        public DbSet<UserTask> UserTask { get; set; } = null!;    
        


        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;
        private readonly IConfiguration _configuration;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {        
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DVPContext()
        {
        }

        public DVPContext(DbContextOptions<DVPContext> options)
            : base(options)
        {             
        }

        public DVPContext(DbContextOptions<DVPContext> options, IMediator mediator, IConfiguration configuration) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            //tables

            modelBuilder.Entity<UserTask>(entity => 
             {                
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(t => t.Description)
                    .HasMaxLength(500);  
                entity.Property(t => t.Status)
                    .IsRequired()
                    .HasConversion<string>(); 
                entity.Property(t => t.CreatedAt)
                    .IsRequired();
                entity.Property(t => t.DueDate)
                    .IsRequired(false);
                entity.Property(t => t.UserId)
                    .IsRequired();
                entity.Property(t => t.Priority)
                    .IsRequired()
                    .HasConversion<string>();
                entity.Property(t => t.Comments)
                    .HasMaxLength(500); 
                entity.Property(t => t.CompletionDate)
                    .IsRequired(false);   
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id); 
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100); 

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200); 

                entity.Property(u => u.Nickname)
                    .HasMaxLength(50); 

                entity.Property(u => u.CreatedAt)
                    .IsRequired(); 

                entity.Property(u => u.IsEnabled)
                    .IsRequired(); 
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles"); 
                entity.HasKey(r => r.Id);
                entity.Property(r => r.RoleName)
                    .IsRequired() 
                    .HasMaxLength(100); 
                entity.Property(r => r.RoleDescription)
                    .HasMaxLength(500); 

            });
            
            modelBuilder.Entity<UserRole>(
                entity =>
                {
                    entity.ToTable("UserRole"); 
                    entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                    entity.Property(ur => ur.UserId)
                        .IsRequired();

                    entity.Property(ur => ur.RoleId)
                        .IsRequired(); 
                }
            );

            //relatinships
            modelBuilder.Entity<UserRole>()
                .HasOne<User>() 
                .WithMany() 
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne<Role>() 
                .WithMany() 
                .HasForeignKey(ur => ur.RoleId);
            
            modelBuilder.Entity<UserTask>()
               .HasOne<User>() 
               .WithMany() 
               .HasForeignKey(t => t.UserId);
                

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync<Guid>(this);
            await _mediator.DispatchDomainEventsAsync<int>(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}