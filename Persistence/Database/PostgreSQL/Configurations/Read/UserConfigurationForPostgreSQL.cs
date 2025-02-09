using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class UserReadModelConfigurationForPostgreSQL : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                v => v.Value.ToString(),
                v => new UserId(Ulid.Parse(v))
            )
            .HasColumnType("varchar(26)");

        builder.Property(a => a.UserName)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => UserName.FromString(v)
            )
            .HasColumnType("varchar(50)");

        builder.Property(a => a.Email)
            .IsRequired()
            .HasConversion(
                v => v!.Value,
                v => Email.FromString(v)
            )
            .HasColumnType("varchar(200)");

        builder.Property(a => a.PhoneNumber)
            .IsRequired()
            .HasConversion(
                v => v!.Value,
                v => PhoneNumber.FromString(v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasConversion(
                v => v!.Value,
                v => PasswordHash.FromString(v)
            )
            .HasColumnType("varchar(64)");

        builder.Property(a => a.UserStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (UserStatus)Enum.Parse(typeof(UserStatus), v)
            )
            .HasColumnType("varchar(20)");

        builder.Property(a => a.IsVerify)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => IsVerify.FromBoolean(v))
            .HasColumnType("boolean");

        builder.Property(a => a.Gender)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)
            )
            .HasColumnType("varchar(10)");

        builder.Property(a => a.DateOfBirth)
            .IsRequired(false)
            .HasColumnType("TIMESTAMP");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(256)");

        //* Forign Key
        builder.HasOne(a => a.Customer)
            .WithOne(a => a.User)
            .HasForeignKey<Customer>(a => a.UserId);

        builder.HasOne(a => a.Employee)
            .WithOne(a => a.User)
            .HasForeignKey<Employee>(a => a.UserId);

        builder.HasMany(a => a.UserAuthenticationTokens)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.HasMany(a => a.VerificationTokens)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

    }
}