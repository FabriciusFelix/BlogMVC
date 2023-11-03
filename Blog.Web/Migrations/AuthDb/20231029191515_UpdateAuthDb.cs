using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class UpdateAuthDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a29f552f-d9e5-4093-b6ce-6bc9894cc533",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "Admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "888a6b47-3ad4-41a6-921d-a630b84256c3",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEJpstOucedXAb2ttvahLoF6c8Adh7a4qCCfVMkM/EwHWs8m1yrrmdnpRvyUUr7R72A==", "91d43edf-e2d3-4b32-b95b-7f06a95533c0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a29f552f-d9e5-4093-b6ce-6bc9894cc533",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "admin", "admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "888a6b47-3ad4-41a6-921d-a630b84256c3",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEF15b9tqRpqNImzPkumsrJ1iJ61rg4sZ7N9iOaFZRi1l8H5gpS0r6ly11tdHw7lgJg==", "2b242aad-d2cb-43d4-97b1-1ee00649319a" });
        }
    }
}
