using NextSharp.Domain.Entities.CustomerEntity;
using NextSharp.Domain.Entities.EmployeeEntity;
using NextSharp.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextSharp.Persistence.Converter;

namespace NextSharp.Persistence.Database.Postgresql.Configurations;

internal sealed class UserConfigurationForPostgreSQL : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion<UlidToStringConverter>()
            .HasColumnType("varchar(26)");

        builder.Property(a => a.UserName)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(a => a.Email)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(a => a.PhoneNumber)
            .IsRequired()
            .HasColumnType("varchar(20)");

        builder.Property(a => a.PasswordHash)
            .IsRequired()
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