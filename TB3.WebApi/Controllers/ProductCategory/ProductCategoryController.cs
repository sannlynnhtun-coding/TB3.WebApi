using Microsoft.AspNetCore.Mvc;
using TB3.Database.AppDbContextModels;
using TB3.WebApi.Services;
using TB3.WebApi.Services.ProductCategory;

namespace TB3.WebApi.Controllers.ProductCategory;

// https://localhost:7258/api/ProductCategory
[Route("api/[controller]")]
[ApiController]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _productCategoryService;

    public ProductCategoryController(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;

    }

    [HttpGet("{pageNo}/{pageSize}")]
    public IActionResult GetProductCategories(int pageNo, int pageSize)
    {
        var result = _productCategoryService.GetProductCategories(pageNo, pageSize);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductCategory(int id)
    {
        var result = _productCategoryService.GetProductCategoryById(id);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        var result = _productCategoryService.CreateProductCategory(request);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        var result = _productCategoryService.UpdateProductCategory(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        var result = _productCategoryService.PatchProductCategory(id, request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {

        var result = _productCategoryService.DeleteProductCategory(id);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    //[HttpGet]
    //public IActionResult GetProductCategories()
    //{
    //    var lst = _db.TblProductCategories
    //        .OrderByDescending(x => x.ProductCategoryId)
    //        .ToList();

    //    return Ok(lst);
    //}

    //[HttpGet("{id}")]
    //public IActionResult GetProductCategory(int id)
    //{
    //    var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
    //    if (item is null)
    //    {
    //        return NotFound("Product Category not found.");
    //    }

    //    var response = new ProductCategoryGetResponseDto
    //    {
    //        ProductCategoryCode = item.ProductCategoryCode,
    //        ProductCategoryName = item.ProductCategoryName
    //    };

    //    return Ok(response);
    //}

    //[HttpPost]
    //public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    //{
    //    _db.TblProductCategories.Add(new TblProductCategory
    //    {
    //        ProductCategoryCode = request.ProductCategoryCode,
    //        ProductCategoryName = request.ProductCategoryName
    //    });

    //    int result = _db.SaveChanges();
    //    string message = result > 0 ? "Saving Successful." : "Saving Failed.";

    //    return Ok(message);
    //}

    //[HttpPut("{id}")]
    //public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    //{
    //    var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
    //    if (item is null)
    //    {
    //        return NotFound("Product Category not found.");
    //    }

    //    item.ProductCategoryCode = request.ProductCategoryCode;
    //    item.ProductCategoryName = request.ProductCategoryName;

    //    int result = _db.SaveChanges();
    //    string message = result > 0 ? "Updating Successful." : "Updating Failed.";

    //    return Ok(message);
    //}

    //[HttpPatch("{id}")]
    //public IActionResult PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    //{
    //    var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
    //    if (item is null)
    //    {
    //        return NotFound("Product Category not found.");
    //    }

    //    if (!string.IsNullOrEmpty(request.ProductCategoryCode))
    //        item.ProductCategoryCode = request.ProductCategoryCode;

    //    if (!string.IsNullOrEmpty(request.ProductCategoryName))
    //        item.ProductCategoryName = request.ProductCategoryName;

    //    int result = _db.SaveChanges();
    //    string message = result > 0 ? "Patching Successful." : "Patching Failed.";

    //    return Ok(message);
    //}

    //[HttpDelete("{id}")]
    //public IActionResult DeleteProductCategory(int id)
    //{

    //    var item = _db.TblProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
    //    if (item is null)
    //    {
    //        return NotFound("Product Category not found.");
    //    }
    //    _db.TblProductCategories.Remove(item);

    //    int result = _db.SaveChanges();
    //    string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

    //    return Ok(message);
    //}
}

public class ProductCategoryCreateRequestDto
{
    public string? ProductCategoryCode { get; set; }
    public string? ProductCategoryName { get; set; }
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