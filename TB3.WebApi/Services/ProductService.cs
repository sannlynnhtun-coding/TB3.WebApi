using TB3.Database.AppDbContextModels;

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
}

public class ProductGetResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } 
    public List<ProductDto> Products { get; set; }
}

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
