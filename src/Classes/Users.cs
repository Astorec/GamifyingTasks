using Google.Cloud.Firestore;

namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class Users
    {
        [FirestoreProperty]
        public string Uid {get; set;}
        [FirestoreProperty]
        public string UserName { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public Timestamp DayReg { get; set; }
        [FirestoreProperty]
        public int Level {get; set;} = 1;
        [FirestoreProperty]
        public int Exp {get; set;} = 0;
        [FirestoreProperty]
        public int requiredExp {get; set;} = 200;
    }
}