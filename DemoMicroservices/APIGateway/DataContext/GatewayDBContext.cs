using System;
using APIGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.DataContext
{
	public class GatewayDBContext: DbContext
	{
        public GatewayDBContext(DbContextOptions<GatewayDBContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ModelBuilders.UserBuilder(modelBuilder).BuildUserModel();
        }
    }
}

