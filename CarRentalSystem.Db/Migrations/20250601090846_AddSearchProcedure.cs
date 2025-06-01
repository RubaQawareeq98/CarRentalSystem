using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalSystem.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TYPE dbo.CarSearchCriteria AS TABLE
                (
                    Brand VARCHAR(50),
                    Model VARCHAR(50),
                    MinPrice DECIMAL(10,2),
                    MaxPrice DECIMAL(10,2),
                    Location VARCHAR(30),
                    StartDate DATETIME,
                    EndDate DATETIME,
                    MinYear INT,
                    MaxYear INT,
                    Color VARCHAR(50)
                );
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_SearchCars
                    @SearchCriteria dbo.CarSearchCriteria READONLY
                AS
                BEGIN
                    SELECT c.* 
                    FROM Cars c
                    INNER JOIN @SearchCriteria sc ON 
                        (sc.Brand IS NULL OR c.Brand = sc.Brand) AND
                        (sc.Model IS NULL OR c.Model = sc.Model) AND
                        (sc.MinPrice IS NULL OR c.Price >= sc.MinPrice) AND
                        (sc.MaxPrice IS NULL OR c.Price <= sc.MaxPrice) AND
                        (sc.Location IS NULL OR c.Location = sc.Location) AND
                        (sc.MinYear IS NULL OR c.Year >= sc.MinYear) AND
                        (sc.MaxYear IS NULL OR c.Year <= sc.MaxYear) AND
                        (sc.Color IS NULL OR c.Color = sc.Color)
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_SearchCars;");
            migrationBuilder.Sql("DROP TYPE IF EXISTS dbo.CarSearchCriteria;");
        }
    }
}
