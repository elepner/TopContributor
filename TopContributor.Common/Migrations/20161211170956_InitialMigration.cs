﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TopContributor.Common.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PrimaryEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VCSRepositories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CrawlerParams = table.Column<string>(nullable: true),
                    CrawlerProviderType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VCSRepositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SourceRepoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_VCSRepositories_SourceRepoId",
                        column: x => x.SourceRepoId,
                        principalTable: "VCSRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepositoryAccounts",
                columns: table => new
                {
                    SourceRepoId = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: false),
                    CommitId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryAccounts", x => new { x.SourceRepoId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_RepositoryAccounts_Users_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryAccounts_VCSRepositories_SourceRepoId",
                        column: x => x.SourceRepoId,
                        principalTable: "VCSRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Deletions = table.Column<int>(nullable: false),
                    Insertions = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true),
                    SourceId = table.Column<string>(nullable: true),
                    VSCAuthorAccountId = table.Column<string>(nullable: true),
                    VSCRepositoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commits_VCSRepositories_SourceId",
                        column: x => x.SourceId,
                        principalTable: "VCSRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commits_RepositoryAccounts_VSCRepositoryId_VSCAuthorAccountId",
                        columns: x => new { x.VSCRepositoryId, x.VSCAuthorAccountId },
                        principalTable: "RepositoryAccounts",
                        principalColumns: new[] { "SourceRepoId", "AccountId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commits_SourceId",
                table: "Commits",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_VSCRepositoryId_VSCAuthorAccountId",
                table: "Commits",
                columns: new[] { "VSCRepositoryId", "VSCAuthorAccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SourceRepoId",
                table: "Projects",
                column: "SourceRepoId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryAccounts_PersonId",
                table: "RepositoryAccounts",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "RepositoryAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VCSRepositories");
        }
    }
}
