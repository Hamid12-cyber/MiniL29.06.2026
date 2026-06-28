using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniL29._06._2026.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniL29._06._2026.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(e => e.Surname)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(e => e.Salary)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
        }
    }
}
