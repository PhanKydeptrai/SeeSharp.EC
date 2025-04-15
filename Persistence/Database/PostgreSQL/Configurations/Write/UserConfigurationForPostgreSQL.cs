using Domain.Entities.Customers;
using Domain.Entities.Employees;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.PostgreSQL.Configurations.Write;

internal sealed class UserConfigurationForPostgreSQL : IEntityTypeConfiguration<User>
{
    private readonly UserName _rootUserName;
    private readonly Email _rootUserEmail;
    private readonly PhoneNumber _rootUserPhoneNumber;
    private readonly PasswordHash _rootUserPassword;
    public UserConfigurationForPostgreSQL(
        UserName rootUserName,
        Email rootUserEmail,
        PhoneNumber rootUserPhoneNumber,
        PasswordHash rootUserPassword)
    {
        _rootUserName = rootUserName;
        _rootUserEmail = rootUserEmail;
        _rootUserPhoneNumber = rootUserPhoneNumber;
        _rootUserPassword = rootUserPassword;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasConversion(
                value => value.Value,
                value => UserId.FromGuid(value))
            .HasColumnType("uuid");

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
            .HasColumnType("integer");

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
            .HasColumnType("date");

        builder.Property(a => a.ImageUrl)
            .IsRequired(false)
            .HasColumnType("varchar(256)");
        
        // Seed Data
        builder.HasData(
            User.FromExisting(
                UserId.RootUserId,
                _rootUserName,
                _rootUserEmail,
                _rootUserPhoneNumber,
                _rootUserPassword,
                string.Empty));

        //* Forign Key
        builder.HasOne(a => a.Customer)
            .WithOne(a => a.User)
            .HasForeignKey<Customer>(a => a.UserId);

        builder.HasOne(a => a.Employee)
            .WithOne(a => a.User)
            .HasForeignKey<Employee>(a => a.UserId);
    }
}