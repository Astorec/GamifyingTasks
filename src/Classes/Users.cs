using Google.Cloud.Firestore;

namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class Users
    {
        [FirestoreProperty]
        public string Uid { get; set; }
        [FirestoreProperty]
        public string UserName { get; set; }
        [FirestoreProperty]
        public string PfpUrl { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public Timestamp DayReg { get; set; }
        [FirestoreProperty]
        public int Level { get; set; }
        [FirestoreProperty]
        public int Exp { get; set; }
        [FirestoreProperty]
        public int requiredExp { get; set; }
        [FirestoreProperty]
        public Timestamp LastLogin { get; set; }
        [FirestoreProperty]
        public int LoginStreak { get; set; }
        [FirestoreProperty]
        public int LongestStreak { get; set; }
    }
}