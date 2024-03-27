using Google.Cloud.Firestore;
namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class Tasks
    {
        [FirestoreProperty]
        public string UID { get; set; }
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Difficulty { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public bool Completed { get; set; } = false;
        [FirestoreProperty]
        public Timestamp DueDate { get; set; }


        public void SetDueDate(DateTime value)
        {
            DueDate = Timestamp.FromDateTime(value);
        }
    }
}