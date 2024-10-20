using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "kanban");

            migrationBuilder.CreateTable(
                name: "Boards",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PermissionIds = table.Column<List<int>>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BoardId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BoardId1 = table.Column<long>(type: "bigint", nullable: true),
                    BoardId2 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "kanban",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Columns_Boards_BoardId1",
                        column: x => x.BoardId1,
                        principalSchema: "kanban",
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Columns_Boards_BoardId2",
                        column: x => x.BoardId2,
                        principalSchema: "kanban",
                        principalTable: "Boards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoardId = table.Column<long>(type: "bigint", nullable: false),
                    ColumnId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartAt = table.Column<DateOnly>(type: "date", nullable: true),
                    EndAt = table.Column<DateOnly>(type: "date", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Tags = table.Column<List<string>>(type: "text[]", nullable: true),
                    Members = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    BoardId1 = table.Column<long>(type: "bigint", nullable: true),
                    ColumnId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "kanban",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Boards_BoardId1",
                        column: x => x.BoardId1,
                        principalSchema: "kanban",
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalSchema: "kanban",
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Columns_ColumnId1",
                        column: x => x.ColumnId1,
                        principalSchema: "kanban",
                        principalTable: "Columns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Edited = table.Column<bool>(type: "boolean", nullable: false),
                    CardId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "kanban",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Cards_CardId1",
                        column: x => x.CardId1,
                        principalSchema: "kanban",
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserConfig",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfCards = table.Column<long>(type: "bigint", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "kanban",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserConfigId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "kanban",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalSchema: "kanban",
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_UserConfig_UserConfigId",
                        column: x => x.UserConfigId,
                        principalSchema: "kanban",
                        principalTable: "UserConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "kanban",
                table: "Boards",
                columns: new[] { "Id", "Color", "Description", "Icon", "Name" },
                values: new object[] { 1L, "blue", "Seu primeiro Quadro", "board", "Bem Vindo" });

            migrationBuilder.InsertData(
                schema: "kanban",
                table: "Roles",
                columns: new[] { "Id", "Description", "Name", "PermissionIds" },
                values: new object[,]
                {
                    { 1L, "Adiministrador do Sistema", "Admin", new List<int> { 0, 1, 2 } },
                    { 2L, "Usuário base do Sistema", "User", new List<int> { 0, 1 } }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_Id",
                schema: "kanban",
                table: "Boards",
                column: "Id")
                .Annotation("Npgsql:CreatedConcurrently", true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId",
                schema: "kanban",
                table: "Cards",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId1",
                schema: "kanban",
                table: "Cards",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ColumnId",
                schema: "kanban",
                table: "Cards",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ColumnId1",
                schema: "kanban",
                table: "Cards",
                column: "ColumnId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Id",
                schema: "kanban",
                table: "Cards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId",
                schema: "kanban",
                table: "Columns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId1",
                schema: "kanban",
                table: "Columns",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId2",
                schema: "kanban",
                table: "Columns",
                column: "BoardId2");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_Id",
                schema: "kanban",
                table: "Columns",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CardId",
                schema: "kanban",
                table: "Comments",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CardId1",
                schema: "kanban",
                table: "Comments",
                column: "CardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Id",
                schema: "kanban",
                table: "Comments",
                column: "Id")
                .Annotation("Npgsql:CreatedConcurrently", true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                schema: "kanban",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId1",
                schema: "kanban",
                table: "Comments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Id",
                schema: "kanban",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfig_Icon",
                schema: "kanban",
                table: "UserConfig",
                column: "Icon");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfig_UserId",
                schema: "kanban",
                table: "UserConfig",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "kanban",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "kanban",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId1",
                schema: "kanban",
                table: "Users",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserConfigId",
                schema: "kanban",
                table: "Users",
                column: "UserConfigId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "kanban",
                table: "Comments",
                column: "UserId",
                principalSchema: "kanban",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId1",
                schema: "kanban",
                table: "Comments",
                column: "UserId1",
                principalSchema: "kanban",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConfig_Users_UserId",
                schema: "kanban",
                table: "UserConfig",
                column: "UserId",
                principalSchema: "kanban",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConfig_Users_UserId",
                schema: "kanban",
                table: "UserConfig");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "Columns",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "Boards",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "kanban");

            migrationBuilder.DropTable(
                name: "UserConfig",
                schema: "kanban");
        }
    }
}
