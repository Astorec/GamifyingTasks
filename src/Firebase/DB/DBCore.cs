using System.Data.Common;
using FirebaseAdmin;
using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using GamifyingTasks.Classes;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
namespace GamifyingTasks.Firebase.DB
{
    public static class DBCore
    {
        #region Variables
        public static event EventHandler<EventArgs> ListUpdated;
        public enum TaskType { Today, Upcoming, All }
        private static FirestoreDb m_dbInstance;
        private static FirebaseApp m_app;

        private static Dictionary<string, Tasks> m_allTasks = new Dictionary<string, Tasks>();
        private static Dictionary<string, Tasks> m_todayTasks = new Dictionary<string, Tasks>();
        private static Dictionary<string, Tasks> m_upcomingTasks = new Dictionary<string, Tasks>();
        private static Dictionary<string, Tasks> m_userTasks = new Dictionary<string, Tasks>();
        private static Dictionary<string, Users> m_allUsers = new Dictionary<string, Users>();
        private static User m_currentUser;

        // Stores the local information of the Current User that is required for Displaying. 
        public static Users CurrentLocalUser = new Users();
        public static bool IsLoggedIn = false;
        public static bool HasUpdated = false;
        private static bool IsSetup; // For the inital setup of the Application
        #endregion

        #region  DBCore Initialistaion
        public static void init()
        {
            m_app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),

                ProjectId = "hons-project-f5a1e",
            });

            IsSetup = true;
        }
        #endregion


        #region  Task Related Methods
        public static async Task<Dictionary<string, Tasks>> GetTasks()
        {
            if (IsSetup)
                IsSetup = false;

            CollectionReference coll = GetDBInstance().Collection("tasks");
            QuerySnapshot docs = await coll.GetSnapshotAsync();


            foreach (var doc in docs.Documents)
            {
                // Store the current id of the doc
                var uid = doc.Id;

                if (!m_allTasks.TryGetValue(uid, out var existingTask))
                {
                    var task = new Tasks
                    {
                        Name = doc.GetValue<string>("Name"),
                        Description = doc.GetValue<string>("Description"),
                        UserId = doc.GetValue<string>("UserId"),
                        Difficulty = doc.GetValue<string>("Difficulty"),
                        Completed = doc.GetValue<bool>("Completed"),
                        DueDate = doc.GetValue<Timestamp>("DueDate")
                    };

                    m_allTasks.Add(uid, task);

                    // If the task due date is set today's date or if it hits our default value that was created
                    // when the user didn't opt to add a date, add it to the Today's Task list.
                    if (task.DueDate.ToDateTime().Date == DateTime.UtcNow.Date ||
                    task.DueDate.ToDateTime().Date == new DateTime(9999, 12, 31).ToUniversalTime().Date)
                    {
                        if (m_todayTasks == null)
                            m_todayTasks = new Dictionary<string, Tasks>();
                        m_todayTasks.Add(uid, task);
                    }


                    // Else if the task due date is greater than or equal to Today's date add it to the upcoming
                    // task list
                    else if (task.DueDate.ToDateTime().Date >= DateTime.UtcNow.AddDays(1).Date)
                    {
                        if (m_upcomingTasks == null)
                            m_upcomingTasks = new Dictionary<string, Tasks>();
                        m_upcomingTasks.Add(uid, task);
                    }

                    // Else if the Due date is less than Today's date and the task is also not completed
                    // we add it to Today's tasks and when the front end code handles the task to display
                    // as a different colour
                    else if (task.DueDate.ToDateTime().Date < DateTime.UtcNow.Date && !task.Completed)
                    {
                        if (m_todayTasks == null)
                            m_todayTasks = new Dictionary<string, Tasks>();
                        m_todayTasks.Add(uid, task);
                    }

                }
            }

            // When a task is added to the Collection on Firestore, we set the HasUpdated
            // bool to true so that when we call GetAllTasks() we can return the locally
            // stored list instead of asking for another update 
            if (HasUpdated)
                HasUpdated = false;


            return m_allTasks;
        }

        public static async Task AddNewTask(Tasks task)
        {
            await m_dbInstance.Collection("tasks").AddAsync(task);
            HasUpdated = true;
            await GetAllTasks();
        }

        public static async Task DeleteTask(string uid)
        {
            DocumentReference docRef = m_dbInstance.Document("tasks/" + uid);
            m_allTasks.Remove(uid);
            if (m_todayTasks.ContainsKey(uid))
                m_todayTasks.Remove(uid);
            if (m_upcomingTasks.ContainsKey(uid))
                m_upcomingTasks.Remove(uid);

            await docRef.DeleteAsync();
            HasUpdated = true;
        }
        #endregion

        #region User Related Methods
        public static async Task AddNewUser(Users user)
        {
            if (m_allUsers == null)
                m_allUsers = new Dictionary<string, Users>();
            await m_dbInstance.Collection("Users").AddAsync(user);
            await GetUsers();
        }

        public static async Task<string> GetUserID()
        {
            return m_currentUser.Uid;
        }
        public static async Task<Dictionary<string, Users>> GetUsers()
        {
            CollectionReference coll = GetDBInstance().Collection("Users");
            QuerySnapshot docs = await coll.GetSnapshotAsync();


            foreach (var doc in docs.Documents)
            {
                // Store the current id of the doc
                var uid = doc.Id;

                if (!m_allUsers.TryGetValue(uid, out var existingUser))
                {
                    var user = new Users
                    {
                        Uid = doc.Id,
                        UserName = doc.GetValue<string>("UserName"),
                        Email = doc.GetValue<string>("Email"),
                        DayReg = doc.GetValue<Timestamp>("DayReg")
                    };

                    m_allUsers.Add(uid, user);

                }
            }
            Console.WriteLine("There are " + m_allUsers.Count() + " users");
            return m_allUsers;
        }
        #endregion


        /// <summary>
        /// Get the current instance of the DB that is being used and if it is null
        /// create a new instance
        /// </summary>
        /// <returns></returns>
        public static FirestoreDb GetDBInstance()
        {
            if (m_dbInstance == null)
            {
                m_dbInstance = FirestoreDb.Create("hons-project-f5a1e");
            }

            return m_dbInstance;
        }

        /// <summary>
        /// Gets all the Tasks from the Firestore DB
        /// </summary>
        /// <returns>List of Tasks</returns>
        public static async Task<Dictionary<string, Tasks>> GetAllTasks()
        {
            if (m_allTasks == null)
            {
                m_allTasks = new Dictionary<string, Tasks>();
            }

            // If the Hasupdated variable has been set to true get an updated
            // version of the list
            if (HasUpdated || IsSetup)
            {
                return await GetTasks();
            }
            else
            {
                // Return the local list instead of a new updated list if HasUpdated 
                // is false

                return m_allTasks;
            }
        }

        public static async Task<Dictionary<string, Tasks>> GetUserTasks(TaskType type)
        {

            await GetTasks();

            var ret = new Dictionary<string, Tasks>();
            switch (type)
            {
                case TaskType.Today:
                    if (m_todayTasks != null)
                    {
                        var filtter = m_todayTasks.Where(x => x.Value.UserId == m_currentUser.Uid);
                        return filtter.ToDictionary(x => x.Key, x => x.Value);
                    }

                    break;
                case TaskType.Upcoming:
                    if (m_upcomingTasks != null)
                    {
                        var filtter = m_upcomingTasks.Where(x => x.Value.UserId == m_currentUser.Uid);
                        return filtter.ToDictionary(x => x.Key, x => x.Value);
                    }
                    break;
                case TaskType.All:
                    if (m_allTasks != null)
                    {
                        var filtter = m_allTasks.Where(x => x.Value.UserId == m_currentUser.Uid);
                        return filtter.ToDictionary(x => x.Key, x => x.Value);
                    }
                    break;
                default:
                    return ret;
            }
            return m_userTasks;
        }
        public static async Task UpdateUser(Users user)
        {
            DocumentReference docRef = m_dbInstance.Collection("Users").Document(user.Uid);

            m_allUsers[user.Uid] = user;
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                {"Exp", user.Exp},
                {"Level", user.Level},
                {"RequiredExp", user.requiredExp}
            };
            DocumentSnapshot snap = await docRef.GetSnapshotAsync();

            if (snap.Exists)
            {
                await docRef.UpdateAsync(updates);
            }
        }
        public static Dictionary<string, Tasks> GetUsersTodayTasks()
        {
            if (m_todayTasks == null)
            {
                m_todayTasks = new Dictionary<string, Tasks>();
            }


            foreach (var task in m_allTasks)
            {
                Console.WriteLine("Hit 03");
                if (task.Value.UserId == m_currentUser.Uid && !m_userTasks.TryGetValue(task.Key, out var existingTask))
                {
                    Console.WriteLine("Hit 04");
                    m_userTasks.Add(task.Key, task.Value);
                }
            }

            return m_userTasks;
        }
        public static int GetUsersLevel()
        {

            int ret = 0;

            foreach (var user in m_allUsers)
            {
                if (user.Key == m_currentUser.Uid)
                {
                    ret = user.Value.Level;
                    return ret;
                }
            }
            return ret;
        }



        public static void SetCurrentUser(User user)
        {
            m_currentUser = user;
            CurrentLocalUser = m_allUsers.Where(x => x.Value.Uid == user.Uid).FirstOrDefault().Value;
        }

        public static User CurrentUser()
        {
            return m_currentUser;
        }

        public static Users LoggedInUser()
        {
            foreach (var user in m_allUsers)
            {
                if (user.Key == m_currentUser.Uid)
                {

                    return user.Value;
                }
            }
            return new Users();
        }
    }
}