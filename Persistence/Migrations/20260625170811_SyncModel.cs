using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class _20260625170811_SyncModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatEvents_KitChats_ChatId",
            table: "ChatEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessages_KitChats_ChatId",
            table: "ChatMessages");

        migrationBuilder.DropForeignKey(
            name: "FK_KitChats_KitSessions_SessionId",
            table: "KitChats");

        migrationBuilder.DropForeignKey(
            name: "FK_KitSessions_AspNetUsers_SpeakerId",
            table: "KitSessions");

        migrationBuilder.DropForeignKey(
            name: "FK_KitSessions_KitEvents_EventId",
            table: "KitSessions");

        migrationBuilder.DropForeignKey(
            name: "FK_KitVideos_KitSessions_SessionId",
            table: "KitVideos");

        migrationBuilder.DropForeignKey(
            name: "FK_ModerationEvents_KitChats_ChatId",
            table: "ModerationEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_ShowControls_KitChats_ChatId",
            table: "ShowControls");

        migrationBuilder.DropForeignKey(
            name: "FK_UserAttendings_KitSessions_SessionId",
            table: "UserAttendings");

        migrationBuilder.DropPrimaryKey(
            name: "PK_KitVideos",
            table: "KitVideos");

        migrationBuilder.DropPrimaryKey(
            name: "PK_KitSessions",
            table: "KitSessions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_KitEvents",
            table: "KitEvents");

        migrationBuilder.DropPrimaryKey(
            name: "PK_KitChats",
            table: "KitChats");

        migrationBuilder.RenameTable(
            name: "KitVideos",
            newName: "Videos");

        migrationBuilder.RenameTable(
            name: "KitSessions",
            newName: "Sessions");

        migrationBuilder.RenameTable(
            name: "KitEvents",
            newName: "Events");

        migrationBuilder.RenameTable(
            name: "KitChats",
            newName: "Chats");

        migrationBuilder.RenameIndex(
            name: "IX_KitVideos_SessionId",
            table: "Videos",
            newName: "IX_Videos_SessionId");

        migrationBuilder.RenameIndex(
            name: "IX_KitSessions_StartTime",
            table: "Sessions",
            newName: "IX_Sessions_StartTime");

        migrationBuilder.RenameIndex(
            name: "IX_KitSessions_SpeakerId",
            table: "Sessions",
            newName: "IX_Sessions_SpeakerId");

        migrationBuilder.RenameIndex(
            name: "IX_KitSessions_EventId",
            table: "Sessions",
            newName: "IX_Sessions_EventId");

        migrationBuilder.RenameIndex(
            name: "IX_KitEvents_StartDate",
            table: "Events",
            newName: "IX_Events_StartDate");

        migrationBuilder.RenameIndex(
            name: "IX_KitChats_SessionId",
            table: "Chats",
            newName: "IX_Chats_SessionId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Videos",
            table: "Videos",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Sessions",
            table: "Sessions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Events",
            table: "Events",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Chats",
            table: "Chats",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatEvents_Chats_ChatId",
            table: "ChatEvents",
            column: "ChatId",
            principalTable: "Chats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessages_Chats_ChatId",
            table: "ChatMessages",
            column: "ChatId",
            principalTable: "Chats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Chats_Sessions_SessionId",
            table: "Chats",
            column: "SessionId",
            principalTable: "Sessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ModerationEvents_Chats_ChatId",
            table: "ModerationEvents",
            column: "ChatId",
            principalTable: "Chats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Sessions_AspNetUsers_SpeakerId",
            table: "Sessions",
            column: "SpeakerId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Sessions_Events_EventId",
            table: "Sessions",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ShowControls_Chats_ChatId",
            table: "ShowControls",
            column: "ChatId",
            principalTable: "Chats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_UserAttendings_Sessions_SessionId",
            table: "UserAttendings",
            column: "SessionId",
            principalTable: "Sessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Videos_Sessions_SessionId",
            table: "Videos",
            column: "SessionId",
            principalTable: "Sessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ChatEvents_Chats_ChatId",
            table: "ChatEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_ChatMessages_Chats_ChatId",
            table: "ChatMessages");

        migrationBuilder.DropForeignKey(
            name: "FK_Chats_Sessions_SessionId",
            table: "Chats");

        migrationBuilder.DropForeignKey(
            name: "FK_ModerationEvents_Chats_ChatId",
            table: "ModerationEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_Sessions_AspNetUsers_SpeakerId",
            table: "Sessions");

        migrationBuilder.DropForeignKey(
            name: "FK_Sessions_Events_EventId",
            table: "Sessions");

        migrationBuilder.DropForeignKey(
            name: "FK_ShowControls_Chats_ChatId",
            table: "ShowControls");

        migrationBuilder.DropForeignKey(
            name: "FK_UserAttendings_Sessions_SessionId",
            table: "UserAttendings");

        migrationBuilder.DropForeignKey(
            name: "FK_Videos_Sessions_SessionId",
            table: "Videos");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Videos",
            table: "Videos");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Sessions",
            table: "Sessions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Events",
            table: "Events");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Chats",
            table: "Chats");

        migrationBuilder.RenameTable(
            name: "Videos",
            newName: "KitVideos");

        migrationBuilder.RenameTable(
            name: "Sessions",
            newName: "KitSessions");

        migrationBuilder.RenameTable(
            name: "Events",
            newName: "KitEvents");

        migrationBuilder.RenameTable(
            name: "Chats",
            newName: "KitChats");

        migrationBuilder.RenameIndex(
            name: "IX_Videos_SessionId",
            table: "KitVideos",
            newName: "IX_KitVideos_SessionId");

        migrationBuilder.RenameIndex(
            name: "IX_Sessions_StartTime",
            table: "KitSessions",
            newName: "IX_KitSessions_StartTime");

        migrationBuilder.RenameIndex(
            name: "IX_Sessions_SpeakerId",
            table: "KitSessions",
            newName: "IX_KitSessions_SpeakerId");

        migrationBuilder.RenameIndex(
            name: "IX_Sessions_EventId",
            table: "KitSessions",
            newName: "IX_KitSessions_EventId");

        migrationBuilder.RenameIndex(
            name: "IX_Events_StartDate",
            table: "KitEvents",
            newName: "IX_KitEvents_StartDate");

        migrationBuilder.RenameIndex(
            name: "IX_Chats_SessionId",
            table: "KitChats",
            newName: "IX_KitChats_SessionId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_KitVideos",
            table: "KitVideos",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_KitSessions",
            table: "KitSessions",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_KitEvents",
            table: "KitEvents",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_KitChats",
            table: "KitChats",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ChatEvents_KitChats_ChatId",
            table: "ChatEvents",
            column: "ChatId",
            principalTable: "KitChats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ChatMessages_KitChats_ChatId",
            table: "ChatMessages",
            column: "ChatId",
            principalTable: "KitChats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_KitChats_KitSessions_SessionId",
            table: "KitChats",
            column: "SessionId",
            principalTable: "KitSessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_KitSessions_AspNetUsers_SpeakerId",
            table: "KitSessions",
            column: "SpeakerId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KitSessions_KitEvents_EventId",
            table: "KitSessions",
            column: "EventId",
            principalTable: "KitEvents",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_KitVideos_KitSessions_SessionId",
            table: "KitVideos",
            column: "SessionId",
            principalTable: "KitSessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ModerationEvents_KitChats_ChatId",
            table: "ModerationEvents",
            column: "ChatId",
            principalTable: "KitChats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ShowControls_KitChats_ChatId",
            table: "ShowControls",
            column: "ChatId",
            principalTable: "KitChats",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_UserAttendings_KitSessions_SessionId",
            table: "UserAttendings",
            column: "SessionId",
            principalTable: "KitSessions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
