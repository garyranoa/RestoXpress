using UHack.Core.Domain.Applications;

namespace UHack.Data.Mapping.Applications
{
    public partial class ApplicationMap : AppEntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            this.ToTable("applications");
            this.HasKey(a => a.Id);

            this.Property(p => p.Name).HasMaxLength(50);
            this.Property(p => p.Url).HasMaxLength(100);
            this.Property(p => p.SslEnabled).HasColumnType("bit");
            this.Property(p => p.SecureUrl).HasMaxLength(100);
            this.Property(p => p.Hosts).HasMaxLength(100);
        }
    }
}
