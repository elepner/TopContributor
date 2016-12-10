using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TopContributor.Common.DataAccess;

namespace TopContributor.Common.Migrations
{
    [DbContext(typeof(RepoDataContext))]
    partial class RepoDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TopContributor.Common.Model.Commit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("Deleted");

                    b.Property<int>("Inserted");

                    b.Property<string>("Message");

                    b.Property<string>("SourceId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Commits");
                });

            modelBuilder.Entity("TopContributor.Common.Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("FullName");

                    b.Property<string>("LastName");

                    b.Property<string>("PrimaryEmail");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("TopContributor.Common.Model.RepoAccount", b =>
                {
                    b.Property<string>("SourceRepoId");

                    b.Property<string>("AccountId");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<int?>("PersonId");

                    b.HasKey("SourceRepoId", "AccountId");

                    b.HasIndex("PersonId");

                    b.ToTable("RepositoryAccounts");
                });

            modelBuilder.Entity("TopContributor.Common.Model.Commit", b =>
                {
                    b.HasOne("TopContributor.Common.Model.Person", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("TopContributor.Common.Model.RepoAccount", b =>
                {
                    b.HasOne("TopContributor.Common.Model.Person")
                        .WithMany("Accounts")
                        .HasForeignKey("PersonId");
                });
        }
    }
}
