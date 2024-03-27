using GamifyingTasks.Classes;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    public interface ITasks
    {
        public Task CreateTask(Tasks task);
        public Task UpdateTask(Tasks task);
        public Task DeleteTask(string uid);
        public Task InitTasks();
        public List<Tasks> GetTodaysTasks();
        public List<Tasks> GetUpcomingTasks();
        public List<Tasks> GetCompletedTasks();
    }
}