using System;
using ProductManagement.DataContext;
using ProductManagement.Helper;
using ProductManagement.Interfaces;
using ProductManagement.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ProductManagement.Repositories
{
    public class ProductRepository : IProductInterface
    {
        private readonly ProductDBContext _context;

        public ProductRepository(ProductDBContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductModel> GetProducts()
        {
            var products = _context.Products.ToList();
            return products;
        }

        public PagedList<ProductModel> GetPaginatedProducts(String? keywords, PagedListParams pagedListParams)
        {
            var filteredProducts = _context.Products.ToList();
            if (!String.IsNullOrEmpty(keywords))
            {
                filteredProducts = _context.Products.Where(d =>
                    d.Name.Contains(keywords)).ToList();
            }

            var products = PagedList<ProductModel>.ToPagedList(filteredProducts,
                pagedListParams.PageNumber,
                pagedListParams.PageSize);

            return products;
        }

        public ProductModel GetProduct(Int32 id)
        {
            var product = _context.Products.Where(d => d.Id == id).FirstOrDefault();
            return product;
        }

        public Boolean CreateProduct(ProductModel product)
        {
            _context.Add(product);
            return Save();
        }

        public Boolean UpdateProduct(ProductModel product)
        {
            _context.Update(product);
            return Save();
        }

        public Boolean DeleteProduct(ProductModel product)
        {
            _context.Remove(product);
            return Save();
        }

        public Boolean ProductExists(Int32 id)
        {
            return _context.Products.Any(o => o.Id == id);
        }

        public Boolean Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

