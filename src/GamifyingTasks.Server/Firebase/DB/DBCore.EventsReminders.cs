using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.Authentication;
using GamifyingTasks.Firebase.DB.Interfaces;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreEventsReminders : IEvents, IReminders
    {
        private List<Events> m_TodaysEvents = new List<Events>();
        private List<Events> m_UpcomingEvents = new List<Events>();
        readonly IDBCore? dBCore;
           readonly IUsers _users;

        public DBCoreEventsReminders(IDBCore? dBCore, IUsers users)
        {
            this.dBCore = dBCore;
            _users = users;
        }

        #region  Events
        public async Task InitEvents()
        {
            // REset the lists
            m_TodaysEvents = new List<Events>();
            m_UpcomingEvents = new List<Events>();

            if (dBCore != null)
            {
                var coll = dBCore.GetDB().Collection("Events");
                QuerySnapshot docs = await coll.GetSnapshotAsync();


                foreach (DocumentSnapshot doc in docs.Documents)
                {
                    Events userEvent = doc.ConvertTo<Events>();

                    if (userEvent.UserId == _users.GetUser().Uid)
                    {
                        Console.WriteLine($"Event Date: {userEvent.EventDate}, Today Date: {DateTime.Today.Date}");
                        if (userEvent.EventDate.ToDateTime().Date == DateTime.Today.Date)
                        {
                            m_TodaysEvents.Add(userEvent);
                        }
                        else if (userEvent.EventDate.ToDateTime().Date > DateTime.Today.Date)
                        {
                            m_UpcomingEvents.Add(userEvent);
                        }
                    }
                }
            }
        }
        public async Task CreateEvent(Events userEvents)
        {
            var docRef = await dBCore.GetDB().Collection("Events").AddAsync(userEvents);
            await docRef.UpdateAsync("UID", docRef.Id);

            if(userEvents.EventDate.ToDateTime().Date == DateTime.Today.Date)
            {
                m_TodaysEvents.Add(userEvents);
            }
            else if(userEvents.EventDate.ToDateTime().Date > DateTime.Today.Date)
            {
                m_UpcomingEvents.Add(userEvents);
            }
        }

        public async Task UpdateEvent(Events userEvents)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteEvent(string uid)
        {
            throw new NotImplementedException();
        }


        public List<Events> GetTodaysEvents()
        {
            return m_TodaysEvents;
        }

        public List<Events> GetUpcomingEvents()
        {
            return m_UpcomingEvents;
        }


        #endregion

        #region Reminders
        public Reminders InitReminders()
        {
            throw new NotImplementedException();
        }

        public Reminders CreateReminder(Tasks task)
        {
            throw new NotImplementedException();
        }
        public Reminders UpdateReminder(Tasks task)
        {
            throw new NotImplementedException();
        }
        public Reminders DeleteReminder(string uid)
        {
            throw new NotImplementedException();
        }

        public List<Reminders> GetReminders()
        {
            throw new NotImplementedException();
        }
        #endregion







    }
}