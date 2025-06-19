using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBD_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataToSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "Address", "Email", "IsDeleted", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main St, City, Country", "company1@gmail.com", false, "123456789" },
                    { 2, "456 Elm St, City, Country", "company2@gmail.com", false, "987654321" },
                    { 3, "789 Oak St, City, Country", "person3@gmail.com", false, "555555555" },
                    { 4, "321 Pine St, City, Country", "person4@gmail.com", false, "444444444" }
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "DiscountId", "ApplicableTo", "DiscountPercentage", "EndDate", "StartDate" },
                values: new object[,]
                {
                    { 1, "contract", 0.10m, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "contract", 0.15m, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "subscription", 0.20m, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "SoftwareId", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Education", "Software for Java developers", "Intellij IDEA" },
                    { 2, "Education", "Software for .NET developers", "Visual Studio" },
                    { 3, "Education", "Software for Java developers", "Eclipse" },
                    { 4, "Education", "Software for Python developers", "PyCharm" },
                    { 5, "Education", "Software for JavaScript developers", "WebStorm" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "ClientId", "CompanyName", "Krs" },
                values: new object[,]
                {
                    { 1, "Google", "1234567890" },
                    { 2, "Microsoft", "0987654321" }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "ClientId", "FirstName", "LastName", "Pesel" },
                values: new object[,]
                {
                    { 3, "John", "Doe", "12345678901" },
                    { 4, "Jane", "Smith", "10987654321" }
                });

            migrationBuilder.InsertData(
                table: "software_version",
                columns: new[] { "SoftwareVersionId", "BasePrice", "ReleaseDate", "SoftwareId", "Version" },
                values: new object[,]
                {
                    { 1, 1000m, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "2023.1" },
                    { 2, 1200m, new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "17.0.1" },
                    { 3, 1200m, new DateTime(2023, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "17.0.2" },
                    { 4, 800m, new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "4.25" },
                    { 5, 800m, new DateTime(2023, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "4.26" },
                    { 6, 900m, new DateTime(2023, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "2023.2" },
                    { 7, 1100m, new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "2023.1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "DiscountId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "ClientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "ClientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "software_version",
                keyColumn: "SoftwareVersionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "ClientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "ClientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 5);
        }
    }
}
