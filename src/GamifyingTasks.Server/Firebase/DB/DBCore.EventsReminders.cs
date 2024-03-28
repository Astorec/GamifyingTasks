using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.Authentication;
using GamifyingTasks.Firebase.DB.Interfaces;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreEventsReminders : IEvents, IReminders
    {
        // Lists to store the events
        private List<Events> m_TodaysEvents = new List<Events>();
        private List<Events> m_UpcomingEvents = new List<Events>();
        private List<Events> m_AllEvents = new List<Events>();

        // DBCore instance
        readonly IDBCore? dBCore;
        // Users instance
        readonly IUsers _users;

        /// <summary>
        /// Constructor for DBCoreEventsReminders
        /// </summary>
        public DBCoreEventsReminders(IDBCore? dBCore, IUsers users)
        {
            this.dBCore = dBCore;
            _users = users;
        }

        #region  Events

        /// <summary>
        /// InitEvents gets the events from the database and stores them in the lists
        /// </summary>
        /// <returns></returns>
        public async Task InitEvents()
        {
            // REset the lists
            m_TodaysEvents = new List<Events>();
            m_UpcomingEvents = new List<Events>();
            m_AllEvents = new List<Events>();

            Console.WriteLine("InitEvents");
            // Get the events from the database
            if (dBCore != null)
            {   

                    Console.WriteLine("InitEvents 2");
                // Get a copy of the Events collection
                var coll = dBCore.GetDB().Collection("Events");
                QuerySnapshot docs = await coll.GetSnapshotAsync();
                // Loop through the documents
                foreach (DocumentSnapshot doc in docs.Documents)
                {
                    Console.WriteLine("InitEvents 3");
                    Events userEvent = doc.ConvertTo<Events>();

                    if (userEvent.UserId == _users.GetUser().Uid)
                    {

                        Console.WriteLine("InitEvents 4");
                        m_AllEvents.Add(userEvent);

                        Console.WriteLine($"Event Count: {m_AllEvents.Count}"); // Debugging output
                        // Add the event to the correct list
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

        /// <summary>
        /// CreateEvent creates a new event and adds it to the database
        /// </summary>
        /// <param name="userEvents"></param>
        /// <returns></returns>
        public async Task CreateEvent(Events userEvents)
        {
            // Add the event to the database
            var docRef = await dBCore.GetDB().Collection("Events").AddAsync(userEvents);
            await docRef.UpdateAsync("UID", docRef.Id);


            Console.Write("Hit 01");

            Console.WriteLine($"Event Date: {userEvents.EventDate.ToDateTime().Date} Today's Date: {DateTime.Today.Date}"); // Debugging output

            Console.WriteLine(userEvents.EventDate.ToDateTime().Date == DateTime.Today.Date); // Debugging output

            // Add the event to the correct list
            if (userEvents.EventDate.ToDateTime().Date == DateTime.Today.Date)
            {
                Console.WriteLine("Hit 01.5");
                m_TodaysEvents.Add(userEvents);
            }
            else if (userEvents.EventDate.ToDateTime().Date != DateTime.Today.Date)
            {

                Console.Write("Hit 02");
                m_UpcomingEvents.Add(userEvents);
            }
        }

        //TODO: Implement UpdateEvent
        public async Task UpdateEvent(Events userEvents)
        {
            throw new NotImplementedException();
        }

        //TODO: Implement DeleteEvent
        public async Task DeleteEvent(string uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetTodaysEvents returns the events that are happening today
        /// </summary>
        /// <returns>
        /// Today's events
        /// </returns>
        public List<Events> GetTodaysEvents()
        {
            return m_TodaysEvents;
        }

        /// <summary>
        /// GetUpcomingEvents returns the events that are happening in the future
        /// </summary>
        /// <returns>
        /// Upcoming events
        /// </returns>
        public List<Events> GetUpcomingEvents()
        {
            return m_UpcomingEvents;
        }

        public List<Events> GetAllEvents()
        {
            return m_AllEvents;
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