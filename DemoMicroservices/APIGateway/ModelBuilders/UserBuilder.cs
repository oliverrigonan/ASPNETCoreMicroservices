using System;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.ModelBuilders
{
	public class UserBuilder
	{
        private readonly ModelBuilder _modelBuilder;

        public UserBuilder(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void BuildUserModel()
        {
            _modelBuilder.Entity<Models.UserModel>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasColumnName("Username").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Email).HasColumnName("Email").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").HasColumnType("nvarchar(255)").HasMaxLength(255).IsRequired();
            });
        }
    }
}

