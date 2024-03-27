using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Storage;

namespace GamifyingTasks.Firebase.Authentication
{
    public static class FirebaseAuth
    {
        // FirebaseAuthClient is a class that represents a Firebase Authentication Client.
        private static FirebaseAuthClient m_client;
        
        /// <summary>
        /// init creates a new FirebaseAuthClient instance.
        /// </summary>
        /// <returns></returns>
        private static FirebaseAuthClient init()
        {
            // Create a new FirebaseAuthConfig instance.
            var config = new FirebaseAuthConfig
            {
                ApiKey = " ",
                AuthDomain = "hons-project-f5a1e.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]{
                new EmailProvider()
            }
            };
            return new FirebaseAuthClient(config);

        }        

        /// <summary>
        /// GetClient returns the FirebaseAuthClient instance.
        /// </summary>
        /// <returns></returns>
        public static FirebaseAuthClient GetClient()
        {
            if (m_client == null)
                m_client = init();
            return m_client;
        }
    }
}
