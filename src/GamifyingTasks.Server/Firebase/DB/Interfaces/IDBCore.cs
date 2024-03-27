using FirebaseAdmin;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB.Interfaces
{
    /// <summary>
    /// Interface for DBCore
    /// </summary>
    public interface IDBCore
    {
        /// <summary>
        ///   GetApp returns the FirebaseApp instance.
        /// </summary>
        /// <returns></returns>
        public FirebaseApp GetApp();

        /// <summary>
        /// CreateDB creates a new FirestoreDb instance.
        /// </summary>
        public void CreateDB();

        /// <summary>
        /// GetDB returns the FirestoreDb instance.
        /// </summary>
        /// <returns>
        ///  instance of FirestoreDb
        /// </returns>
        public FirestoreDb GetDB();

        /// <summary>
        /// Close closes the FirestoreDb instance. To Be Implemented
        /// </summary>
        public void Close();
    }
}