using Firebase.Auth;
using GamifyingTasks.Classes;

namespace GamifyingTasks.Firebase.DB.Interfaces{
    public interface IUsers{
        public Users GetUser(string uid);
        public Task CreateUser(Users user);
        public Task UpdateUser(Users user);
        public Task Login(User user);
        public Task<Users> DeleteUser(string uid);
    }
}