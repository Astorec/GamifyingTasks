using GamifyingTasks.Classes;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    public interface IReminders
    {
        public Reminders CreateReminder(Tasks task);
        public Reminders UpdateReminder(Tasks task);
        public Reminders DeleteReminder(string uid);
        public Reminders InitReminders();
        public List<Reminders> GetReminders();
    }
}