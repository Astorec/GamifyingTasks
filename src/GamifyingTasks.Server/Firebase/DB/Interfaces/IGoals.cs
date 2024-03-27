using GamifyingTasks.Classes;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    public interface IGoals
    {
        public Task CreateGoal (UserGoals userGoals);
        public Task DeleteGoal(string uid);
        public Task InitGoals();
        public List<UserGoals> GetGoals();
    }
}