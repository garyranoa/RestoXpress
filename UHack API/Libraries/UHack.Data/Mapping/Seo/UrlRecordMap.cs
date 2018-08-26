using UHack.Core.Domain.Seo;

namespace UHack.Data.Mapping.Seo
{
    public partial class UrlRecordMap : AppEntityTypeConfiguration<UrlRecord>
    {
        public UrlRecordMap()
        {
            this.ToTable("url_records");
            this.HasKey(lp => lp.Id);

            this.Property(lp => lp.EntityName).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.Slug).IsRequired().HasMaxLength(400);
            this.Property(lp => lp.IsActive).HasColumnType("bit");
        }
    }
}
