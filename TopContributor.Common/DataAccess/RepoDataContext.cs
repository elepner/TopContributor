using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopContributor.Common.Model;

namespace TopContributor.Common.DataAccess
{
    public class RepoDataContext : DbContext
    {

        public RepoDataContext(DbContextOptions<RepoDataContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RepoAccount>().HasKey(c => new {c.SourceRepoId, c.AccountId});

            modelBuilder.Entity<Model.Project>()
            .HasOne(p => p.SourceRepository)
            .WithMany(b => b.Projects)
            .HasForeignKey(p => p.SourceRepoId)
            .HasPrincipalKey(b => b.Id);

            modelBuilder.Entity<Model.RepoAccount>()
                .HasOne(x => x.SourceRepository)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.SourceRepoId)
                .HasPrincipalKey(x => x.Id);

            modelBuilder.Entity<Model.RepoAccount>().HasOne(x => x.Person)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.PersonId)
                .HasPrincipalKey(x => x.Id);

            modelBuilder.Entity<Model.Commit>()
                .HasOne(x => x.AuthorRepoAccount)
                .WithMany(x => x.Commits)
                .HasForeignKey(x => new {x.VSCRepositoryId, x.VSCAuthorAccountId})
                .HasPrincipalKey(x => new {x.SourceRepoId, x.AccountId});

            //modelBuilder.Entity<Model.Commit>()
            //    .HasOne(x => x.Author)
            //    .WithMany(x => x.Commits)
            //    .HasForeignKey(x => x.AuthorId)
            //    .HasPrincipalKey(x => x.Id);
        }

        public DbSet<Model.Commit> Commits { get; set; }
        public DbSet<Model.Person> Persons { get; set; }
        public DbSet<Model.RepoAccount> RepositoryAccounts { get; set; }
        public DbSet<Model.VCSRepository> VCSRepositories { get; set; }
        public DbSet<Model.Project> Projects { get; set; }
    }
}
