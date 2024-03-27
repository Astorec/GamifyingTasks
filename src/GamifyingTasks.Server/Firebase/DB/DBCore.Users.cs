using Firebase.Auth;
using GamifyingTasks.Firebase.Authentication;
using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB.Interfaces;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreUsers : IUsers
    {
        // IDBCore is an interface that represents the DBCore class.
        readonly IDBCore? dBCore;
        // Instance of the Users
        private Users m_currentUser = new Users();
        // Boolean to check if the user is logged in
        private bool m_isLoggedIn = false;

        /// <summary>
        /// Constructor for DBCoreUsers
        /// </summary>
        /// <param name="dBCore"></param>
        public DBCoreUsers(IDBCore dBCore)
        {
            this.dBCore = dBCore;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUser(Users user)
        {
            // Ensure that DBCore is not null
            if (dBCore != null)
            {
                // Add the user to the DB
                await dBCore.GetDB().Collection("Users").AddAsync(user);

                // Set the current user to the user that was just created
                m_currentUser = user;

                // Set the logged in status to true
                m_isLoggedIn = true;
            }
        }

        /// <summary>
        /// Delete a user. To Be implemented
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Users> DeleteUser(string uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the current user
        /// </summary>
        /// <returns>
        ///  Current User
        /// </returns>
        public Users GetUser()
        {
            return m_currentUser;
        }

        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Login(User user)
        {
            // Ensure that DBCore is not null
            if (dBCore != null)
            {
                // find the user that matches the email of the user that is trying to log in
                var userQuery = dBCore.GetDB().Collection("Users").WhereEqualTo("Email", user.Info.Email);
                var userQuerySnapshot = await userQuery.GetSnapshotAsync();

                // Set the current user to the user that matches the logged in user
                foreach (var doc in userQuerySnapshot.Documents)
                {
                    m_currentUser = doc.ConvertTo<Users>();
                }

                m_isLoggedIn = true;
            }
        }

        /// <summary>
        /// Log out the user
        /// </summary>
        /// <returns></returns>
        public async Task LogOut()
        {
            if (dBCore != null)
            {
                // Logout via FirebaseAuth
                FirebaseAuth.GetClient().SignOut();

                // Set the current user to null
                m_currentUser = new Users();

                // Set the logged in status to false
                m_isLoggedIn = false;
            }
        }

        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateUser(Users user)
        {
            if (dBCore != null)
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

        /// <summary>
        /// Check if the user is logged in
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn()
        {

            return m_isLoggedIn;
        }
    }
}