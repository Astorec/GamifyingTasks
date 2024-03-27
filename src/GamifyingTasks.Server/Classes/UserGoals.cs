using Google.Cloud.Firestore;

namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class UserGoals
    {
        [FirestoreProperty]
        public string UID { get; set; }
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string GoalName { get; set; }
        [FirestoreProperty]
        public int GoalLevel { get; set; }
        [FirestoreProperty]
        public string Reward { get; set; }
    }
}