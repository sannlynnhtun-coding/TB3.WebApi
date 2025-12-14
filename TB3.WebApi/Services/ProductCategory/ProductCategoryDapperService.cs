using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TB3.WebApi.Controllers.ProductCategory;

namespace TB3.WebApi.Services.ProductCategory;

public class ProductCategoryDapperService : IProductCategoryDapperService
{
    private readonly string _connectionString;
    public ProductCategoryDapperService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DbConnection")!;
    }

    public ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            db.Open();

            int skip = (pageNo - 1) * pageSize;

            string query = @"
            SELECT ProductCategoryId, ProductCategoryCode, ProductCategoryName
            FROM Tbl_ProductCategory
            ORDER BY ProductCategoryId DESC
            OFFSET @Skip ROWS
            FETCH NEXT @Take ROWS ONLY";

            var lst = db.Query<ProductCategoryDto>(query, new
            {
                Skip = skip,
                Take = pageSize
            }).ToList();

            return new ProductCategoryGetResponseDto()
            {
                IsSuccess = true,
                Message = "Success.",
                ProductCategories = lst
            };
        }
    }

    public ProductCategoryGetByIdResponseDto GetProductCategory(int id)
    {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
            db.Open();

            string query = @"SELECT ProductCategoryId, ProductCategoryCode, ProductCategoryName
                         FROM Tbl_ProductCategory
                         WHERE ProductCategoryId = @id";

            var item = db.QueryFirstOrDefault<ProductCategoryDto>(query, new { id });

            if (item is null)
            {
                return new ProductCategoryGetByIdResponseDto()
                {
                    IsSuccess = false,
                    Message = "Product category not found."
                };
            }

            return new ProductCategoryGetByIdResponseDto()
            {
                IsSuccess = true,
                Message = "Success.",
                ProductCategory = item
            };
        }
    }

    public ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        db.Open();

        string query = @"
        INSERT INTO Tbl_ProductCategory
        (ProductCategoryCode, ProductCategoryName)
        VALUES (@ProductCategoryCode, @ProductCategoryName)";

        int result = db.Execute(query, request);

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product Category created successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to create product."
        };
    }

    public ProductCategoryResponseDto UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        db.Open();

        string query = @"
        UPDATE Tbl_ProductCategory
        SET ProductCategoryCode = @ProductCategoryCode,
            ProductCategoryName = @ProductCategoryName
        WHERE ProductCategoryId = @ProductCategoryId";

        int result = db.Execute(query, new
        {
            ProductCategoryId = id,
            request.ProductCategoryCode,
            request.ProductCategoryName
        });

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product Category updated successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to update product."
        };
    }

    public ProductCategoryResponseDto PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        db.Open();

        string query = "UPDATE Tbl_ProductCategory SET ";

        if (!string.IsNullOrEmpty(request.ProductCategoryCode))
            query += "ProductCategoryCode = @ProductCategoryCode, ";

        if (!string.IsNullOrEmpty(request.ProductCategoryName))
            query += "ProductCategoryName = @ProductCategoryName";

        query = query.TrimEnd(',', ' ');
        query += " WHERE ProductCategoryId = @ProductCategoryId";

        int result = db.Execute(query, new
        {
            ProductCategoryId = id,
            request.ProductCategoryCode,
            request.ProductCategoryName
        });

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product Category patched successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to patch product."
        };
    }

    public ProductCategoryResponseDto DeleteProductCategory(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        db.Open();

        string query = @"
        DELETE FROM Tbl_ProductCategory
        WHERE ProductCategoryId = @ProductCategoryId";

        int result = db.Execute(query, new { ProductCategoryId = id });

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product Category deleted successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to delete product."
        };
    }
}
