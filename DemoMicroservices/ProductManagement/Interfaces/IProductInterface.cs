using System;
using ProductManagement.Helper;
using ProductManagement.Models;

namespace ProductManagement.Interfaces
{
    public interface IProductInterface
    {
        public IEnumerable<ProductModel> GetProducts();
        public PagedList<ProductModel> GetPaginatedProducts(String? keywords, PagedListParams pagedListParams);
        public ProductModel GetProduct(Int32 id);
        public Boolean CreateProduct(ProductModel product);
        public Boolean UpdateProduct(ProductModel product);
        public Boolean DeleteProduct(ProductModel product);
        public Boolean ProductExists(Int32 id);
    }
}