using FirebaseAdmin;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    public interface IDBCore
    {
        public FirebaseApp GetApp();
        public void CreateDB();
        public FirestoreDb GetDB();
        public void Close();
    }
}