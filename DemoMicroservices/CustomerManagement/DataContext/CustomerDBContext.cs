using System;
using CustomerManagement.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.DataContext
{
	public class CustomerDBContext : DbContext
	{
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options)
        {
        }

        public DbSet<CustomerModel> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ModelBuilders.CustomerModelBuilder(modelBuilder).BuildCustomerModel();
        }
    }
}

