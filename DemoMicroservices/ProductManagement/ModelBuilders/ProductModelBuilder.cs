using System;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.ModelBuilders
{
	public class ProductModelBuilder
	{
        private readonly ModelBuilder _modelBuilder;

        public ProductModelBuilder(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void BuildProductModel()
        {
            _modelBuilder.Entity<Models.ProductModel>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("Name").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
            });
        }
    }
}

