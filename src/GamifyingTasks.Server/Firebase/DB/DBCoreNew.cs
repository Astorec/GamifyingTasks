using FirebaseAdmin;
using GamifyingTasks.Firebase.DB.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace GamifyingTasks.Firebase.DB
{
    public class DBCoreNew : IDBCore
    {

        private  FirebaseApp m_app;
        private FirestoreDb m_dbInstance;
        DBCoreNew()
        {
            m_app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),

                ProjectId = "hons-project-f5a1e",
            });
        }

        public void Close()
        {
           throw new NotImplementedException();
        }

        public void CreateDB()
        {
             m_dbInstance = FirestoreDb.Create("hons-project-f5a1e");
        }

        public FirebaseApp GetApp()
        {
            return m_app;
        }

        public FirestoreDb GetDB()
        {
            return m_dbInstance;
        }
    }
}