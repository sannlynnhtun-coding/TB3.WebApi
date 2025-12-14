using Microsoft.AspNetCore.Mvc;
using TB3.WebApi.Controllers.ProductCategory;
using TB3.WebApi.Services;
using TB3.WebApi.Services.ProductCategory;

namespace TB3.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryDapperController : ControllerBase
{
    private readonly IProductCategoryDapperService _service;

    public ProductCategoryDapperController(IProductCategoryDapperService service)
    {
        _service = service;
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public IActionResult GetProductCategories(int pageNo = 1, int pageSize = 10)
    {
        var result = _service.GetProductCategories(pageNo, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductCategory(int id)
    {
        var result = _service.GetProductCategory(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        var result = _service.CreateProductCategory(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        var result = _service.UpdateProductCategory(id, request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        var result = _service.PatchProductCategory(id, request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {
        var result = _service.DeleteProductCategory(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
