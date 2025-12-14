using TB3.WebApi.Controllers;

namespace TB3.WebApi.Services;

public interface IProductCategoryADODotNetService
{
    ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize);
    ProductCategoryGetByIdResponseDto GetProductCategory(int id);
    ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto request);
    ProductCategoryResponseDto UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request);
    ProductCategoryResponseDto PatchProductCategory(int id, ProductCategoryPatchRequestDto request);
    ProductCategoryResponseDto DeleteProductCategory(int id);
}