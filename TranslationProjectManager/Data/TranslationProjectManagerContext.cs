using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TranslationProjectManager.Data;

/// <summary>
/// Kontekst bazy danych jest uzywany dla dostepu do danych aplikacji
/// </summary>
public class TranslationProjectManagerContext : IdentityDbContext<User>
{
    /// <summary>
    /// Kolekcje obiektow w bazie danych
    /// </summary>
    public DbSet<Client> Clients { get; set; }
    public DbSet<TranslationProject> TranslationProjects { get; set; }
    public DbSet<TranslationProjectComment> TranslationProjectComments { get; set; }
    public DbSet<TranslationTask> TranslationTasks { get; set; }
    public DbSet<TranslationTaskComment> TranslationTaskComments { get; set; }
    public DbSet<TranslatorAssignment> TranslatorAssignments { get; set; }
    public DbSet<TranslationProjectFile> TranslationProjectFiles { get; set; }
    public DbSet<File> Files { get; set; }

    public TranslationProjectManagerContext(DbContextOptions<TranslationProjectManagerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Zmiana defaultowych nazw tabeli, dostarczonych w ramach frameworku dla obslugi tozsamosci i uprawnien uzytkownikow
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<IdentityRole>().ToTable("Role");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

        // Opisywanie relacji powiedzy encjami w bazie danych
        // Generalnie, wszedzie powinien byc soft delete, dla tego mamy ustawiony wszedzie DeleteBehavior.NoAction
        modelBuilder.Entity<Client>().HasMany(c => c.TranslationProjects).WithOne(p => p.Client).HasForeignKey(p => p.ClientId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationProject>().HasMany(p => p.TranslationTasks).WithOne(t => t.TranslationProject).HasForeignKey(t => t.TranslationProjectId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationProject>().HasMany(p => p.TranslationProjectFiles).WithOne(f => f.TranslationProject).HasForeignKey(f => f.TranslationProjectId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationProject>().HasMany(p => p.TranslationProjectComments).WithOne(tpc => tpc.TranslationProject).HasForeignKey(tpc => tpc.TranslationProjectId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationTask>().HasMany(t => t.TranslatorAssignments).WithOne(a => a.TranslationTask).HasForeignKey(a => a.TranslationTaskId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationTask>().HasMany(t => t.TranslationTaskComments).WithOne(tpc => tpc.TranslationTask).HasForeignKey(tpc => tpc.TranslationTaskId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<TranslationTask>().HasMany(f => f.VisibleFiles).WithMany(t => t.VisibleIn);
        modelBuilder.Entity<User>().HasMany(u => u.TranslatorAssignments).WithOne(a => a.Translator).HasForeignKey(a => a.TranslatorId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>().HasMany(u => u.TranslationProjects).WithOne(t => t.Manager).HasForeignKey(t => t.ManagerId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>().HasMany(u => u.TranslationProjectComments).WithOne(tpc => tpc.Author).HasForeignKey(tpc => tpc.AuthorId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>().HasMany(u => u.TranslationTaskComments).WithOne(tpc => tpc.Author).HasForeignKey(tpc => tpc.AuthorId).OnDelete(DeleteBehavior.NoAction);
    }
}
