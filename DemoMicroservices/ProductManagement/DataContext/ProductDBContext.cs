using System;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.DataContext
{
	public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ModelBuilders.ProductModelBuilder(modelBuilder).BuildProductModel();
        }
    }
}

