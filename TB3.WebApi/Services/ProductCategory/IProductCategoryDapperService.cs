using TB3.WebApi.Controllers.ProductCategory;

namespace TB3.WebApi.Services.ProductCategory
{
    public interface IProductCategoryDapperService
    {
        ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto request);
        ProductCategoryResponseDto DeleteProductCategory(int id);
        ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize);
        ProductCategoryGetByIdResponseDto GetProductCategory(int id);
        ProductCategoryResponseDto PatchProductCategory(int id, ProductCategoryPatchRequestDto request);
        ProductCategoryResponseDto UpdateProductCategory(int id, ProductCategoryUpdateRequestDto request);
    }
}