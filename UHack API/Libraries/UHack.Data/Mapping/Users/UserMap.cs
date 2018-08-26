using UHack.Core.Domain.Users;

namespace UHack.Data.Mapping.Users
{
    public partial class UserMap : AppEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("application_users");
            this.HasKey(b => b.Id);
            this.Property(p => p.UserGuid).HasMaxLength(50);
            this.Property(p => p.Username).HasMaxLength(100);
            this.Property(p => p.Email).HasMaxLength(50);
            this.Property(p => p.Firstname).HasMaxLength(50);
            this.Property(p => p.Lastname).HasMaxLength(50);
            this.Property(p => p.Password).HasMaxLength(200);
            this.Property(p => p.PasswordSalt).HasMaxLength(200);
            this.Property(p => p.ActiveInBusiness).HasColumnType("bit");
            this.Property(p => p.Active).HasColumnType("bit");
            this.Property(p => p.Deleted).HasColumnType("bit");
            this.Property(p => p.LastIpAddress).HasMaxLength(30);

            this.Ignore(i => i.PasswordFormat);
        }

    }
}
