using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Database.PostgreSQL.ReadModels;

namespace Domain.Database.PostgreSQL.Configurations.Read;

internal sealed class UserReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<UserReadModel>
{
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                value => value.ToGuid(),
                value => new Ulid(value))
            .HasColumnType("uuid");

        builder.Property(a => a.UserName)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(a => a.Email)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(a => a.PhoneNumber)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasColumnType("varchar(64)");

        builder.Property(a => a.UserStatus)
            .IsRequired()
            .HasColumnType("integer");

        builder.Property(a => a.IsVerify)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.Gender)
            .IsRequired()
            .HasColumnType("varchar(10)");

        builder.Property(a => a.DateOfBirth)
            .IsRequired(false)
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(256)");

        //* Forign Key
        builder.HasOne(a => a.CustomerReadModel)
            .WithOne(a => a.UserReadModel)
            .HasForeignKey<CustomerReadModel>(a => a.UserId);

        builder.HasOne(a => a.EmployeeReadModel)
            .WithOne(a => a.UserReadModel)
            .HasForeignKey<EmployeeReadModel>(a => a.UserId);

        builder.HasMany(a => a.UserAuthenticationTokenReadModels)
            .WithOne(a => a.UserReadModel)
            .HasForeignKey(a => a.UserId);

        builder.HasMany(a => a.VerificationTokenReadModels)
            .WithOne(a => a.UserReadModel)
            .HasForeignKey(a => a.UserId);

    }
}