using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TB3.Database.AppDbContextModels;
using TB3.WebApi.Services;

namespace TB3.WebApi.Controllers;

// https://localhost:7258/api/Product
[Route("api/[controller]")]
[ApiController]
public class ProductAdoDotNetController : ControllerBase
{
    private readonly ProductAdoDotNetService _productAdoDotNetService;

    public ProductAdoDotNetController()
    {
        _productAdoDotNetService = new ProductAdoDotNetService();
    }

    [HttpGet("{pageNo}/{pageSize}")]
    public IActionResult GetProducts(int pageNo, int pageSize)
    {
        var result = _productAdoDotNetService.GetProducts(pageNo, pageSize);
        if (!result.IsSuccess)
            return NotFound(result);
        
        return Ok(result.Products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var result = _productAdoDotNetService.GetProduct(id);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result.Product);
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductCreateRequestDto request)
    {
        var result = _productAdoDotNetService.CreateProduct(request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, ProductUpdateRequestDto request)
    {
        var result = _productAdoDotNetService.UpdateProduct(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchProduct(int id, ProductPatchRequestDto request)
    {
        var result = _productAdoDotNetService.PatchProduct(id, request);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var result = _productAdoDotNetService.DeleteProduct(id);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}