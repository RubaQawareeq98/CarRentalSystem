using System.Data;
using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Helpers;

public static class SearchDataTableHelper
{
    public static DataTable CreateSearchCriteriaDataTable(CarSearchDto carSearchDto)
    {
        var dt = new DataTable();
        dt.Columns.Add("Brand", typeof(string));
        dt.Columns.Add("Model", typeof(string));
        dt.Columns.Add("MinPrice", typeof(decimal));
        dt.Columns.Add("MaxPrice", typeof(decimal));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("StartDate", typeof(DateTime));
        dt.Columns.Add("EndDate", typeof(DateTime));
        dt.Columns.Add("MinYear", typeof(int));
        dt.Columns.Add("MaxYear", typeof(int));
        dt.Columns.Add("Color", typeof(string));
    
        dt.Rows.Add(
            carSearchDto.Brand,
            carSearchDto.Model,
            carSearchDto.MinPrice ?? (object)DBNull.Value,
            carSearchDto.MaxPrice ?? (object)DBNull.Value,
            carSearchDto.Location,
            carSearchDto.StartDate ?? DateTime.Now, 
            carSearchDto.EndDate ?? DateTime.Now.AddDays(7),
            carSearchDto.MinYear ?? (object)DBNull.Value,
            carSearchDto.MaxYear ?? (object)DBNull.Value,
            carSearchDto.Color
        );
    
        return dt;
    }
}