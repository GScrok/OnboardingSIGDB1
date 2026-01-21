using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingSIGDB1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIdInEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Cargo",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Funcionario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_CompanyId",
                table: "Funcionario",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Empresa_CompanyId",
                table: "Funcionario",
                column: "CompanyId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Empresa_CompanyId",
                table: "Funcionario");

            migrationBuilder.DropIndex(
                name: "IX_Funcionario_CompanyId",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Funcionario");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Cargo",
                newName: "Descricao");
        }
    }
}
