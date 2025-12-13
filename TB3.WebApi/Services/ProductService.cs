using Azure;
using Azure.Core;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using TB3.Database.AppDbContextModels;
using TB3.WebApi.Controllers;

namespace TB3.WebApi.Services;

// Business Logic + Data Access
public class ProductService
{
    private readonly AppDbContext _db;

    public ProductService()
    {
        _db = new AppDbContext();
    }

    public ProductGetResponseDto GetProducts(int pageNo, int pageSize)
    {
        ProductGetResponseDto dto = new ProductGetResponseDto();
        if (pageNo == 0)
        {
            dto = new ProductGetResponseDto()
            {
                IsSuccess = false,
                Message = "Page number must be greater than zero."
            };
            return dto;
        }
        if (pageSize == 0)
        {
            dto = new ProductGetResponseDto()
            {
                IsSuccess = false,
                Message = "Page size must be greater than zero."
            };
            return dto;
        }

        var lst = _db.TblProducts
            .OrderByDescending(x => x.ProductId)
            .Where(x => x.DeleteFlag == false)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        //List<ProductDto> products = new List<ProductDto>();
        //foreach (var item in lst)
        //{
        //    ProductDto product = new ProductDto
        //    {
        //        Price = item.Price,
        //        ProductId = item.ProductId, 
        //        ProductName = item.ProductName,
        //        Quantity = item.Quantity
        //    };
        //    products.Add(product);
        //}

        var products = lst.Select(item => new ProductDto
        {
            Price = item.Price,
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity
        }).ToList();

        dto = new ProductGetResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            Products = products
        };
        return dto;
    }

    public ProductGetByIdResponseDto GetProductById(int id)
    {
        ProductGetByIdResponseDto dto = new ProductGetByIdResponseDto();
        var item = _db.TblProducts
            .Where(x => x.DeleteFlag == false)
            .FirstOrDefault(x => x.ProductId == id);
        if (item is null)
        {
            dto = new ProductGetByIdResponseDto()
            {
                IsSuccess = false,
                Message = "Product Not found"
            };

            return dto;
        }

        var product = new ProductDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Price = item.Price,
            Quantity = item.Quantity,
        };

        dto = new ProductGetByIdResponseDto()
        {
            IsSuccess = true,
            Message = "Product is successfully retrieved.",
            Product = product,
        };

        return dto;
    }

    public ProductResponseDto CreateProduct(ProductCreateRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;

        ProductResponseDto dto = new ProductResponseDto();

        if (String.IsNullOrEmpty(requestDto.ProductName))
        {
            message = "Product name cannot be empty.";
            goto Response;

            //dto = new ProductResponseDto
            //{
            //    IsSuccess = false,
            //    Message = "Product name cannot be empty."
            //};
            //return dto;
        }
        if (requestDto.Price <= 0)
        {
            message = "Price cannot be empty.";
            goto Response;

            //dto = new ProductResponseDto
            //{
            //    IsSuccess = false,
            //    Message = "Price cannot be empty."
            //};
            //return dto;
        }
        if (requestDto.Quantity <= 0)
        {
            message = "Quatity cannot be empty.";
            goto Response;

            //dto = new ProductResponseDto
            //{
            //    IsSuccess = false,
            //    Message = "Quatity cannot be empty."
            //};
            //return dto;
        }

        _db.TblProducts.Add(new TblProduct
        {
            CreatedDateTime = DateTime.Now,
            Price = requestDto.Price,
            DeleteFlag = false,
            ProductName = requestDto.ProductName,
            Quantity = requestDto.Quantity,
        });

        int result = _db.SaveChanges();

        message = "Saving Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Saving Successful.";
            goto Response;

            //dto = new ProductResponseDto
            //{
            //    IsSuccess = false,
            //    Message = "Saving Failed"
            //};
            //return dto;
        }
        
        Response:
        dto = new ProductResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductResponseDto UpdateProduct(int id, ProductUpdateRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;
        ProductResponseDto dto = new ProductResponseDto();

        if (String.IsNullOrEmpty(requestDto.ProductName))
        {
            message = "Product name cannot be empty.";
            goto Response;
        }
        if (requestDto.Price <= 0)
        {
            message = "Price cannot be empty.";
            goto Response;
        }
        if (requestDto.Quantity <= 0)
        {
            message = "Quantity cannot be empty.";
            goto Response;
        }

        var item = _db.TblProducts
            .Where(x => x.DeleteFlag == false)
            .FirstOrDefault(x => x.ProductId == id);

        if (item is null)
        {
            message = "Product Not Found";
            goto Response;
        }

        item.ProductName = requestDto.ProductName;
        item.Price = requestDto.Price;
        item.Quantity = requestDto.Quantity;
        item.ModifiedDateTime = DateTime.Now;
        int result = _db.SaveChanges();

        message = "Updating Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Updating Successful.";
            goto Response;
        }

    Response:
        dto = new ProductResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductResponseDto PatchProduct(int id, ProductPatchRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;
        ProductResponseDto dto = new ProductResponseDto();

        if(string.IsNullOrEmpty(requestDto.ProductName) && (requestDto.Price is null || requestDto.Price <= 0) && (requestDto.Quantity is null || requestDto.Quantity <= 0))
        {
            message = "No data to update.";
            goto Response;
        }

        var item = _db.TblProducts
            .Where(x => x.DeleteFlag == false)
            .FirstOrDefault(x => x.ProductId == id);
        if (item is null)
        {
            message = "Product Not Found";
            goto Response;
        }

        if (!string.IsNullOrEmpty(requestDto.ProductName))
            item.ProductName = requestDto.ProductName;

        if (requestDto.Price is not null && requestDto.Price > 0)
            item.Price = requestDto.Price ?? 0;

        if (requestDto.Quantity is not null && requestDto.Quantity > 0)
            item.Quantity = requestDto.Quantity ?? 0;

        item.ModifiedDateTime = DateTime.Now;
        int result = _db.SaveChanges();

        message = "Patching Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Patching Successful.";
            goto Response;
        }

    Response:
        dto = new ProductResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductResponseDto DeleteProduct(int id)
    {
        ProductResponseDto dto = new ProductResponseDto();

        if (id <= 0)
        {
            dto.Message = "Invalid Product Id.";
            goto Response;
        }

        var item = _db.TblProducts
            .Where(x => x.DeleteFlag == false)
            .FirstOrDefault(x => x.ProductId == id);

        if (item is null)
        {
            dto.Message = "Product Not Found.";
            goto Response;
        }

        // Proceed Soft Delete
        item.DeleteFlag = true;
        item.ModifiedDateTime = DateTime.Now;
        int result = _db.SaveChanges();

        if (result < 1)
        {
            dto.Message = "Deleting Failed.";
            goto Response;
        }

        dto.IsSuccess = true;
        dto.Message = "Deleting Successful.";

    Response:
        return dto;
    }
}

public class ProductGetResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<ProductDto> Products { get; set; }
}

public class ProductResponseDto
{
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}

public class ProductGetByIdResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public ProductDto Product { get; set; }
}

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
