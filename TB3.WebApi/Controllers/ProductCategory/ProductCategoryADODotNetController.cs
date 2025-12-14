using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using TB3.Database.AppDbContextModels;

namespace TB3.WebApi.Controllers.ProductCategory;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryAdoDotNetController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

    public ProductCategoryAdoDotNetController()
    {
        _db = new AppDbContext();
        _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "Batch3MiniPOS",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate = true,
        };
    }

    [HttpGet]
    public IActionResult GetProductCategories()
    {
        SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        string query = @"SELECT [ProductCategoryId]
      ,[ProductCategoryCode]
      ,[ProductCategoryName]     
  FROM [dbo].[Tbl_ProductCategory]";

        SqlCommand cmd = new SqlCommand(query, connection);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);

        connection.Close();

        var lst = new List<ProductCategory>();

        foreach (DataRow row in dt.Rows)
        {
            lst.Add(new ProductCategory
            {
                ProductCategoryId = Convert.ToInt32(row["ProductCategoryId"]),
                ProductCategoryCode = row["ProductCategoryCode"].ToString(),
                ProductCategoryName = row["ProductCategoryName"].ToString()
            });
        }

        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        string query = "SELECT* FROM Tbl_ProductCategory WHERE ProductCategoryId = @id";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        connection.Close();

        if (dt.Rows.Count == 0)
        {
            return NotFound("Product not found.");
        }

        DataRow row = dt.Rows[0];

        var response = new ProductCategoryGetResponseDto
        {
            ProductCategoryCode = row["ProductCategoryCode"].ToString(),
            ProductCategoryName = row["ProductCategoryName"].ToString()
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);

        connection.Open();

        string query = @"INSERT INTO [dbo].[Tbl_ProductCategory]
           ([ProductCategoryCode]
           ,[ProductCategoryName])
     VALUES
           (@code
           ,@name)";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@code", request.ProductCategoryCode);
        cmd.Parameters.AddWithValue("@name", request.ProductCategoryName);
        int result = cmd.ExecuteNonQuery();

        connection.Close();

        string message = result > 0 ? "Successfully Saved!" : "NOT Saved!";

        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        connection.Open();

        string checkQuery = "SELECT count(*) FROM Tbl_ProductCategory WHERE ProductCategoryId = @id";
        SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
        checkCmd.Parameters.AddWithValue("@id", id);

        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

        connection.Close();

        if (count == 0)
        {
            return NotFound("Product Category not found!");
        }

        string updateQuery = @"UPDATE [dbo].[Tbl_ProductCategory]
   SET [ProductCategoryCode] = @code
      ,[ProductCategoryName] =@name
 WHERE ProductCategoryId = @id";
        connection.Open();
        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
        updateCommand.Parameters.AddWithValue("@id", id);
        int updateCount = 0;

        if (!string.IsNullOrEmpty(request.ProductCategoryCode))
        {
            updateCount++;
            updateCommand.Parameters.AddWithValue("@code", request.ProductCategoryCode);
        }

        if (!string.IsNullOrEmpty(request.ProductCategoryName))
        {
            updateCount++;
            updateCommand.Parameters.AddWithValue("@name", request.ProductCategoryName);
        }

        if (updateCount < 2)
        {
            return BadRequest("Please provide all required fields!");
        }

        int result = updateCommand.ExecuteNonQuery();

        connection.Close();

        string message = result > 0 ? "Updated!" : "Not Updated!";

        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {
        SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
        string checkQuery = "SELECT count(*) FROM Tbl_ProductCategory WHERE ProductCategoryId = @id";
        string deleteQuery = @"delete from Tbl_ProductCategory WHERE ProductCategoryId = @id";

        connection.Open();
        SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
        checkCmd.Parameters.AddWithValue("@id", id);

        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

        connection.Close();

        if (count == 0)
        {
            return NotFound("Product Category not found!");
        }

        connection.Open();
        SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
        deleteCmd.Parameters.AddWithValue("@id", id);

        int result = deleteCmd.ExecuteNonQuery();

        connection.Close();

        string message = result > 0 ? "Deleted!" : "Not Deleted!";

        return Ok(message);

    }

}

public class ProductCategory
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
}