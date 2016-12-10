﻿using System;
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
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("Deletions");

                    b.Property<int>("Insertions");

                    b.Property<string>("Message");

                    b.Property<string>("ProjectId");

                    b.Property<string>("SourceId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SourceId");

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

            modelBuilder.Entity("TopContributor.Common.Model.Project", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("SourceRepoId");

                    b.HasKey("Id");

                    b.HasIndex("SourceRepoId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TopContributor.Common.Model.RepoAccount", b =>
                {
                    b.Property<string>("SourceRepoId");

                    b.Property<string>("AccountId");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<int>("PersonId");

                    b.HasKey("SourceRepoId", "AccountId");

                    b.HasIndex("PersonId");

                    b.ToTable("RepositoryAccounts");
                });

            modelBuilder.Entity("TopContributor.Common.Model.VCSRepository", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CrawlerParams");

                    b.Property<string>("CrawlerProviderType");

                    b.HasKey("Id");

                    b.ToTable("VCSRepositories");
                });

            modelBuilder.Entity("TopContributor.Common.Model.Commit", b =>
                {
                    b.HasOne("TopContributor.Common.Model.Person", "Author")
                        .WithMany("Commits")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TopContributor.Common.Model.VCSRepository", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("TopContributor.Common.Model.Project", b =>
                {
                    b.HasOne("TopContributor.Common.Model.VCSRepository", "SourceRepository")
                        .WithMany("Projects")
                        .HasForeignKey("SourceRepoId");
                });

            modelBuilder.Entity("TopContributor.Common.Model.RepoAccount", b =>
                {
                    b.HasOne("TopContributor.Common.Model.Person", "Person")
                        .WithMany("Accounts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TopContributor.Common.Model.VCSRepository", "SourceRepository")
                        .WithMany("Accounts")
                        .HasForeignKey("SourceRepoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
