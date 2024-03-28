using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;
using GamifyingTasks.Firebase.Authentication;
using Google.Cloud.Firestore;
using Grpc.Auth;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreGoals : IGoals
    {
        // DBCore instance
        readonly IDBCore? dBCore;

        // Users instance
        readonly IUsers _users;

        // List to store the goals
        private List<UserGoals> m_userGoals = new List<UserGoals>();

        /// <summary>
        /// Constructor for DBCoreGoals
        /// </summary>

        public DBCoreGoals(IDBCore dBCore, IUsers users)
        {
            this.dBCore = dBCore;
            _users = users;
        }


        /// <summary>
        /// CreateGoal creates a new goal in the database
        /// </summary>
        /// <param name="userGoals"></param>
        /// <returns></returns>
        public async Task CreateGoal(UserGoals userGoals)
        {
            if (dBCore != null)
            {
                // Add the goal to the database
                var docRef = await dBCore.GetDB().Collection("Goals").AddAsync(userGoals);

                await docRef.UpdateAsync("UID", docRef.Id);

                m_userGoals.Add(userGoals);
            }

        }


        /// <summary>
        /// UpdateGoal updates a goal in the database
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task DeleteGoal(string uid)
        {
            // Delete the goal from the database
            var docRef = await dBCore.GetDB().Collection("Goals").Document(uid).DeleteAsync();

            // Remvoe Goal from List
            m_userGoals.RemoveAll(x => x.UID == uid);
        }

        /// <summary>
        /// InitGoals gets the goals from the database and stores them in the list
        ///  </summary>
        ///  <returns></returns>
        public List<UserGoals> GetGoals()
        {
            return m_userGoals;
        } 


        /// <summary>
        /// InitGoals gets the goals from the database and stores them in the list
        /// </summary>
        /// <returns></returns>
        public async Task InitGoals()
        {
            // Get the goals from the database
            var docSnap = await dBCore.GetDB().Collection("Goals").GetSnapshotAsync();

            foreach (DocumentSnapshot doc in docSnap.Documents)
            {
                // Convert the document to a UserGoals object
                UserGoals userGoal = doc.ConvertTo<UserGoals>();

                // Add the goal to the list
                if (userGoal.UserId == _users.GetUser().Uid)
                {
                    m_userGoals.Add(userGoal);
                }
            }
        }
    }
}