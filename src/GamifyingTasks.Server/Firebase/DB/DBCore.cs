using FirebaseAdmin;
using GamifyingTasks.Firebase.DB.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCore : IDBCore
    {
        // FirebaseApp is a class that represents a Firebase App.
        private FirebaseApp m_app;

        // FirestoreDb is a class that represents a Firestore Database.
        private FirestoreDb m_dbInstance;

        /// <summary>
        /// Constructor for DBCoreNew
        /// </summary>
        public DBCore()
        {
            // Create a new FirebaseApp instance if one does not already exist.
            m_app = FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
                ProjectId = "hons-project-f5a1e",
            });

            // Create a new FirestoreDb instance.
            CreateDB();

        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CreateDB creates a new FirestoreDb instance.
        /// </summary>

        public void CreateDB()
        {
            m_dbInstance = FirestoreDb.Create("hons-project-f5a1e");
        }


        /// <summary>
        /// GetApp returns the FirebaseApp instance.
        /// </summary>
        /// <returns>
        /// instance of FirebaseApp
        /// </returns>
        public FirebaseApp GetApp()
        {
            return m_app;
        }

        /// <summary>
        /// GetDB returns the FirestoreDb instance.
        /// </summary>
        /// <returns>
        /// instance of FirestoreDb
        /// </returns>
        public FirestoreDb GetDB()
        {
            return m_dbInstance;
        }
    }
}