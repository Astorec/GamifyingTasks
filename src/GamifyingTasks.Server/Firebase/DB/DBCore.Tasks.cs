using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;

namespace GamifyingTasks.Firebase.DB{
    public class  DBCoreTasks : ITasks
    {
        public Task CreateTask(Tasks task)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTask(string uid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetTasks()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTask(Tasks task)
        {
            throw new NotImplementedException();
        }
    }
}