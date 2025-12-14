using TB3.Database.AppDbContextModels;
using TB3.WebApi.Controllers.ProductCategory;

namespace TB3.WebApi.Services.ProductCategory;

public class ProductCategoryService : IProductCategoryService
{
    private readonly AppDbContext _db;

    public ProductCategoryService(AppDbContext db)
    {
        _db = db;
    }

    public ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize)
    {
        ProductCategoryGetResponseDto dto = new ProductCategoryGetResponseDto();
        if (pageNo == 0)
        {
            dto = new ProductCategoryGetResponseDto()
            {
                IsSuccess = false,
                Message = "Page number must be greater than zero."
            };
            return dto;
        }
        if (pageSize == 0)
        {
            dto = new ProductCategoryGetResponseDto()
            {
                IsSuccess = false,
                Message = "Page size must be greater than zero."
            };
            return dto;
        }

        var lst = _db.TblProductCategories
            .OrderByDescending(x => x.ProductCategoryId)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var productCategories = lst.Select(item => new ProductCategoryDto
        {
            ProductCategoryId = item.ProductCategoryId,
            ProductCategoryCode = item.ProductCategoryCode,
            ProductCategoryName = item.ProductCategoryName
        }).ToList();

        dto = new ProductCategoryGetResponseDto()
        {
            IsSuccess = true,
            Message = "Success.",
            ProductCategories = productCategories
        };
        return dto;
    }

    public ProductCategoryGetByIdResponseDto GetProductCategoryById(int categoryID)
    {
        ProductCategoryGetByIdResponseDto dto = new ProductCategoryGetByIdResponseDto();
        var item = _db.TblProductCategories
            .FirstOrDefault(x => x.ProductCategoryId == categoryID);
        if (item is null)
        {
            dto = new ProductCategoryGetByIdResponseDto()
            {
                IsSuccess = false,
                Message = "Product Not found"
            };

            return dto;
        }

        var productCategory = new ProductCategoryDto
        {
            ProductCategoryId = item.ProductCategoryId,
            ProductCategoryCode = item.ProductCategoryCode,
            ProductCategoryName = item.ProductCategoryName
        };

        dto = new ProductCategoryGetByIdResponseDto()
        {
            IsSuccess = true,
            Message = "Product is successfully retrieved.",
            ProductCategory = productCategory,
        };

        return dto;
    }

    public ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;

        ProductCategoryResponseDto dto = new ProductCategoryResponseDto();

        if (String.IsNullOrEmpty(requestDto.ProductCategoryCode))
        {
            message = "Product category code cannot be empty.";
            goto Response;
        }
        if (String.IsNullOrEmpty(requestDto.ProductCategoryName))
        {
            message = "Product category name cannot be empty.";
            goto Response;
        }

        _db.TblProductCategories.Add(new TblProductCategory
        {
            ProductCategoryCode = requestDto.ProductCategoryCode,
            ProductCategoryName = requestDto.ProductCategoryName
        });

        int result = _db.SaveChanges();

        message = "Saving Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Saving Successful.";
            goto Response;
        }

    Response:
        dto = new ProductCategoryResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductCategoryResponseDto UpdateProductCategory(int categoryID, ProductCategoryUpdateRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;
        ProductCategoryResponseDto dto = new ProductCategoryResponseDto();

        if (String.IsNullOrEmpty(requestDto.ProductCategoryCode))
        {
            message = "Product category code cannot be empty.";
            goto Response;
        }
        if (String.IsNullOrEmpty(requestDto.ProductCategoryName))
        {
            message = "Product category name cannot be empty.";
            goto Response;
        }

        var item = _db.TblProductCategories
            .FirstOrDefault(x => x.ProductCategoryId == categoryID);

        if (item is null)
        {
            message = "Product Not Found";
            goto Response;
        }

        item.ProductCategoryCode = requestDto.ProductCategoryCode;
        item.ProductCategoryName = requestDto.ProductCategoryName;
        int result = _db.SaveChanges();

        message = "Updating Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Updating Successful.";
            goto Response;
        }

    Response:
        dto = new ProductCategoryResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductCategoryResponseDto PatchProductCategory(int categroyID, ProductCategoryPatchRequestDto requestDto)
    {
        bool isSuccess = false;
        string message = string.Empty;
        ProductCategoryResponseDto dto = new ProductCategoryResponseDto();

        if (string.IsNullOrEmpty(requestDto.ProductCategoryCode) && string.IsNullOrEmpty(requestDto.ProductCategoryCode))
        {
            message = "No data to update.";
            goto Response;
        }

        var item = _db.TblProductCategories
            .FirstOrDefault(x => x.ProductCategoryId == categroyID);
        if (item is null)
        {
            message = "Product Not Found";
            goto Response;
        }

        if (!string.IsNullOrEmpty(requestDto.ProductCategoryCode))
            item.ProductCategoryCode = requestDto.ProductCategoryCode;

        if (!string.IsNullOrEmpty(requestDto.ProductCategoryName))
            item.ProductCategoryName = requestDto.ProductCategoryName;

        int result = _db.SaveChanges();

        message = "Patching Failed.";
        if (result > 0)
        {
            isSuccess = true;
            message = "Patching Successful.";
            goto Response;
        }

    Response:
        dto = new ProductCategoryResponseDto
        {
            IsSuccess = isSuccess,
            Message = message,
        };

        return dto;
    }

    public ProductCategoryResponseDto DeleteProductCategory(int categoryID)
    {
        ProductCategoryResponseDto dto = new ProductCategoryResponseDto();

        if (categoryID <= 0)
        {
            dto.Message = "Invalid Product Id.";
            goto Response;
        }

        var item = _db.TblProductCategories
            .FirstOrDefault(x => x.ProductCategoryId == categoryID);

        if (item is null)
        {
            dto.Message = "Product Not Found.";
            goto Response;
        }

        // Real delete product category item
        _db.TblProductCategories.Remove(item);
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

public class ProductCategoryGetResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<ProductCategoryDto> ProductCategories { get; set; }
}

public class ProductCategoryResponseDto
{
    public bool IsSuccess { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}

public class ProductCategoryGetByIdResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public ProductCategoryDto ProductCategory { get; set; }
}

public class ProductCategoryDto
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryCode { get; set; } = null!;
    public string ProductCategoryName { get; set; } = null!;

}
