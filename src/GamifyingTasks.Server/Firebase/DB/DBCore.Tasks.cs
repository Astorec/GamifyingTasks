using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;
using GamifyingTasks.Firebase.Authentication;
using Google.Cloud.Firestore;
using Grpc.Auth;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreTasks : ITasks
    {
        // Instance of the DBCore
        readonly IDBCore? dBCore;

        // Instance of the Users
        readonly IUsers _users;

        // Lists of Tasks for Today
        private List<Tasks> m_todaysTasks = new List<Tasks>();

        // List of Tasks for the Future
        private List<Tasks> m_upcomingTasks = new List<Tasks>();

        // List of Completed Tasks
        private List<Tasks> m_completedTasks = new List<Tasks>();

        /// <summary>
        /// Constructor for DBCoreTasks
        /// </summary>
        /// <param name="dBCore"></param>
        /// <param name="users"></param>
        public DBCoreTasks(IDBCore dBCore, IUsers users)
        {
            this.dBCore = dBCore;
            _users = users;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">Pass in the newly created task by the user</param>
        /// <returns></returns>
        public async Task CreateTask(Tasks task)
        {
            // Ensure that DBCore is not null
            if (dBCore != null)
            {
                // Add the task to the DB and create a copy of the document
                var docRef = await dBCore.GetDB().Collection("tasks").AddAsync(task);


                // update the UID of the doc to match the UID of the document
                await docRef.UpdateAsync("UID", docRef.Id);


                // If the Due Date is today or has the default value add it to our list of todays tasks otherwise add it to the upcoming tasks
                if (task.DueDate.ToDateTime().Date == DateTime.Today.Date || task.DueDate.ToDateTime().Date == new DateTime(9999, 12, 31).Date)
                {
                    m_todaysTasks.Add(task);
                }
                else if (task.DueDate.ToDateTime().Date > DateTime.Today.Date)
                {
                    m_upcomingTasks.Add(task);
                }                
            }
        }

        /// <summary>
        /// Initialise the tasks for the user
        /// </summary>
        public async Task InitTasks()
        {
            // REset the lists
            m_todaysTasks = new List<Tasks>();
            m_upcomingTasks = new List<Tasks>();
            m_completedTasks = new List<Tasks>();

            // Ensure that DBCore is not null
            if (dBCore != null)
            {
                // Get the tasks from the DB
                var coll = dBCore.GetDB().Collection("tasks");

                // Take a snashot of the Collection
                QuerySnapshot docs = await coll.GetSnapshotAsync();
    
                // Loop through the documents and add them to the correct list
                foreach (DocumentSnapshot doc in docs.Documents)
                {

                    // Convert the document to a Tasks format
                    Tasks task = doc.ConvertTo<Tasks>();

                    // Ensure that the ID matches the user ID
                    if (task.UserId == _users.GetUser().Uid)
                    {
                        // Add the task to the correct list
                        if (task.Completed)
                        {
                            m_completedTasks.Add(task);
                        }
                        else if (task.DueDate.ToDateTime().Date == DateTime.Today.Date || task.DueDate.ToDateTime().Date == new DateTime(9999, 12, 31).Date)
                        {
                            m_todaysTasks.Add(task);
                        }
                        else if (task.DueDate.ToDateTime().Date > DateTime.Today.Date)
                        {
                            m_upcomingTasks.Add(task);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="uid">Pass in the UID of the task</param>
        /// <returns></returns>
        public async Task DeleteTask(string uid)
        {
            if (dBCore != null)
            {
                // Get the task that matches the UID and delete it
                DocumentReference docRef = dBCore.GetDB().Document("tasks/" + uid);

                // remove the task from the list of tasks if it exists
                m_todaysTasks.RemoveAll(x => x.UID == uid);
                m_upcomingTasks.RemoveAll(x => x.UID == uid);
                m_completedTasks.RemoveAll(x => x.UID == uid);

                // Find the task in the DB that matches the task to be deleted
                await docRef.DeleteAsync();


            }
        }

        /// <summary>
        /// Get the tasks that are due today
        /// </summary>
        /// <returns></returns>
        public List<Tasks> GetTodaysTasks()
        {
            return m_todaysTasks;
        }

        /// <summary>
        /// Get the tasks that are due in the future
        /// </summary>
        /// <returns></returns>
        public List<Tasks> GetUpcomingTasks()
        {
            return m_upcomingTasks;
        }

        /// <summary>
        /// Get the tasks that have been completed
        /// </summary>
        /// <returns></returns>
        public List<Tasks> GetCompletedTasks()
        {
            return m_completedTasks;
        }

        public async Task UpdateTask(Tasks task)
        {
            if (dBCore != null)
            {
                var coll = dBCore.GetDB().Collection("tasks");
                QuerySnapshot docs = await coll.GetSnapshotAsync();

                // Find the task in the DB that matches the task to be updated
                foreach (DocumentSnapshot doc in docs.Documents)
                {
                    if (doc.ConvertTo<Tasks>().UID == task.UID)
                    {
                        await doc.Reference.SetAsync(task);
                    }
                }


                if (task.Completed)
                {
                    // Remove the task from the Todays task or Upcoming task list if it exists
                    m_todaysTasks.RemoveAll(x => x.UID == task.UID);
                    m_upcomingTasks.RemoveAll(x => x.UID == task.UID);

                    // Add the task to the completed task list
                    m_completedTasks.Add(task);
                }


            }
        }
    }
}