

namespace Core.ReminderServices
{
    public interface IEmailReminderService
    {
        Task SendWeeklyRemindersAsync();
    }
}
