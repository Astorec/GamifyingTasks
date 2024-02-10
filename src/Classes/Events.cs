using Google.Cloud.Firestore;
namespace GamifyingTasks.Classes
{
    [FirestoreData]
    public class Events
    {
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string EventName { get; set; }
        [FirestoreProperty]
        public string EventLocation { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public Timestamp EventDate { get; set; }


        public void SetEventDate(DateTime value)
        {
            EventDate = Timestamp.FromDateTime(value);
        }
    }
}