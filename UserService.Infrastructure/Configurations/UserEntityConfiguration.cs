using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(u => u.Email)
                   .IsUnique(); 
        }
    }
}
