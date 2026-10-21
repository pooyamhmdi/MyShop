using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVariants_ProductVariantId",
                table: "ProductImages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "VariantSpecificationValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "VariantSpecificationValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VariantSpecificationValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "VariantSpecificationValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Specifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Specifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Specifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Specifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SpecificationOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "SpecificationOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SpecificationOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "SpecificationOptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProductVariantId",
                table: "ProductImages",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "ProductImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE Brands
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;

                UPDATE Categories
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;

                UPDATE ProductImages
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;

                UPDATE SpecificationOptions
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;

                UPDATE Specifications
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;

                UPDATE VariantSpecificationValues
                SET CreatedOn = SYSUTCDATETIME()
                WHERE CreatedOn IS NULL;
                """);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "VariantSpecificationValues",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Specifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "SpecificationOptions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Brands",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVariants_ProductVariantId",
                table: "ProductImages",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVariants_ProductVariantId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "VariantSpecificationValues");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "VariantSpecificationValues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VariantSpecificationValues");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "VariantSpecificationValues");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Specifications");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SpecificationOptions");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "SpecificationOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SpecificationOptions");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "SpecificationOptions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Brands");

            migrationBuilder.AlterColumn<long>(
                name: "ProductVariantId",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVariants_ProductVariantId",
                table: "ProductImages",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}