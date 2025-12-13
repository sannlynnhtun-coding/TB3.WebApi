using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace TB3.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryDapperController : ControllerBase
{
    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
    {
        DataSource = ".",
        InitialCatalog = "Batch3MiniPOS",
        UserID = "sa",
        Password = "sasa@123",
        TrustServerCertificate = true,
    };

    [HttpGet]
    public IActionResult GetProductCategories()
    {
        using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        {
            string query = @"SELECT [ProductCategoryId]
                                  ,[ProductCategoryCode]
                                  ,[ProductCategoryName]
                                  ,[DeleteFlag]
                                  ,[CreatedDateTime]
                                  ,[ModifiedDateTime]
                                FROM [dbo].[TblProductCategory]
                                WHERE DeleteFlag = 0";
            db.Open();
            List<ProductCategoryResponseDto> lst = db.Query<ProductCategoryResponseDto>(query).ToList();
            return Ok(lst);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetProductCategory(int id)
    {
        var item = findById(id);
        if (item is null)
        {
            return NotFound("No item found.");
        }

        var response = new ProductCategoryResponseDto
        {
            ProductCategoryCode = item.ProductCategoryCode,
            ProductCategoryName = item.ProductCategoryName,
            CreatedDateTime = item.CreatedDateTime,
            ModifiedDateTime = item.ModifiedDateTime,
        };
        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto requestDto)
    {
        using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        {
            string query = @"INSERT INTO [dbo].[TblProductCategory]
                                   ([ProductCategoryCode]
                                   ,[ProductCategoryName]
                                   ,[DeleteFlag]
                                   ,[CreatedDateTime]
                                   ,[ModifiedDateTime])
                             VALUES
                                   (@ProductCategoryCode
                                   ,@ProductCategoryName
                                   ,@DeleteFlag
                                   ,@CreatedDateTime
                                   ,NULL)";
            db.Open();

            int result = db.Execute(query, new { 
                ProductCategoryCode = requestDto.ProductCategoryCode,
                ProductCategoryName = requestDto.ProductCategoryName, 
                DeleteFlag = false, 
                CreatedDateTime = DateTime.Now
            });
            string message = result > 0 ? "Creation Successful" : "Creation Failed";
            return Ok(message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto requestDto)
    {
        var item = findById(id);
        if (item is null)
        {
            return NotFound("No item found.");
        }

        using(IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        {
            string query = @"UPDATE [dbo].[TblProductCategory]
                               SET [ProductCategoryCode] = @ProductCategoryCode
                                  ,[ProductCategoryName] = @ProductCategoryName
                                  ,[ModifiedDateTime] = @ModifiedDateTime
                             WHERE ProductCategoryId = @ProductCategoryId";

            db.Open();
            int result = db.Execute(query, new {
                ProductCategoryId = item.ProductCategoryId, 
                ProductCategoryCode = requestDto.ProductCategoryCode, 
                ProductCategoryName = requestDto.ProductCategoryName, 
                ModifiedDateTime = DateTime.Now
            });
            string message = result > 0 ? "Update Successful" : "Update Failed";
            return Ok(message);
        }
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProductCategory(int id, ProductCategoryPatchRequestDto requestDto)
    {
        var item = findById(id);
        if (item is null)
        {
            return NotFound("No item found.");
        }

        string condition = string.Empty;

        if (!string.IsNullOrEmpty(requestDto.ProductCategoryCode))
        {
            condition += "[ProductCategoryCode] = @ProductCategoryCode, ";
        }

        if (!string.IsNullOrEmpty(requestDto.ProductCategoryName))
        {
            condition += "[ProductCategoryName] = @ProductCategoryName, ";
        }

        if (condition.Length == 0)
        {
            return BadRequest("No data to update.");
        }

        condition += "[ModifiedDateTime] = @ModifiedDateTime, ";

        condition = condition.Substring(0, condition.Length - 2);

        string query = $@"
        UPDATE [dbo].[TblProductCategory]
        SET {condition}
        WHERE ProductCategoryId = @ProductCategoryId";

        using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        int result = db.Execute(query, new
        {
            ProductCategoryId = item.ProductCategoryId,
            ProductCategoryCode = requestDto.ProductCategoryCode,
            ProductCategoryName = requestDto.ProductCategoryName,
            ModifiedDateTime = DateTime.Now,
        });

        string message = result > 0 ? "Patching Successful" : "Patching Failed";

        return Ok(message);
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {
        var item = findById(id);
        if (item is null)
        {
            return NotFound("No item found.");
        }

        using(IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        {
            string query = @"UPDATE [dbo].[TblProductCategory]
                               SET [DeleteFlag] = 1
                             WHERE ProductCategoryId = @ProductCategoryId";

            db.Open();
            int result = db.Execute(query, new { ProductCategoryId = id});
            string message = result > 0 ? "Delete Successful" : "Delete Failed";
            return Ok(message);
        }
    }

    private ProductCategoryDto? findById(int id)
    {
        using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
        {
            string query = @"SELECT [ProductCategoryId]
                                  ,[ProductCategoryCode]
                                  ,[ProductCategoryName]
                                  ,[DeleteFlag]
                                  ,[CreatedDateTime]
                                  ,[ModifiedDateTime]
                                FROM [dbo].[TblProductCategory]
                                WHERE ProductCategoryId = @ProductCategoryId AND DeleteFlag = 0";
            db.Open();

            var item = db.Query<ProductCategoryDto>(query, new { ProductCategoryId = id }).FirstOrDefault();
            return item;
        }
    }
}

public class ProductCategoryDto
{
    public int? ProductCategoryId { get; set; }
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
    public bool? DeleteFlag { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}

public class ProductCategoryResponseDto
{
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}