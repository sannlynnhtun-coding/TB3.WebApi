using Microsoft.Data.SqlClient;
using TB3.WebApi.Controllers;

namespace TB3.WebApi.Services;

public class ProductAdoDotNetService
{
    private readonly string _connectionString = "Server=.;Database=Batch3MiniPOS;User ID=sa;Password=sasa@123;TrustServerCertificate=True;";

    public ProductGetResponseDto GetProducts(int pageNo, int pageSize)
    {
        var lts = new List<ProductDto>();
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        int skip = (pageNo - 1) * pageSize;
        string query = @"
            SELECT ProductId, ProductName, Quantity, Price FROM Tbl_Product
        WHERE DeleteFlag = 0
        ORDER BY ProductId DESC
        OFFSET @Skip ROWS
        FETCH NEXT @Take ROWS ONLY";
        
        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Skip", skip);
        cmd.Parameters.AddWithValue("@Take", pageSize);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var item = new ProductDto()
            {
                ProductId = Convert.ToInt32(reader["ProductId"]),
                ProductName = Convert.ToString(reader["ProductName"])!,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"])
            };
            
            lts.Add(item);
        }
        
        connection.Close();

        return new ProductGetResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            Products = lts
        };
    }

    public ProductGetByIdResponseDto GetProduct(int id)
    {
        ProductDto item = null;
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            SELECT ProductId, ProductName, Quantity, Price FROM Tbl_Product
        WHERE ProductId = @ProductId AND DeleteFlag = 0";
        
        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductId", id);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            item = new ProductDto()
            {
                ProductId = Convert.ToInt32(reader["ProductId"]),
                ProductName = Convert.ToString(reader["ProductName"])!,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"])
            };
        }

        if (item is null)
        {
            return new ProductGetByIdResponseDto()
            {
                IsSuccess = false,
                Message = "Product not found.",
            };
        }
        
        connection.Close();

        return new ProductGetByIdResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            Product = item
        };
    }

    public ProductResponseDto CreateProduct(ProductCreateRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductName))
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Product name is required.",
            };
        }
        
        if (request.Quantity <= 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Quantity must be greater than 0.",
            };
        }

        if (request.Price <= 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Price must be greater than 0.",
            };
        }
        
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            INSERT INTO Tbl_Product (ProductName, Quantity, Price, DeleteFlag)
        VALUES (@ProductName, @Quantity, @Price, 0)";
        
        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductName", request.ProductName);
        cmd.Parameters.AddWithValue("@Quantity", request.Quantity);
        cmd.Parameters.AddWithValue("@Price", request.Price);
        
        int result = cmd.ExecuteNonQuery();
        
        connection.Close();

        if (result > 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = true,
                Message = "Product created successfully.",
            };
        }

        return new ProductResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to create product.",
        };
    }

    public ProductResponseDto UpdateProduct(int id, ProductUpdateRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductName))
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Product name is required.",
            };
        }
        
        if (request.Quantity <= 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Quantity must be greater than 0.",
            };
        }

        if (request.Price <= 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Price must be greater than 0.",
            };
        }
        
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            UPDATE Tbl_Product SET ProductName = @ProductName, Quantity = @Quantity, Price = @Price, ModifiedDateTime = @ModifiedDateTime
        WHERE ProductId = @ProductId";
        
        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductId", id);
        cmd.Parameters.AddWithValue("@ProductName", request.ProductName);
        cmd.Parameters.AddWithValue("@Quantity", request.Quantity);
        cmd.Parameters.AddWithValue("@Price", request.Price);
        cmd.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
        
        int result = cmd.ExecuteNonQuery();
        
        connection.Close();

        if (result > 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = true,
                Message = "Product updated successfully."
            };
        }

        return new ProductResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to update product.",
        };
    }

    public ProductResponseDto PatchProduct(int id, ProductPatchRequestDto request)
    {
        string conditions = "";
        if (!string.IsNullOrWhiteSpace(request.ProductName))
            conditions += "ProductName = @ProductName,";
        if (request.Quantity is not null && request.Quantity > 0)
            conditions += "Quantity = @Quantity,";
        if (request.Price is not null && request.Price > 0)
            conditions += "Price = @Price,";
        conditions += "ModifiedDateTime = @ModifiedDateTime";

        if (conditions.Length == 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = false,
                Message = "Invalid request.",
            };
        }
        
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = $@"
            UPDATE Tbl_Product SET {conditions}
        WHERE ProductId = @ProductId";

        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductId", id);
        
        if (!string.IsNullOrWhiteSpace(request.ProductName))
            cmd.Parameters.AddWithValue("@ProductName", request.ProductName);
        if (request.Quantity is not null && request.Quantity > 0)
            cmd.Parameters.AddWithValue("@Quantity", request.Quantity);
        if (request.Price is not null && request.Price > 0)
            cmd.Parameters.AddWithValue("@Price", request.Price);
        cmd.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
        
        int result = cmd.ExecuteNonQuery();
        
        connection.Close();

        if (result > 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = true,
                Message = "Product updated successfully."
            };
        }

        return new ProductResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to update product.",
        };
    }

    public ProductResponseDto DeleteProduct(int id)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = @"
            UPDATE Tbl_Product SET DeleteFlag = 1
        WHERE ProductId = @ProductId";
        
        SqlCommand cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@ProductId", id);
        
        int result = cmd.ExecuteNonQuery();
        
        connection.Close();

        if (result > 0)
        {
            return new ProductResponseDto()
            {
                IsSuccess = true,
                Message = "Product deleted successfully."
            };
        }

        return new ProductResponseDto()
        {
            IsSuccess = false,
            Message = "Failed to delete product.",
        };
    }
}