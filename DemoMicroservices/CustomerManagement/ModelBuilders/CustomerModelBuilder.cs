using System;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.ModelBuilders
{
	public class CustomerModelBuilder
	{
        private readonly ModelBuilder _modelBuilder;

        public CustomerModelBuilder(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void BuildCustomerModel()
        {
            _modelBuilder.Entity<Models.CustomerModel>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("Name").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Address).HasColumnName("Address").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
            });
        }
    }
}