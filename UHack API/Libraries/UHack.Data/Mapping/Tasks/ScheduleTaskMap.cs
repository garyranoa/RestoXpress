using UHack.Core.Domain.Tasks;

namespace UHack.Data.Mapping.Tasks
{
    public partial class ScheduleTaskMap : AppEntityTypeConfiguration<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.ToTable("schedule_tasks");
            this.HasKey(t => t.Id);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50);
            this.Property(t => t.TaskType).IsRequired().HasMaxLength(100);

            this.Property(t => t.Enabled).HasColumnType("bit");
            this.Property(t => t.StopOnError).HasColumnType("bit");
        }
    }
}
