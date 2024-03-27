using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;
using GamifyingTasks.Firebase.Authentication;
using Google.Cloud.Firestore;
using Grpc.Auth;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreGoals : IGoals
    {
        readonly IDBCore? dBCore;
        readonly IUsers _users;

        private List<UserGoals> m_userGoals = new List<UserGoals>();

        public DBCoreGoals(IDBCore dBCore, IUsers users)
        {
            this.dBCore = dBCore;
            _users = users;
        }

        public async Task CreateGoal(UserGoals userGoals)
        {
            if (dBCore != null)
            {

                var docRef = await dBCore.GetDB().Collection("Goals").AddAsync(userGoals);

                 await docRef.UpdateAsync("UID", docRef.Id);
            }

        }

        public async Task DeleteGoal(string uid)
        {
            var docRef = await dBCore.GetDB().Collection("Goals").Document(uid).DeleteAsync();

            // Remvoe Goal from List
            m_userGoals.RemoveAll(x => x.UID == uid);
        }

        public List<UserGoals> GetGoals()
        {
            return m_userGoals;
        }

        public async Task InitGoals()
        {
           var docSnap = await dBCore.GetDB().Collection("Goals").GetSnapshotAsync();

            foreach (DocumentSnapshot doc in docSnap.Documents)
            {
                UserGoals userGoal = doc.ConvertTo<UserGoals>();

                if (userGoal.UserId == _users.GetUser().Uid)
                {
                    m_userGoals.Add(userGoal);
                }
            }
        }
    }
}