using GamifyingTasks.Classes;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    public interface IEvents
    {
        public Task CreateEvent(Events userEvents);
        public Task UpdateEvent (Events userEvents);
        public Task DeleteEvent(string uid);
        public Task InitEvents();
        public List<Events> GetTodaysEvents();
        public List<Events> GetUpcomingEvents();
    }
}