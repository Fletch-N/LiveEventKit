using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class _20260624223928_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Bio = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                Industry = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                Organization = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                State = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                Interests = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                Pronouns = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                ProfileImage = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "KitEvents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                Image = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                StartDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KitEvents", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserFollowings",
            columns: table => new
            {
                ObserverId = table.Column<Guid>(type: "TEXT", nullable: false),
                TargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                FollowedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserFollowings", x => new { x.ObserverId, x.TargetId });
                table.ForeignKey(
                    name: "FK_UserFollowings_AspNetUsers_ObserverId",
                    column: x => x.ObserverId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserFollowings_AspNetUsers_TargetId",
                    column: x => x.TargetId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "KitSessions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                SpeakerId = table.Column<Guid>(type: "TEXT", nullable: false),
                Sponsor = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                StartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                AccessLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                Image = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KitSessions", x => x.Id);
                table.ForeignKey(
                    name: "FK_KitSessions_AspNetUsers_SpeakerId",
                    column: x => x.SpeakerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_KitSessions_KitEvents_EventId",
                    column: x => x.EventId,
                    principalTable: "KitEvents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "KitChats",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                SessionId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KitChats", x => x.Id);
                table.ForeignKey(
                    name: "FK_KitChats_KitSessions_SessionId",
                    column: x => x.SessionId,
                    principalTable: "KitSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "KitVideos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                PlaybackUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                StartedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                EndedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                TotalDuration = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KitVideos", x => x.Id);
                table.ForeignKey(
                    name: "FK_KitVideos_KitSessions_SessionId",
                    column: x => x.SessionId,
                    principalTable: "KitSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserAttendings",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                RegisteredAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                CheckedInAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                TimeViewed = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserAttendings", x => new { x.UserId, x.SessionId });
                table.ForeignKey(
                    name: "FK_UserAttendings_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserAttendings_KitSessions_SessionId",
                    column: x => x.SessionId,
                    principalTable: "KitSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ChatEvents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatEvents", x => x.Id);
                table.ForeignKey(
                    name: "FK_ChatEvents_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ChatEvents_KitChats_ChatId",
                    column: x => x.ChatId,
                    principalTable: "KitChats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ChatMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                Body = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                IsHidden = table.Column<bool>(type: "INTEGER", nullable: false),
                AuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ChatMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_ChatMessages_AspNetUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ChatMessages_KitChats_ChatId",
                    column: x => x.ChatId,
                    principalTable: "KitChats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ModerationEvents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                TargetId = table.Column<Guid>(type: "TEXT", nullable: false),
                Command = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ModerationEvents", x => x.Id);
                table.ForeignKey(
                    name: "FK_ModerationEvents_AspNetUsers_TargetId",
                    column: x => x.TargetId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ModerationEvents_KitChats_ChatId",
                    column: x => x.ChatId,
                    principalTable: "KitChats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ShowControls",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ChatId = table.Column<Guid>(type: "TEXT", nullable: false),
                Command = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShowControls", x => x.Id);
                table.ForeignKey(
                    name: "FK_ShowControls_KitChats_ChatId",
                    column: x => x.ChatId,
                    principalTable: "KitChats",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ChatEvents_ChatId_TimeStamp",
            table: "ChatEvents",
            columns: new[] { "ChatId", "TimeStamp" });

        migrationBuilder.CreateIndex(
            name: "IX_ChatEvents_UserId",
            table: "ChatEvents",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_AuthorId",
            table: "ChatMessages",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_ChatMessages_ChatId_TimeStamp",
            table: "ChatMessages",
            columns: new[] { "ChatId", "TimeStamp" });

        migrationBuilder.CreateIndex(
            name: "IX_KitChats_SessionId",
            table: "KitChats",
            column: "SessionId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_KitEvents_StartDate",
            table: "KitEvents",
            column: "StartDate");

        migrationBuilder.CreateIndex(
            name: "IX_KitSessions_EventId",
            table: "KitSessions",
            column: "EventId");

        migrationBuilder.CreateIndex(
            name: "IX_KitSessions_SpeakerId",
            table: "KitSessions",
            column: "SpeakerId");

        migrationBuilder.CreateIndex(
            name: "IX_KitSessions_StartTime",
            table: "KitSessions",
            column: "StartTime");

        migrationBuilder.CreateIndex(
            name: "IX_KitVideos_SessionId",
            table: "KitVideos",
            column: "SessionId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ModerationEvents_ChatId_TimeStamp",
            table: "ModerationEvents",
            columns: new[] { "ChatId", "TimeStamp" });

        migrationBuilder.CreateIndex(
            name: "IX_ModerationEvents_TargetId",
            table: "ModerationEvents",
            column: "TargetId");

        migrationBuilder.CreateIndex(
            name: "IX_ShowControls_ChatId_TimeStamp",
            table: "ShowControls",
            columns: new[] { "ChatId", "TimeStamp" });

        migrationBuilder.CreateIndex(
            name: "IX_UserAttendings_SessionId",
            table: "UserAttendings",
            column: "SessionId");

        migrationBuilder.CreateIndex(
            name: "IX_UserFollowings_TargetId",
            table: "UserFollowings",
            column: "TargetId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "ChatEvents");

        migrationBuilder.DropTable(
            name: "ChatMessages");

        migrationBuilder.DropTable(
            name: "KitVideos");

        migrationBuilder.DropTable(
            name: "ModerationEvents");

        migrationBuilder.DropTable(
            name: "ShowControls");

        migrationBuilder.DropTable(
            name: "UserAttendings");

        migrationBuilder.DropTable(
            name: "UserFollowings");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "KitChats");

        migrationBuilder.DropTable(
            name: "KitSessions");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "KitEvents");
    }
}
