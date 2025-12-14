using Microsoft.Data.SqlClient;
using TB3.WebApi.Controllers.ProductCategory;

namespace TB3.WebApi.Services;

public class ProductCategoryADODotService : IProductCategoryADODotNetService
{
    private readonly string _connectionString;

    public ProductCategoryADODotService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DbConnection")!;
    }

    public ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize)
    {
        var lts = new List<ProductCategoryDto>();
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        int skip = (pageNo - 1) * pageSize;
        string query = @"
            SELECT ProductCategoryCode, ProductCategoryName FROM Tbl_ProductCategory
        ORDER BY ProductCategoryId DESC
        OFFSET @Skip ROWS
        FETCH NEXT @Take ROWS ONLY";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Skip", skip);
        cmd.Parameters.AddWithValue("@Take", pageSize);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var item = new ProductCategoryDto()
            {
                ProductCategoryCode = Convert.ToString(reader["ProductCategoryCode"])!,
                ProductCategoryName = Convert.ToString(reader["ProductCategoryName"])!
            };

            lts.Add(item);
        }

        connection.Close();

        return new ProductCategoryGetResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            ProductCategories = lts
        };
    }
    
    public ProductCategoryGetByIdResponseDto GetProductCategory(int id)
    {
        ProductCategoryDto item = null;
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            SELECT ProductCategoryId, ProductCategoryCode, ProductCategoryName FROM Tbl_ProductCategory
        WHERE ProductCategoryId = @ProductCategoryId";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductCategoryId", id);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            item = new ProductCategoryDto()
            {
                ProductCategoryCode = Convert.ToString(reader["ProductCategoryCode"])!,
                ProductCategoryName = Convert.ToString(reader["ProductCategoryName"])!
            };
        }

        if (item is null)
        {
            return new ProductCategoryGetByIdResponseDto()
            {
                IsSuccess = false,
                Message = "Product category not found.",
            };
        }

        connection.Close();

        return new ProductCategoryGetByIdResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            ProductCategory = item
        };
    }
    
    public ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductCategoryCode))
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = false,
                Message = "Product category code is required.",
            };
        }
        
        if (string.IsNullOrWhiteSpace(request.ProductCategoryName))
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = false,
                Message = "Product category name is required.",
            };
        }

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            INSERT INTO Tbl_ProductCategory (ProductCategoryCode, ProductCategoryName)
        VALUES (@ProductCategoryCode, @ProductCategoryName)";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductCategoryCode", request.ProductCategoryCode);
        cmd.Parameters.AddWithValue("@ProductCategoryName", request.ProductCategoryName);

        int result = cmd.ExecuteNonQuery();

        connection.Close();

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product category created successfully.",
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to create product category.",
        };
    }
    
    public ProductCategoryResponseDto UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductCategoryCode))
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = false,
                Message = "Product category code is required.",
            };
        }
        
        if (string.IsNullOrWhiteSpace(request.ProductCategoryName))
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = false,
                Message = "Product category name is required.",
            };
        }

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            UPDATE Tbl_ProductCategory SET ProductCategoryCode = @ProductCategoryCode, ProductCategoryName = @ProductCategoryName
        WHERE ProductCategoryId = @ProductCategoryId";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductCategoryId", id);
        cmd.Parameters.AddWithValue("@ProductCategoryCode", request.ProductCategoryCode);
        cmd.Parameters.AddWithValue("@ProductCategoryName", request.ProductCategoryName);

        int result = cmd.ExecuteNonQuery();

        connection.Close();

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product category updated successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to update product category.",
        };
    }
    
    public ProductCategoryResponseDto PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        string conditions = "";
        if (!string.IsNullOrWhiteSpace(request.ProductCategoryCode))
            conditions += "ProductCategoryCode = @ProductCategoryCode,";
        if (!string.IsNullOrWhiteSpace(request.ProductCategoryName))
            conditions += "ProductCategoryName = @ProductCategoryName,";

        if (conditions.Length == 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid request.",
            };
        }
        
        if (!string.IsNullOrWhiteSpace(conditions))
        {
            conditions = conditions.Substring(0, conditions.Length - 1);
        }

        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = $@"
            UPDATE Tbl_ProductCategory SET {conditions}
        WHERE ProductCategoryId = @ProductCategoryId";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductCategoryId", id);

        if (!string.IsNullOrWhiteSpace(request.ProductCategoryCode))
            cmd.Parameters.AddWithValue("@ProductCategoryCode", request.ProductCategoryCode);
        if (!string.IsNullOrWhiteSpace(request.ProductCategoryName))
            cmd.Parameters.AddWithValue("@ProductCategoryName", request.ProductCategoryName);

        int result = cmd.ExecuteNonQuery();

        connection.Close();

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product category updated successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to update product category.",
        };
    }
    
    public ProductCategoryResponseDto DeleteProductCategory(int id)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = "DELETE FROM Tbl_ProductCategory WHERE ProductCategoryId = @ProductCategoryId";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductCategoryId", id);

        int result = cmd.ExecuteNonQuery();

        connection.Close();

        if (result > 0)
        {
            return new ProductCategoryResponseDto()
            {
                IsSuccess = true,
                Message = "Product category deleted successfully."
            };
        }

        return new ProductCategoryResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to delete product category.",
        };
    }
}

public class ProductCategoryGetResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<ProductCategoryDto> ProductCategories { get; set; }
}

public class ProductCategoryGetByIdResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public ProductCategoryDto ProductCategory { get; set; }
}

public class ProductCategoryResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}

public class ProductCategoryDto
{
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
}