using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Appulu.Core.Domain.Users;

namespace Appulu.Data.Mapping.Users
{
    /// <summary>
    /// Represents a user password mapping configuration
    /// </summary>
    public partial class UserPasswordMap : AppEntityTypeConfiguration<UserPassword>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.ToTable(nameof(UserPassword), Schemas.Users);
            builder.HasKey(password => password.Id);

            builder.HasOne(password => password.User)
                .WithMany()
                .HasForeignKey(password => password.UserId)
                .IsRequired();

            builder.Ignore(password => password.PasswordFormat);

            base.Configure(builder);
        }

        #endregion
    }
}