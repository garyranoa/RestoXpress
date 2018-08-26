 using UHack.Core.Domain.Logging;

namespace UHack.Data.Mapping.Logging
{
    public partial class LogMap : AppEntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.ToTable("logs");
            this.HasKey(l => l.Id);
            this.Property(l => l.ShortMessage).IsRequired();
            this.Property(p => p.IpAddress).HasMaxLength(30);
            this.Property(p => p.ShortMessage).HasColumnType("text");
            this.Property(p => p.FullMessage).HasColumnType("text");
            this.Property(p => p.PageUrl).HasMaxLength(200);
            this.Property(p => p.ReferrerUrl).HasColumnType("text");

            this.Ignore(l => l.LogLevel);
            this.Ignore(l => l.User);

        }
    }
}
