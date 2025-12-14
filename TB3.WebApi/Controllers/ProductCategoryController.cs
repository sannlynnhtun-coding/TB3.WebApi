using Microsoft.AspNetCore.Mvc;
using TB3.Database.AppDbContextModels;

namespace TB3.WebApi.Controllers;

// https://localhost:7258/api/ProductCategory
[Route("api/[controller]")]
[ApiController]
public class ProductCategoryController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductCategoryController()
    {
        _db = new AppDbContext();

    }

    [HttpGet]
    public IActionResult GetProductCategories()
    {
        var lst = _db.TblProductCategories
            .OrderByDescending(x => x.ProductCategoryId)
            .ToList();

        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductCategory(int id)
    {
        var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
        if (item is null)
        {
            return NotFound("Product Category not found.");
        }

        var response = new ProductCategoryGetResponseDto
        {
            ProductCategoryCode = item.ProductCategoryCode,
            ProductCategoryName = item.ProductCategoryName
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        _db.TblProductCategories.Add(new TblProductCategory
        {
            ProductCategoryCode = request.ProductCategoryCode,
            ProductCategoryName = request.ProductCategoryName
        });

        int result = _db.SaveChanges();
        string message = result > 0 ? "Saving Successful." : "Saving Failed.";

        return Ok(message);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
        if (item is null)
        {
            return NotFound("Product Category not found.");
        }

        item.ProductCategoryCode = request.ProductCategoryCode;
        item.ProductCategoryName = request.ProductCategoryName;

        int result = _db.SaveChanges();
        string message = result > 0 ? "Updating Successful." : "Updating Failed.";

        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
        if (item is null)
        {
            return NotFound("Product Category not found.");
        }

        if (!string.IsNullOrEmpty(request.ProductCategoryCode))
            item.ProductCategoryCode = request.ProductCategoryCode;

        if (!string.IsNullOrEmpty(request.ProductCategoryName))
            item.ProductCategoryName = request.ProductCategoryName;

        int result = _db.SaveChanges();
        string message = result > 0 ? "Patching Successful." : "Patching Failed.";

        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {

        var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
        if (item is null)
        {
            return NotFound("Product Category not found.");
        }
        _db.TblProductCategories.Remove(item);

        int result = _db.SaveChanges();
        string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

        return Ok(message);
    }
}

public class ProductCategoryCreateRequestDto
{
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
}

public class ProductCategoryUpdateRequestDto
{
    public string ProductCategoryCode { get; set; }
    public string ProductCategoryName { get; set; }
}

public class ProductCategoryPatchRequestDto
{
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
}

public class ProductCategoryGetResponseDto
{
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
}