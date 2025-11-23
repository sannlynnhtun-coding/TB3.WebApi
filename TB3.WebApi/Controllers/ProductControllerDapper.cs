using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Data;
using TB3.Database.AppDbContextModels;

namespace TB3.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductControllerDapper : ControllerBase
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = "LAPTOP-7EKI2OO3\\SQLEXPRESS",
            InitialCatalog = "ProductnSale",
            UserID = "mylogin",
            Password = "nyan123",
            TrustServerCertificate = true
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = "select * from Tbl_Product where DeleteFlag = 0";
                var lst = db.Query(query).ToList();
                return Ok(lst);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = "select * from Tbl_Product where DeleteFlag = 0 and ProductId = @id";
                var item = db.QueryFirstOrDefault(query, new { id = id });
                
                if (item is null)
                {
                    return NotFound("Product not found.");
                }

                var response = new ProductGetResponseDto
                {
                    ProductName = item.ProductName
                };
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductCreateRequestDto request)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = @"INSERT INTO [dbo].[Tbl_Product]
           ([ProductName]
           ,[Quantity]
           ,[Price]
           ,[DeleteFlag]
           ,[CreatedDateTime])
     VALUES(@ProductName, @Quantity, @Price, 0, @DateTime)";
                int result = db.Execute(query, new
                {
                    DateTime = DateTime.Now,
                    Price = request.Price,
                    DeleteFlag = false,
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                });
                
                string message = result > 0 ? "Saving Successful." : "Saving Failed.";

                return Ok(message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductUpdateRequestDto request)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = @"UPDATE [dbo].[Tbl_Product]
   SET ProductName = @ProductName, Quantity = @Quantity, Price = @Price, ModifiedDateTime = @ModifiedDateTime where ProductId = @ProductId";

                int rowAffected = db.Execute(query, new
                {
                    ProductId = id,
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                    Price = request.Price,
                    ModifiedDateTime = DateTime.Now
                });
                string message = rowAffected > 0 ? "Successfully updated" : "Fail to update";
                return Ok(message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, ProductPatchRequestDto request)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = @"UPDATE [dbo].[Tbl_Product] SET ModifiedDateTime = @ModifiedDateTime, ";
                if (!string.IsNullOrEmpty(request.ProductName))
                {
                    query += "ProductName = @ProductName, ";
                }
                if (request.Price is not null && request.Price > 0)
                {
                    query += "Price = @Price, ";
                }
                if (request.Quantity is not null && request.Quantity > 0)
                {
                    query += "Quantity = @Quantity, ";
                }
                query = query.TrimEnd(',', ' ');

                query += " where ProductId = @ProductId;";

                int result = db.Execute(query, new
                {
                    ProductId = id,
                    ModifiedDateTime = DateTime.Now,
                    Price = request.Price,                 
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                });

                string message = result > 0 ? "Patching Successful." : "Patching Failed.";

                return Ok(message);

            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();
                string query = @"UPDATE [dbo].[Tbl_Product]
   SET DeleteFlag = 1, ModifiedDateTime = @ModifiedDateTime where ProductId = @ProductId";

                int result = db.Execute(query, new
                {
                    ProductId = id,
                    ModifiedDateTime = DateTime.Now

                });

                string message = result > 0 ? "Successfully deleted" : "Fail to delete";
                return Ok(message);
            }
        }



    }
}
