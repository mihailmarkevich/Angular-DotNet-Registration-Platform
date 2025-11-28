using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "registration",
                table: "Industries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Manufacturing" },
                    { 2, "IT Services" },
                    { 3, "Healthcare" },
                    { 4, "Finance" },
                    { 5, "Retail" },
                    { 6, "Logistics" },
                    { 7, "Education" },
                    { 8, "Energy" },
                    { 9, "Automotive" },
                    { 10, "Food & Beverage" }
                });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "Companies",
                columns: new[] { "Id", "CreatedAt", "IndustryId", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Alpha Manufacturing GmbH" },
                    { 2, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Beta IT Solutions AG" },
                    { 3, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "MediCare Kliniken GmbH" },
                    { 4, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, "SecureFinance AG" },
                    { 5, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, "RetailX Handels GmbH" },
                    { 6, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, "LogiTrans Logistics GmbH" },
                    { 7, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 7, "EduTech Academy GmbH" },
                    { 8, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 8, "GreenEnergy Solutions AG" },
                    { 9, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 9, "AutoDrive Systems GmbH" },
                    { 10, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 10, "FreshBite Foods GmbH" }
                });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "Users",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "PrivacyAcceptedAt", "TermsAcceptedAt", "UserName" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "michael.keller@example.com", "Michael", "Keller", new byte[] { 1, 2, 3, 4, 5 }, new byte[] { 9, 8, 7, 6 }, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "michael.keller" },
                    { 2, 1, new DateTime(2024, 2, 2, 12, 0, 0, 0, DateTimeKind.Utc), "sarah.brandt@example.com", "Sarah", "Brandt", new byte[] { 1, 1, 2, 2, 3 }, new byte[] { 4, 4, 5, 5 }, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "sarah.brandt" },
                    { 3, 2, new DateTime(2024, 2, 3, 12, 0, 0, 0, DateTimeKind.Utc), "thomas.fischer@example.com", "Thomas", "Fischer", new byte[] { 5, 4, 3, 2, 1 }, new byte[] { 1, 2, 3, 4 }, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "thomas.fischer" },
                    { 4, 3, new DateTime(2024, 2, 4, 12, 0, 0, 0, DateTimeKind.Utc), "laura.schmidt@example.com", "Laura", "Schmidt", new byte[] { 9, 9, 8, 8, 7 }, new byte[] { 3, 3, 2, 2 }, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "laura.schmidt" },
                    { 5, 4, new DateTime(2024, 2, 5, 12, 0, 0, 0, DateTimeKind.Utc), "jonas.mueller@example.com", "Jonas", "Müller", new byte[] { 7, 7, 7, 7, 7 }, new byte[] { 1, 3, 5, 7 }, new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), "jonas.mueller" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "registration",
                table: "Industries",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
