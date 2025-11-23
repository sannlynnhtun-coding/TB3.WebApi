using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TB3.Database.AppDbContextModels;

namespace TB3.WebApi.Controllers;

// https://localhost:7258/api/Product
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductController()
    {
        _db = new AppDbContext();
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var lst = _db.TblProducts
            .OrderByDescending(x => x.ProductId)
            .Where(x => x.DeleteFlag == false)
            .ToList();
        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var item = _db.TblProducts.FirstOrDefault(x => x.ProductId == id);
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

    [HttpPost]
    public IActionResult CreateProduct(ProductCreateRequestDto request)
    {
        _db.TblProducts.Add(new TblProduct
        {
            CreatedDateTime = DateTime.Now,
            Price = request.Price,
            DeleteFlag = false,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
        });
        int result = _db.SaveChanges();
        string message = result > 0 ? "Saving Successful." : "Saving Failed.";

        return Ok(message);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, ProductUpdateRequestDto request)
    {
        var item = _db.TblProducts.FirstOrDefault(x => x.ProductId == id);
        if (item is null)
        {
            return NotFound("Product not found.");
        }

        item.ProductName = request.ProductName;
        item.Price = request.Price;
        item.Quantity = request.Quantity;
        item.ModifiedDateTime = DateTime.Now;
        int result = _db.SaveChanges();
        string message = result > 0 ? "Updating Successful." : "Updating Failed.";

        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProduct(int id, ProductPatchRequestDto request)
    {
        var item = _db.TblProducts.FirstOrDefault(x => x.ProductId == id);
        if (item is null)
        {
            return NotFound("Product not found.");
        }

        if (!string.IsNullOrEmpty(request.ProductName))
            item.ProductName = request.ProductName;
        if (request.Price is not null && request.Price > 0)
            item.Price = request.Price ?? 0;
        //item.Price = Convert.ToDecimal(request.Price);
        if (request.Quantity is not null && request.Quantity > 0)
            item.Quantity = request.Quantity ?? 0;
        item.ModifiedDateTime = DateTime.Now;
        int result = _db.SaveChanges();
        string message = result > 0 ? "Patching Successful." : "Patching Failed.";

        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var item = _db.TblProducts.FirstOrDefault(x => x.ProductId == id);
        if (item is null)
        {
            return NotFound("Product not found.");
        }
        item.DeleteFlag = true;
        int result = _db.SaveChanges();
        string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

        return Ok(message);
    }
}

public class ProductCreateRequestDto
{
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}

public class ProductUpdateRequestDto
{
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}

public class ProductPatchRequestDto
{
    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }
}

public class ProductGetResponseDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}

public class ProductGetListResponseDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool DeleteFlag { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? ModifiedDateTime { get; set; }
}