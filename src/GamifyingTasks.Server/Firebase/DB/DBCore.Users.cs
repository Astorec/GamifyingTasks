using Firebase.Auth;
using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreUsers : IUsers
    {
        IDBCore dBCore;
        private Users m_currentUser = new Users();
        public async Task CreateUser(Users user)
        {
            await dBCore.GetDB().Collection("Users").AddAsync(user);
            m_currentUser = user;
        }

        public Task<Users> DeleteUser(string uid)
        {
            throw new NotImplementedException();
        }

        public Users GetUser(string uid)
        {
            return m_currentUser;
        }

        public async Task Login(User user)
        {
           // m_currentUser = user;
            // Get the user the matches the logged in user
            var userQuery = dBCore.GetDB().Collection("Users").WhereEqualTo("Email", user.Info.Email);
            var userQuerySnapshot = await userQuery.GetSnapshotAsync();

            // Set the current user to the user that matches the logged in user
            foreach (var doc in userQuerySnapshot.Documents)
            {
                m_currentUser = doc.ConvertTo<Users>();
            }
        }

        public async Task UpdateUser(Users user)
        {
           // get the user that matches the current user
            var userQuery = dBCore.GetDB().Collection("Users").WhereEqualTo("Email", m_currentUser.Email);
            var userQuerySnapshot = await userQuery.GetSnapshotAsync();

            // Update the user
            foreach (var doc in userQuerySnapshot.Documents)
            {
                await doc.Reference.SetAsync(user);
            }
        }
    }
}