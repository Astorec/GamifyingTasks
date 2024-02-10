using Google.Cloud.Firestore;
namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class Reminders
    {
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string ReminderName { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
    }
}