using TB3.WebApi.Controllers.ProductCategory;

namespace TB3.WebApi.Services.ProductCategory
{
    public interface IProductCategoryService
    {
        ProductCategoryResponseDto CreateProductCategory(ProductCategoryCreateRequestDto requestDto);
        ProductCategoryResponseDto DeleteProductCategory(int categoryID);
        ProductCategoryGetByIdResponseDto GetProductCategoryById(int categoryID);
        ProductCategoryGetResponseDto GetProductCategories(int pageNo, int pageSize);
        ProductCategoryResponseDto PatchProductCategory(int categroyID, ProductCategoryPatchRequestDto requestDto);
        ProductCategoryResponseDto UpdateProductCategory(int categoryID, ProductCategoryUpdateRequestDto requestDto);
    }
}