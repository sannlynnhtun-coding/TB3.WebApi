using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using TB3.Database.AppDbContextModels;
using TB3.WebApi.Services;

namespace TB3.WebApi.Controllers.ProductCategory;

[Route("api/[controller]")]
[ApiController]
public class ProductCategoryAdoDotNetController : ControllerBase
{
    private readonly IProductCategoryADODotNetService _productCategoryAdoDotNetService;

    public ProductCategoryAdoDotNetController(IProductCategoryADODotNetService productCategoryAdoDotNetService)
    {
        _productCategoryAdoDotNetService = productCategoryAdoDotNetService;
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public IActionResult GetProductCategories(int pageNo = 1, int pageSize = 10)
    {
        var result = _productCategoryAdoDotNetService.GetProductCategories(pageNo, pageSize);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result.ProductCategories);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductCategory(int id)
    {
        var result = _productCategoryAdoDotNetService.GetProductCategory(id);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result.ProductCategory);
    }

    [HttpPost]
    public IActionResult CreateProductCategory(ProductCategoryCreateRequestDto request)
    {
        var result = _productCategoryAdoDotNetService.CreateProductCategory(request);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request)
    {
        var result = _productCategoryAdoDotNetService.UpdateProductCategory(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProductCategory(int id, ProductCategoryPatchRequestDto request)
    {
        var result = _productCategoryAdoDotNetService.PatchProductCategory(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProductCategory(int id)
    {
        var result = _productCategoryAdoDotNetService.DeleteProductCategory(id);
        
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

}