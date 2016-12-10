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
        }

        public DbSet<Model.Commit> Commits { get; set; }
        public DbSet<Model.Person> Persons { get; set; }
        public DbSet<Model.RepoAccount> RepositoryAccounts { get; set; }
    }
}
