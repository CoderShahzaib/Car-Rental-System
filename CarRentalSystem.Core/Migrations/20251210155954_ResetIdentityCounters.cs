using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalSystem.Core.Migrations
{
    /// <inheritdoc />
    public partial class ResetIdentityCounters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DBCC CHECKIDENT ('Cars', RESEED, 1);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Customers', RESEED, 1);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Bookings', RESEED, 1);");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
